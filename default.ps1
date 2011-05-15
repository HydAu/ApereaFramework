﻿
import-module .\tools\psake-version.psm1

Properties {
    $project_dir = Split-Path $psake.build_script_file
    $src_dir = "$project_dir\src"
    $out_dir = "$project_dir\out"
    
    $nuget = "$project_dir\tools\nuget.exe"
    $nuspec_dir = "$project_dir\nuspecs"
    $nupgk_dir = "$project_dir\nupkg"
    $version_file = "$src_dir\FrameworkVersion.cs"
}

Task Default -Depends Build

Task Clean {
    Write-Host "Clean all Builds" -ForegroundColor Green
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Clean /v:quiet /p:Configuration=Release } 
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Clean /v:quiet /p:Configuration=Debug } 
}


Task BuildWithoutBump {
    Write-Host "ApereaFramework.All.sln" -ForegroundColor Green
    BumpRevision $version_file
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Build /v:quiet /p:Configuration=Release /p:OutDir="$out_dir\" } 
}
Task Build -Depends BumpRevision,Clean {
    Write-Host "ApereaFramework.All.sln" -ForegroundColor Green
    BumpRevision $version_file
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Build /v:quiet /p:Configuration=Release /p:OutDir="$out_dir\" } 
}

Task BumpRevision {
    BumpRevision $version_file
}


Task BumpBuildVersion {
    BumpBuildVersion $version_file

}

Task BumpMinorVersion {
    BumpMinorVersion $version_file
}

Task BumpMajorVersion {
    BumpBuildVersion $version_file
}

Task SetPackageVersion {
    Write-Host "Building NgauGet-Packages" -ForegroundColor Green
    $version = Get-AssemblyInfoVersion $version_file
    $depVersion = "[$version]"
    Set-PackageVersion "$nuspec_dir\Aperea.Core.nuspec" $version
    Set-PackageVersion "$nuspec_dir\Aperea.Mail.nuspec" $version @{"Aperea.Core"=$depVersion}
    Set-PackageVersion "$nuspec_dir\Aperea.Membership.nuspec" $version @{"Aperea.Mail"=$depVersion}
    Set-PackageVersion "$nuspec_dir\Aperea.Mvc.nuspec" $version @{"Aperea.Core"=$depVersion}
}

Task NuGet -Depends BuildWithoutBump, SetPackageVersion  {
    Write-Host "Building NuGet-Packages" -ForegroundColor Green   
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Core.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Mail.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Membership.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Mvc.nuspec" /OutputDirectory "$nupgk_dir\" }    
}
