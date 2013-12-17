#r @"tools\FAKE\tools\FakeLib.dll"

open Fake

// --------------------------------------------------------------------------------------
// Definitions


// --------------------------------------------------------------------------------------
// Clean build results


// --------------------------------------------------------------------------------------
// Build projects


// --------------------------------------------------------------------------------------
// Run tests


// --------------------------------------------------------------------------------------
// Build a NuGet package


// --------------------------------------------------------------------------------------
// Combined targets

Target "Clean" DoNothing

Target "Build" DoNothing

Target "Tests" DoNothing

Target "All" DoNothing
"Clean" ==> "All"
"Build" ==> "All"
"Tests" ==> "All"

Target "Release" DoNothing
"All" ==> "Release"
 
RunTargetOrDefault "All"
