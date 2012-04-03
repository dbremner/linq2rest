﻿// (c) Copyright Reimers.dk.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://www.opensource.org/licenses/MS-PL] for details.
// All other rights reserved.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Linq2Rest")]
[assembly: AssemblyDescription("Generates OData style URL queries from LINQ queries and parses the queries to LINQ serverside.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Reimers.dk")]
[assembly: AssemblyProduct("Linq2Rest")]
[assembly: AssemblyCopyright("Copyright © Reimers.dk 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0228d133-29e3-4baf-9f91-671a6be391bc")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.1.0.0")]
[assembly: AssemblyFileVersion("2.1.0.0")]
[assembly: InternalsVisibleTo("Linq2Rest.Tests")]
[assembly: InternalsVisibleTo("Linq2Rest.Reactive")]
[assembly: InternalsVisibleTo("Linq2Rest.Reactive.Tests")]