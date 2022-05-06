// This file is part of the re-motion Framework (www.re-motion.org)
// Copyright (c) rubicon IT GmbH, www.rubicon.eu
// 
// re-motion Framework is free software; you can redistribute it
// and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation; either version 2.1 of the
// License, or (at your option) any later version.
// 
// re-motion is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-motion; if not, see http://www.gnu.org/licenses.

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Infrastructure.SourceGenerators.InternalsVisibleTo.Tests
{
  public class CSharpSourceGeneratorVerifier<TSourceGenerator>
      where TSourceGenerator : ISourceGenerator, new()
  {
    public class Test : CSharpSourceGeneratorTest<TSourceGenerator, NUnitVerifier>
    {
      public bool SignAssembly { get; set; } = false;
      
      protected override CompilationOptions CreateCompilationOptions ()
      {
        var compilationOptions = base.CreateCompilationOptions();
        compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
            compilationOptions.SpecificDiagnosticOptions.SetItems(GetNullableWarningsFromCompiler()));

        if (SignAssembly)
        {
          compilationOptions = compilationOptions.WithCryptoPublicKey(WellKnownPublicKey.Data.ToImmutableArray())
              .WithPublicSign(true);
        }

        return compilationOptions;
      }

      public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

      private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler ()
      {
        string[] args = {"/warnaserror:nullable"};
        var commandLineArguments = CSharpCommandLineParser.Default.Parse(
            args,
            baseDirectory: Environment.CurrentDirectory,
            sdkDirectory: Environment.CurrentDirectory);
        var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

        return nullableWarnings;
      }

      protected override ParseOptions CreateParseOptions ()
      {
        return ((CSharpParseOptions) base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
      }
    }
  }
}