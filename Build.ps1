param(
$configuration = "Release"
)

function UpdatePackages
{
	$packageConfigs = Get-ChildItem -Path .\ -Include "packages.config" -Recurse
	foreach($config in $packageConfigs){
		.\nuget.exe i $config.FullName -o packages -x
	}
}

function BuildSolution
{
	msbuild .\Linq2Rest.sln /p:configuration=$configuration
}

function CreatePackage
{
	.\nuget.exe pack Linq2Rest.nuspec
	.\nuget.exe pack Linq2Rest.Mvc.nuspec
}

UpdatePackages
BuildSolution
CreatePackage