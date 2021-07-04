using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;

namespace WebApplication2.Validation
{
    public class FeatureManagerSnapshotValidationMiddleware<T> where T : IValidator<IFeatureManagerSnapshot>
    {
        private readonly RequestDelegate _next;

        public FeatureManagerSnapshotValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IFeatureManagerSnapshot featureManagerSnapshot, T validator)
        {
            var validationResult = await validator.ValidateAsync(featureManagerSnapshot);

            if (!validationResult.IsValid)
            {
                throw new FeatureManagerSnapshotValidationException(string.Join(Environment.NewLine, validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            await _next.Invoke(context);
        }
    }
}