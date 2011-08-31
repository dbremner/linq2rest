function UpdatePackages
{
	$packageConfigs = Get-ChildItem -Path .\ -Include "packages.config" -Recurse
	foreach($config in $packageConfigs){
		.\nuget.exe i $config.FullName -o .\packages -x
	}
}

UpdatePackages