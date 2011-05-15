Properties {
    $project_dir = Split-Path $psake.build_script_file
    $src_dir = "$project_dir\src"
    $out_dir = "$project_dir\out"
    
    $nuget = "$project_dir\tools\nuget.exe"
    $nuspec_dir = "$project_dir\nuspecs"
    $nupgk_dir = "$project_dir\nupkg"

}

Task Default -Depends Build

Task Clean
{
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Clean /p:Configuration=Release } 
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Clean /p:Configuration=Debug } 
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Clean /p:Configuration=AutoTest.NET } 

}

Task Build -Depends Clean {
    Write-Host "ApereaFramework.All.sln" -ForegroundColor Green
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Build /p:Configuration=Release /p:OutDir="$out_dir\" } 
}

Task NuGet -Depends Build {
    Write-Host "Building NuGet-Packages" -ForegroundColor Green
    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Core.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Mail.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Membership.nuspec" /OutputDirectory "$nupgk_dir\" }    
    Exec { .\tools\nuget.exe pack "$nuspec_dir\Aperea.Mvc.nuspec" /OutputDirectory "$nupgk_dir\" }    

}