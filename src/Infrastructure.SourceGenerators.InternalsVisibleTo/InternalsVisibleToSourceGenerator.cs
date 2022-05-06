// SPDX-FileCopyrightText: Copyright (c) rubicon IT GmbH, www.rubicon.eu
// SPDX-License-Identifier: LGPL-2.1-or-later

using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Infrastructure.SourceGenerators.InternalsVisibleTo;

[Generator]
public class InternalsVisibleToSourceGenerator : ISourceGenerator
{
  private const string c_internalsVisibleToAssembliesConfigKey = "build_property.InternalsVisibleToAssemblies";
  private const string c_generatedSourceCodeFileName = "GeneratedInternalsVisibleToAttributes.g.cs";

  private const string c_internalsVisibleToTemplateFormat =
      "[assembly: System.Runtime.CompilerServices.InternalsVisibleTo(\"{0}, PublicKey={1}\")]";

  public void Initialize (GeneratorInitializationContext context)
  {
  }

  public void Execute (GeneratorExecutionContext context)
  {
    var assemblyIdentity = context.Compilation.Assembly.Identity;
    if (!assemblyIdentity.HasPublicKey)
      return;

    var internalsVisibleToAssemblies = GetInternalsVisibleToAssemblies(context);
    if (internalsVisibleToAssemblies.Length == 0)
      return;

    var publicKeyString = HexFormatter.AsHexString(assemblyIdentity.PublicKey);
    var sourceCode = string.Join(
        Environment.NewLine,
        internalsVisibleToAssemblies.Select(e => FormatInternalsVisibleToAttribute(e, publicKeyString)));

    context.AddSource(c_generatedSourceCodeFileName, sourceCode);
  }

  private string[] GetInternalsVisibleToAssemblies (GeneratorExecutionContext context)
  {
    var globalOptions = context.AnalyzerConfigOptions.GlobalOptions;
    if (globalOptions.TryGetValue(c_internalsVisibleToAssembliesConfigKey, out var configValue))
      return configValue.Split(',');

    return Array.Empty<string>();
  }

  private string FormatInternalsVisibleToAttribute (string assemblyName, string publicKey)
  {
    return string.Format(c_internalsVisibleToTemplateFormat, assemblyName, publicKey);
  }
}