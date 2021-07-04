# rgvlee.Study.FeatureManagementValidation

## Overview

rgvlee.Study.FeatureManagementValidation is a working example of validating feature management configuration.

We have a case at Arc Infrastructure where a feature (`ArtIntegration`) depends on another feature (`TrackElements`) being turned on. There are a number of ways this could be handled - from simply ensuring that these dependencies are correctly represented in appsettings through to programmatically enforcing them. YMMV however we've opted to go with the latter as our standard approach.

The solution consists of the following:

- An `IFeatureManagerSnapshot` FluentValidation validator. This allows you to form rules such as if Feature A is turned on, Feature B must also be turned on. More complicated rules can be defined as required.

- Middleware to invoke the validator/validate the `IFeatureManagerSnapshot`. If validation fails a `FeatureManagerSnapshotValidationException` populated with the validation result error message/error messages is thrown.

- Startup configuration.

- Middleware tests.

## Design decisions

> When a feature is evaluated for whether it is on or off, its list of feature-filters are traversed until one of the filters decides the feature should be enabled. At this point the feature is considered enabled and traversal through the feature filters stops. If no feature filter indicates that the feature should be enabled, then it will be considered disabled.<sup>[1]</sup>
- Therefore validation is performed against a feature management entity.

> The values returned from the standard IFeatureManager may change if the IConfiguration source which it is pulling from is updated during the request. This can be prevented by using IFeatureManagerSnapshot. IFeatureManagerSnapshot can be retrieved in the same manner as IFeatureManager. IFeatureManagerSnapshot implements the interface of IFeatureManager, but it caches the first evaluated state of a feature during a request and will return the same state of a feature during its lifetime.<sup>[2]</sup>
- Therefore validation is performed against `IFeatureManagerSnapshot`.

## Additional implementation information

- For the validation to be relevant this solution requires you to inject `IFeatureManagerSnapshot` into your dependencies rather than `IFeatureManager`.

- The middleware can be configured to take any validator provided it implements `IValidator<IFeatureManagerSnapshot>`.

## Testing

- Middleware tests can be found in `TestProject1`.
- There are no tests for the validator itself at this point in time.

## Additional considerations

- The state of a feature can vary; from one request to the next for `IFeatureManagerSnapshot`.

- Depending on your deployment strategy you may want to validate the features outside of the ASP.NET Core request pipeline.

[1]: https://github.com/microsoft/FeatureManagement-Dotnet#feature-filters
[2]: https://github.com/microsoft/FeatureManagement-Dotnet#snapshot
