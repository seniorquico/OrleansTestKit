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

## `TestSilo` Workaround

As an alternative to the Orleans TestKit, you may consider testing your specific, failing test case using the `TestSilo` found in `Package.Id`. The `TestSilo` runs a real silo, and will provide a real value in the `Grain.Data` field.

## Test Double Workaround

See [issue #47](https://github.com/OrleansContrib/OrleansTestKit/issues/47) for a discussion and references to upstream issues.
