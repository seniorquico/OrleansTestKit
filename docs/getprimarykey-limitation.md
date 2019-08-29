# `GetPrimaryKey` Limitation

Contents

- [Overview](#overview)
- [`TestSilo` Workaround](#testsilo-workaround)
- [Mock `ActivationData` Workaround](#mock-activationdata-workaround)

## Overview

When run within a test kit environment, code that calls the `GetPrimaryKey` extension methods sometimes result in an `ArgumentException` with the following message:

> Passing a half baked grain as an argument. It is possible that you instantiated a grain class explicitly, as a regular object and not via Orleans runtime or via proper test mocking.

The exception is thrown [by the following block of code](https://github.com/dotnet/orleans/blob/v2.3.0/src/Orleans.Core.Abstractions/Core/GrainExtensions.cs#L88-L96) from `GrainExtensions`:

```csharp
...

var grainBase = grain as Grain;
if (grainBase != null)
{
    if (grainBase.Data == null || grainBase.Data.Identity == null)
    {
        throw new ArgumentException(WRONG_GRAIN_ERROR_MSG, "grain");
    }
    return grainBase.Data.Identity;
}

...
```

The root cause of the issue is that the test kit does not create or populate the `Grain.Data` field. This field is problematic as documented [by the following block of code](https://github.com/dotnet/orleans/blob/v2.3.0/src/Orleans.Core.Abstractions/Core/Grain.cs#L17-L21) from `Grain`:

```csharp
// Do not use this directly because we currently don't provide a way to inject it;
// any interaction with it will result in non unit-testable code. Any behaviour that can be accessed
// from within client code (including subclasses of this class), should be exposed through IGrainRuntime.
// The better solution is to refactor this interface and make it injectable through the constructor.
internal IActivationData Data;
```

## `TestCluster` Workaround

As an alternative to the Orleans TestKit unit test approach, you may consider using the `TestCluster` provided by the official `Microsoft.Orleans.TestingHost` NuGet package to run any test cases that rely on these specific extension methods. The `TestCluster` runs real silos, and grains will have real values in the `Grain.Data` field.

## Test Double Workaround

See [pull request #74](https://github.com/OrleansContrib/OrleansTestKit/pull/74) for a test double implementation.
