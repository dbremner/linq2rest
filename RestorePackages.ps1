function UpdatePackages
{
	$packageConfigs = Get-ChildItem -Path .\ -Include "packages.config" -Recurse
	foreach($config in $packageConfigs){
		.\.nuget\nuget.exe i $config.FullName -o .\packages -source https://nuget.org/api/v2/
	}
}

UpdatePackages