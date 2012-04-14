param(
$configuration = "Release"
)

function UpdatePackages
{
	$packageConfigs = Get-ChildItem -Path .\ -Include "packages.config" -Recurse
	foreach($config in $packageConfigs){
		.\.nuget\nuget.exe i $config.FullName -o packages -source https://nuget.org/api/v2/
	}
}

function BuildSolution
{
	msbuild .\Linq2Rest.sln /p:configuration=$configuration
}

function RunTests
{
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Tests\bin\$configuration\Linq2Rest.Tests.dll
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Reactive.Tests\bin\$configuration\Linq2Rest.Reactive.Tests.dll
}

function PublishPackage
{
	.\.nuget\nuget.exe pack Linq2Rest.nuspec
	.\.nuget\nuget.exe pack Linq2Rest.Mvc.nuspec
	.\.nuget\nuget.exe pack Linq2Rest.Reactive.nuspec
}

UpdatePackages
BuildSolution
RunTests
PublishPackage