﻿namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("FsCheck.Xunit")>]
[<assembly: AssemblyProductAttribute("FsCheck.Xunit")>]
[<assembly: AssemblyDescriptionAttribute("Integrates FsCheck with xUnit.NET")>]
[<assembly: AssemblyVersionAttribute("2.2.5")>]
[<assembly: AssemblyFileVersionAttribute("2.2.5")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "2.2.5"
