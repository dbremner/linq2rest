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

function RunTests
{
	.\packages\NUnit.2.5.10.11092\tools\nunit-console.exe .\Linq2Rest.Tests\bin\$configuration\Linq2Rest.Tests.dll
}

function PublishPackage
{
	.\nuget.exe pack Linq2Rest.nuspec
	.\nuget.exe pack Linq2Rest.Mvc.nuspec
}

UpdatePackages
BuildSolution
RunTests
PublishPackage