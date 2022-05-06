// SPDX-FileCopyrightText: Copyright (c) rubicon IT GmbH, www.rubicon.eu
// SPDX-License-Identifier: LGPL-2.1-or-later

using System;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using VerifyCS =
    Infrastructure.SourceGenerators.InternalsVisibleTo.Tests.CSharpSourceGeneratorVerifier<
        Infrastructure.SourceGenerators.InternalsVisibleTo.InternalsVisibleToSourceGenerator>;

namespace Infrastructure.SourceGenerators.InternalsVisibleTo.Tests
{
  [TestFixture]
  public class InternalsVisibleToSourceGeneratorTest
  {
    private const string c_generatedSourceFileName = "GeneratedInternalsVisibleToAttributes.g.cs";

    [Test]
    public async Task Execute_WithoutPublicKeyAndWithoutSettings_GeneratesNothing ()
    {
      await new VerifyCS.Test().RunAsync();
    }
    
    [Test]
    public async Task Execute_WithPublicKeyAndWithoutSettings_GeneratesNothing ()
    {
      await new VerifyCS.Test()
      {
          SignAssembly = true
      }.RunAsync();
    }

    [Test]
    public async Task Execute_WithoutPublicKeyAndWithSettings_GeneratesNothing ()
    {
      await new VerifyCS.Test()
      {
          TestState =
          {
              AdditionalFiles =
              {
                  ("setttings.editorconfig", $"is_global=true{Environment.NewLine}build_property.InternalsVisibleToAssemblies = Test1")
              }
          }
      }.RunAsync();
    }

    [Test]
    public async Task Execute_WithSingleAssemblyEntry_GeneratesCorrectFile ()
    {
      await new VerifyCS.Test()
      {
          SignAssembly = true,
          TestState =
          {
              AnalyzerConfigFiles =
              {
                  ("/.editorconfig", $"is_global=true{Environment.NewLine}build_property.InternalsVisibleToAssemblies = Test1")
              },
              GeneratedSources =
              {
                  CreateGeneratedSource($"[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"Test1, PublicKey={WellKnownPublicKey.FormattedString}\")]")
              }
          }
      }.RunAsync();
    }

    [Test]
    public async Task Execute_WithMultipleAssemblyEntries_GeneratesCorrectFile ()
    {
      await new VerifyCS.Test()
      {
          SignAssembly = true,
          TestState =
          {
              AnalyzerConfigFiles =
              {
                  ("/.editorconfig", $"is_global=true{Environment.NewLine}build_property.InternalsVisibleToAssemblies = Test1,A.B.C,Test.Test.Com")
              },
              GeneratedSources =
              {
                  CreateGeneratedSource(
                      $"[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"Test1, PublicKey={WellKnownPublicKey.FormattedString}\")]{Environment.NewLine}" +
                      $"[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"A.B.C, PublicKey={WellKnownPublicKey.FormattedString}\")]{Environment.NewLine}" +
                      $"[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"Test.Test.Com, PublicKey={WellKnownPublicKey.FormattedString}\")]")
              }
          }
      }.RunAsync();
    }

    private static (Type, string, string) CreateGeneratedSource (string source)
    {
      return (typeof(InternalsVisibleToSourceGenerator), c_generatedSourceFileName, source);
    } 
  }
}