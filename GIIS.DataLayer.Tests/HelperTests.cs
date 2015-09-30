//Sample license text.
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GIIS.DataLayer.Tests
{
    [TestClass]
    public class HelperTests
    {
        public HelperTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void ConvertToBoolean_Null()
        {
            Assert.IsTrue(Helper.ConvertToBoolean(null) == default(bool));
        }

        [TestMethod]
        public void ConvertToBoolean_StringFalseLowercase()
        {
            Assert.IsFalse(Helper.ConvertToBoolean("false"));
        }

        [TestMethod]
        public void ConvertToBoolean_StringTrueLowercase()
        {
            Assert.IsTrue(Helper.ConvertToBoolean("true"));
        }

        [TestMethod]
        public void ConvertToBoolean_StringFalseUppercase()
        {
            Assert.IsFalse(Helper.ConvertToBoolean("FALSE"));
        }

        [TestMethod]
        public void ConvertToBoolean_StringTrueUppercase()
        {
            Assert.IsTrue(Helper.ConvertToBoolean("TRUE"));
        }

        [TestMethod]
        public void ConvertToBoolean_IntFalse()
        {
            int value = 0;
            Assert.IsFalse(Helper.ConvertToBoolean(value));
        }

        [TestMethod]
        public void ConvertToBoolean_IntTrue()
        {
            int value = 1;
            Assert.IsTrue(Helper.ConvertToBoolean(value));
        }

        [TestMethod]
        public void ConvertToBoolean_DoubleFalse()
        {
            double value = 0;
            Assert.IsTrue(Helper.ConvertToBoolean(value));
        }

        [TestMethod]
        public void ConvertToBoolean_DoubleTrue()
        {
            double value = 1;
            Assert.IsTrue(Helper.ConvertToBoolean(value));
        }

        [TestMethod]
        public void ConvertToDecimal_DecimalString_HighPrecision()
        {
            decimal decimalValue = 3.141592653589793238462643383m;
            string stringDecimal = decimalValue.ToString();

            Assert.IsTrue((decimal)Helper.ConvertToDecimal(stringDecimal) == decimalValue);
        }


    }
}
