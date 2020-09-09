using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpottedUnitn.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpottedUnitn.Infrastructure.Test.TestingUtility
{
    public class ExpectedEntityExceptionAttribute : ExpectedExceptionBaseAttribute
    {
        protected int exceptionCode;

        protected Type exceptionType;

        public ExpectedEntityExceptionAttribute(Type expType, int expCode) : base()
        {
            this.exceptionCode = expCode;
            this.exceptionType = expType;
        }

        protected override void Verify(Exception exception)
        {
            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, this.exceptionType);

            var entityExc = (EntityException)exception;

            Assert.AreEqual(this.exceptionCode, entityExc.Code);
        }
    }
}
