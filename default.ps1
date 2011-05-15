Properties {
    $project_dir = Split-Path $psake.build_script_file
    $src_dir = "$project_dir\src"
    $out_dir = "$project_dir\out"
}

Task Default -Depends Build


Task Build  {
    Write-Host "ApereaFramework.All.sln" -ForegroundColor Green
	Exec { msbuild "$src_dir\ApereaFramework.All.sln" /t:Build /p:Configuration=Release /p:OutDir="$out_dir\" } 
}