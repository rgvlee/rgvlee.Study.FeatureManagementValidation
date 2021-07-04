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
                .MustAsync(async (x, cancellation) => await x.IsEnabledAsync("TrackElements"))
                .WhenAsync(async (x, cancellation) => await x.IsEnabledAsync("ArtIntegration"))
                .WithMessage("Usage of the ArtIntegration feature requires the TrackElements feature to be enabled.");
        }
    }
}