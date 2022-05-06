# Remotion.Infrastructure.SourceGenerators.InternalsVisibleTo

When an assembly is signed the C# compiler requires that the `InternalsVisibleTo` attributes include the public key of the assembly they reference. This is a problem when the key can change because it is generated anew for each development setup.

This project contains a roslyn source generator that generates `InteralsVisibleTo` attributes with a set public key. It is configured using MSBuild properties and uses the public key of the project to generate the correct `InternalsVisibleTo` attributes.

## Configuration

Configuration is done via MSBuild by adding items to the `InternalsVisibleToAssembly` item group:

```xml
<ItemGroup>
  <InternalsVisibleToAssembly Include="MyTest1" Visible="false" />
  <InternalsVisibleToAssembly Include="MyTest2" Visible="false" />
</ItemGroup>
```

The generator will then generated a source file with the corresponding `InternalsVisibleTo` attributes set (The correct public key is inserted instead of `xxx`):

```
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MyTest1, PublicKey=xxx")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MyTest2, PublicKey=xxx")]
```

