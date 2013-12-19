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
let nugetPath = srcDir @@ @".nuget\nuget.exe"
let nugetPackagesDir = srcDir @@ @"packages"
let version = File.ReadAllText(@".\version.txt")

let solutionAssemblyInfo = srcDir @@ binProjectName @@ "Properties\AssemblyInfo.cs"

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
 
RunTargetOrDefault "All"
