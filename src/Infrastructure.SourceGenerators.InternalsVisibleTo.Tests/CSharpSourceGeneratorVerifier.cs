// SPDX-FileCopyrightText: Copyright (c) rubicon IT GmbH, www.rubicon.eu
// SPDX-License-Identifier: LGPL-2.1-or-later

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