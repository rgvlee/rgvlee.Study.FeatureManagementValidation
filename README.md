# rgvlee.Study.FeatureManagementValidation

## Overview

rgvlee.Study.FeatureManagementValidation is a working example of validating feature management configuration.

We have a case at Arc Infrastructure where a feature (`ArtIntegration`) depends on another feature (`TrackElements`) being enabled. There are a number of ways this could be handled - from simply ensuring that these dependencies are correctly represented in appsettings through to programmatically enforcing them. YMMV however we've opted to go with the latter as our standard approach.

The solution consists of the following:

- An `IFeatureManagerSnapshot` FluentValidation validator. This allows you to form rules such as if Feature A is enabled, Feature B must also be enabled. More complicated rules can be defined as required.

- Middleware to invoke the validator/validate the `IFeatureManagerSnapshot`. If validation fails a `FeatureManagerSnapshotValidationException` populated with the validation result error message/error messages is thrown.

- Startup configuration.

- Middleware tests

## Design decisions

- Validation is performed against a feature management entity because features can be enabled via feature filters [[1]].

- The values returned from the standard `IFeatureManager` may change if the `IConfiguration` source which it is pulling from is updated during the request. `IFeatureManagerSnapshot` implements the interface of IFeatureManager, but it caches the first evaluated state of a feature during a request and will return the same state of a feature during its lifetime [[2]]. Therefore validation is performed against `IFeatureManagerSnapshot`.

## Additional implementation information

- For the validation to be relevant this solution requires you to inject `IFeatureManagerSnapshot` into your dependencies rather than `IFeatureManager`.

- The middleware can be configured to take any validator provided it implements `IValidator<IFeatureManagerSnapshot>`.

## Testing

- Tests for the middleware are included in `TestProject1`. There are no tests for the validator itself at this point in time.

## Additional considerations

- The state of a feature can vary; for `IFeatureManagerSnapshot`, from one request to the next.

- Depending on your deployment strategy you may want to validate the features outside of the ASP.NET Core request pipeline.

[1]: https://github.com/microsoft/FeatureManagement-Dotnet#feature-filters
[2]: https://github.com/microsoft/FeatureManagement-Dotnet#snapshot
