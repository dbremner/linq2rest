// <copyright file="PexAssemblyInfo.cs" company="Reimers.dk">Copyright © Reimers.dk 2012</copyright>
using Microsoft.Pex.Framework.Coverage;
using Microsoft.Pex.Framework.Creatable;
using Microsoft.Pex.Framework.Instrumentation;
using Microsoft.Pex.Framework.Moles;
using Microsoft.Pex.Framework.Settings;
using Microsoft.Pex.Framework.Validation;
using Microsoft.Pex.Linq;

// Microsoft.Pex.Framework.Settings
[assembly: PexAssemblySettings(TestFramework = "NUnit")]

// Microsoft.Pex.Framework.Instrumentation
[assembly: PexAssemblyUnderTest("Linq2Rest.Reactive")]
[assembly: PexInstrumentAssembly("System.Core")]
[assembly: PexInstrumentAssembly("Microsoft.CSharp")]
[assembly: PexInstrumentAssembly("System.Reactive")]
[assembly: PexInstrumentAssembly("System.Reactive.Providers")]
[assembly: PexInstrumentAssembly("Linq2Rest")]

// Microsoft.Pex.Framework.Creatable
[assembly: PexCreatableFactoryForDelegates]

// Microsoft.Pex.Framework.Validation
[assembly: PexAllowedContractRequiresFailureAtTypeUnderTestSurface]
[assembly: PexAllowedXmlDocumentedException]

// Microsoft.Pex.Framework.Coverage
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Core")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Microsoft.CSharp")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Reactive")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "System.Reactive.Providers")]
[assembly: PexCoverageFilterAssembly(PexCoverageDomain.UserOrTestCode, "Linq2Rest")]

// Microsoft.Pex.Framework.Moles
[assembly: PexAssumeContractEnsuresFailureAtBehavedSurface]
[assembly: PexChooseAsBehavedCurrentBehavior]

// Microsoft.Pex.Linq
[assembly: PexLinqPackage]

