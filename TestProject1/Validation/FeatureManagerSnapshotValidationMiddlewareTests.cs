using System;
using System.Collections.Generic;
using System.Threading;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using Moq;
using NUnit.Framework;
using TestProject1.Extensions;
using WebApplication2.Validation;
using static FluentAssertions.FluentActions;

namespace TestProject1.Validation
{
    public class FeatureManagerSnapshotValidationMiddlewareTests : BaseForTests
    {
        private IFeatureManagerSnapshot _featureManagementSnapshot;
        private FeatureManagerSnapshotValidationMiddleware<IFeatureManagerSnapshotValidator> _middleware;
        private RequestDelegate _nextRequestDelegate;

        private T CreateMockedValidator<T>(Action<CreateMockedValidatorOptions> createOptions = null) where T : class, IValidator<IFeatureManagerSnapshot>
        {
            var resolvedCreateOptions = new CreateMockedValidatorOptions();
            createOptions?.Invoke(resolvedCreateOptions);

            var validatorMock = new Mock<T>();
            validatorMock.Setup(x => x.ValidateAsync(It.IsAny<IFeatureManagerSnapshot>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => resolvedCreateOptions.ValidateAsyncReturns());

            return validatorMock.Object;
        }

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            _nextRequestDelegate = Mock.Of<RequestDelegate>();
            _featureManagementSnapshot = Mock.Of<IFeatureManagerSnapshot>();
            _middleware = new FeatureManagerSnapshotValidationMiddleware<IFeatureManagerSnapshotValidator>(_nextRequestDelegate);
        }

        [Test]
        public void Invoke_ValidationFailure_ThrowsException()
        {
            var propertyName = string.Empty;
            var errorMessage = Fixture.Create<string>();
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure(propertyName, errorMessage) });
            var validator = CreateMockedValidator<IFeatureManagerSnapshotValidator>(o => o.ValidateAsyncReturns = () => validationResult);

            Invoking(async () => await _middleware.Invoke(null, _featureManagementSnapshot, validator))
                .Should()
                .Throw<FeatureManagerSnapshotValidationException>()
                .WithMessage(errorMessage);

            _nextRequestDelegate.Verify(x => x.Invoke(It.IsAny<HttpContext>()), Times.Never);
        }

        [Test]
        public void Invoke_ValidFeatureManagerSnapshot_InvokesNextRequestDelegate()
        {
            var validator = CreateMockedValidator<IFeatureManagerSnapshotValidator>();

            Invoking(async () => await _middleware.Invoke(null, _featureManagementSnapshot, validator))
                .Should()
                .NotThrow();

            _nextRequestDelegate.Verify(x => x.Invoke(It.IsAny<HttpContext>()), Times.Once);
        }

        private class CreateMockedValidatorOptions
        {
            public Func<ValidationResult> ValidateAsyncReturns = () => new ValidationResult();
        }
    }
}