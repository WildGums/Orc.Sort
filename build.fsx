#r @"tools\FAKE\tools\FakeLib.dll"

open System
open System.IO
open System.Linq
open System.Text
open System.Text.RegularExpressions
open Fake

// --------------------------------------------------------------------------------------
// Definitions

let binProjectName = "Orc.Sort"
let testProjectName = "Orc.Sort.Tests"
let netVersion = "NET40"

let srcDir  = @".\src\"
let deploymentDir  = @".\deployment\"
let packagesDir = deploymentDir @@ "packages"

let dllDeploymentDir = packagesDir @@ @"lib" @@ netVersion
let nuspecTemplatesDir = deploymentDir @@ "templates"

let nugetPath = srcDir @@ @".nuget\nuget.exe"
let nugetPackagesDir = srcDir @@ @"packages"
let nugetAccessKey = if File.Exists(@".\Nuget.key") then File.ReadAllText(@".\Nuget.key") else ""
let version = File.ReadAllText(@".\version.txt")

let solutionAssemblyInfo = srcDir @@ binProjectName @@ "Properties\AssemblyInfo.cs"
let binProjectDependencies:^string list = []

let outputDir = @".\output\"
let outputReleaseDir = outputDir @@ "release" @@ netVersion
let outputBinDir = outputReleaseDir @@ binProjectName
let outputTestDir = outputReleaseDir @@ testProjectName
let testResultsDir = outputDir @@ "TestResults"

let ignoreBinFiles = "*.vshost.exe"
let ignoreBinFilesPattern = @"\**\" @@ ignoreBinFiles
let outputBinFiles = !! (outputBinDir @@ @"\**\*.*")
                            -- ignoreBinFilesPattern

let tests = srcDir @@ @"\**\*.Test.csproj" 
let allProjects = srcDir @@ @"\**\*.csproj" 

let testProjects  = !! tests
let otherProjects = !! allProjects
                        -- tests

// --------------------------------------------------------------------------------------
// Clean build results

Target "CleanPackagesDirectory" (fun _ ->
    CleanDirs [packagesDir]
)

Target "DeleteOutputFiles" (fun _ ->
    !! (outputBinDir + @"\**\*.*")
        ++ (outputTestDir + @"\**\*.*")
        -- ignoreBinFilesPattern
    |> DeleteFiles
)

Target "DeleteOutputDirectories" (fun _ ->
    CreateDir outputDir
    DirectoryInfo(outputDir).GetDirectories("*", SearchOption.AllDirectories)
    |> Array.filter (fun d -> not (d.GetFiles(ignoreBinFiles, SearchOption.AllDirectories).Any()))
    |> Array.map (fun d -> d.FullName)
    |> DeleteDirs
)

// --------------------------------------------------------------------------------------
// Build projects

Target "UpdateAssemblyVersion" (fun _ ->
      let pattern = Regex("Assembly(|File)Version(\w*)\(.*\)")
      let result = "Assembly$1Version$2(\"" + version + "\")"
      let content = File.ReadAllLines(solutionAssemblyInfo, Encoding.Unicode)
                    |> Array.map(fun line -> pattern.Replace(line, result, 1))
      File.WriteAllLines(solutionAssemblyInfo, content, Encoding.Unicode)
)

Target "BuildOtherProjects" (fun _ ->    
    otherProjects
      |> MSBuildRelease "" "Rebuild" 
      |> Log "Build Other Projects"
)

Target "BuildTests" (fun _ ->    
    testProjects
      |> MSBuildRelease "" "Build"
      |> Log "Build Tests"
)

// --------------------------------------------------------------------------------------
// Run tests

Target "RunTests" (fun _ ->
    let nUnitVersion = GetPackageVersion nugetPackagesDir "NUnit.Runners"
    let nUnitPath = sprintf "%s/NUnit.Runners.%s/Tools" nugetPackagesDir nUnitVersion
    ActivateFinalTarget "CloseNUnitTestRunner"
    CleanDir testResultsDir
    CreateDir testResultsDir

    !! (outputDir + @"\**\*.Test.dll") 
      ++ (outputDir + @"\**\*.Tests.dll") 
      |> NUnit (fun p -> 
                 {p with 
                   ToolPath = nUnitPath
                   DisableShadowCopy = true
                   TimeOut = TimeSpan.FromMinutes 20.
                   OutputFile = testResultsDir @@ "TestResults.xml"})
)

FinalTarget "CloseNUnitTestRunner" (fun _ ->  
    ProcessHelper.killProcess "nunit-agent.exe"
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    let nugetAccessPublishKey = getBuildParamOrDefault "nugetkey" nugetAccessKey
    let getOutputFile ext = sprintf @"%s\%s.%s" outputBinDir binProjectName ext
    let binProjectFiles = !! (getOutputFile "dll")
                        ++ (getOutputFile "xml")
    let nugetDependencies = binProjectDependencies
                              |> List.map (fun d -> d, GetPackageVersion nugetPackagesDir d)
    
    let getNupkgFile = sprintf "%s\%s.%s.nupkg" dllDeploymentDir binProjectName version
    let getNuspecFile = sprintf "%s\%s.nuspec" nuspecTemplatesDir binProjectName

    let preparePackage filesToPackage = 
        CreateDir packagesDir
        CreateDir dllDeploymentDir
        CopyFiles dllDeploymentDir filesToPackage

    let cleanPackage name = 
        MoveFile packagesDir getNupkgFile
        DeleteDir (packagesDir @@ "lib")

    let doPackage dependencies =   
        NuGet (fun p -> 
            {p with
                Project = binProjectName
                Version = version
                ToolPath = nugetPath
                OutputPath = dllDeploymentDir
                WorkingDir = packagesDir
                Dependencies = dependencies
                Publish = not (String.IsNullOrEmpty nugetAccessPublishKey)
                AccessKey = nugetAccessPublishKey })
                getNuspecFile
    
    let doAll files depenencies =
        preparePackage files
        doPackage depenencies
        cleanPackage ""

    doAll binProjectFiles nugetDependencies
)

// --------------------------------------------------------------------------------------
// Combined targets

Target "Clean" DoNothing
"CleanPackagesDirectory" ==> "DeleteOutputFiles" ==> "DeleteOutputDirectories" ==> "Clean"

Target "Build" DoNothing
"UpdateAssemblyVersion" ==> "BuildOtherProjects" ==> "Build"

Target "Tests" DoNothing
"BuildTests" ==> "RunTests" ==> "Tests"

Target "All" DoNothing
"Clean" ==> "All"
"Build" ==> "All"
"Tests" ==> "All"

Target "Release" DoNothing
"All" ==> "Release"
"NuGet" ==> "Release"
 
RunTargetOrDefault "All"
