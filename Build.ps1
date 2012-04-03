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
	.\packages\NUnit.Runners\tools\nunit-console.exe .\Linq2Rest.Tests\bin\$configuration\Linq2Rest.Tests.dll
	.\packages\NUnit.Runners\tools\nunit-console.exe .\Linq2Rest.Rx.Tests\bin\$configuration\Linq2Rest.Reactive.Tests.dll
}

function PublishPackage
{
	.\nuget.exe pack Linq2Rest.nuspec
	.\nuget.exe pack Linq2Rest.Mvc.nuspec
	.\nuget.exe pack Linq2Rest.Reactive.nuspec
}

UpdatePackages
BuildSolution
RunTests
PublishPackage