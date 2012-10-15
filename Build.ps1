param(
$configuration = "Release",
$platform = "Any CPU",
$folderPath = ".\",
$cleanPackages = $false
)

$oldEnvPath = ""

function CheckMsBuildPath
{
	$envPath = $Env:Path
	if($envPath.Contains("C:\Windows\Microsoft.NET\Framework\v4.0") -eq $false)
	{
		if(Test-Path "C:\Windows\Microsoft.NET\Framework\v4.0.30319")
		{
			$oldEnvPath = $envPath
			$Env:Path = $envPath + ";C:\Windows\Microsoft.NET\Framework\v4.0.30319"
		}
		else
		{
			Write-Host "Could not determine path to MSBuild. Make sure you have .NET 4.0.30319 installed"
		}
	}
}

function CleanUpMsBuildPath
{
	if($oldEnvPath -ne "")
	{
		Write-Host "Reverting Path variable"
		$Env:Path = $oldEnvPath
	}
}

function CleanFolder
{
	Write-Host "Cleaning Folders in $folderPath"
	Get-ChildItem $folderPath -include bin,obj -Recurse | foreach ($_) { remove-item $_.fullname -Force -Recurse }
	if($cleanPackages -eq $true){
		if(Test-Path "$folderPath\packages"){
			Get-ChildItem "$folderPath\packages" -Recurse | Where { $_.PSIsContainer } | foreach ($_) { Write-Host $_.fullname; remove-item $_.fullname -Force -Recurse }
		}
	}
	
	if(Test-Path "$folderPath\BuildOutput"){
		Get-ChildItem "$folderPath\BuildOutput" -Recurse | foreach ($_) { Write-Host $_.fullname; remove-item $_.fullname -Force -Recurse }
	}
}

function UpdatePackages
{
	$packageConfigs = Get-ChildItem -Path .\ -Include "packages.config" -Recurse
	foreach($config in $packageConfigs){
        Write-Host $config.DirectoryName
		.\.nuget\nuget.exe i $config.FullName -o packages -source https://nuget.org/api/v2/
	}
}

function BuildSolution
{
    $options = "/p:configuration=$configuration;platform=$platform"
	msbuild .\Linq2Rest.All.sln $options
}

function RunTests
{
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Tests\bin\v4.0\$configuration\Linq2Rest.Tests.dll
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Tests\bin\v4.5\$configuration\Linq2Rest.Tests.dll
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Reactive.Tests\bin\v4.0\$configuration\Linq2Rest.Reactive.Tests.dll
	.\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe .\Linq2Rest.Reactive.Tests\bin\v4.5\$configuration\Linq2Rest.Reactive.Tests.dll
}

function PublishPackage
{
	.\.nuget\nuget.exe pack Linq2Rest.nuspec
	.\.nuget\nuget.exe pack Linq2Rest.Mvc.nuspec
	.\.nuget\nuget.exe pack Linq2Rest.Reactive.nuspec
}

CheckMsBuildPath
CleanFolder
UpdatePackages
BuildSolution
RunTests
PublishPackage
CleanUpMsBuildPath