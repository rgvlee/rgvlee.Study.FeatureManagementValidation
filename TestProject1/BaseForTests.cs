using AutoFixture;
using NUnit.Framework;

namespace TestProject1
{
    public abstract class BaseForTests
    {
        protected Fixture Fixture;

        [SetUp]
        public virtual void Setup()
        {
            Fixture = new Fixture();
        }
    }
}