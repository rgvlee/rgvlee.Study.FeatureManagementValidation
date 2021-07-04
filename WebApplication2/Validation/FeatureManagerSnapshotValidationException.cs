using System;

namespace WebApplication2.Validation
{
    public class FeatureManagerSnapshotValidationException : Exception
    {
        public FeatureManagerSnapshotValidationException(string message) : base(message) { }
    }
}