#r @"tools\FAKE\tools\FakeLib.dll"

open System.IO
open System.Linq
open Fake

// --------------------------------------------------------------------------------------
// Definitions

let binProjectName = "Orc.Sort"
let testProjectName = "Orc.Sort.Tests"
let netVersion = "NET40"

let srcDir  = @".\src\"
let deploymentDir  = @".\deployment\"
let packagesDir = deploymentDir @@ "packages"
let nugetPath = srcDir @@ @"\.nuget\nuget.exe"

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


// --------------------------------------------------------------------------------------
// Run tests


// --------------------------------------------------------------------------------------
// Build a NuGet package


// --------------------------------------------------------------------------------------
// Combined targets

Target "Clean" DoNothing
"CleanPackagesDirectory" ==> "DeleteOutputFiles" ==> "DeleteOutputDirectories" ==> "Clean"

Target "Build" DoNothing

Target "Tests" DoNothing

Target "All" DoNothing
"Clean" ==> "All"
"Build" ==> "All"
"Tests" ==> "All"

Target "Release" DoNothing
"All" ==> "Release"
 
RunTargetOrDefault "All"
