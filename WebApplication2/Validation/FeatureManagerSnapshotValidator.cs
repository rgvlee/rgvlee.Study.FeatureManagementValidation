using FluentValidation;
using Microsoft.FeatureManagement;

namespace WebApplication2.Validation
{
    public interface IFeatureManagerSnapshotValidator : IValidator<IFeatureManagerSnapshot> { }

    public class FeatureManagerSnapshotValidator : AbstractValidator<IFeatureManagerSnapshot>, IFeatureManagerSnapshotValidator
    {
        public FeatureManagerSnapshotValidator()
        {
            RuleFor(x => x)
                .MustAsync(async (x, cancellationToken) => await x.IsEnabledAsync("TrackElements", cancellationToken))
                .WhenAsync(async (x, cancellationToken) => await x.IsEnabledAsync("ArtIntegration", cancellationToken))
                .WithMessage("Usage of the ArtIntegration feature requires the TrackElements feature to be enabled.");
        }
    }
}