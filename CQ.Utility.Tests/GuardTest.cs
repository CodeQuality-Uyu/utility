namespace CQ.Utility.Tests
{
    [TestClass]
    public class GuardTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullValue_WhenThrowIsNull()
        {
            try
            {
                Guard.ThrowIsNull(null, "prop");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value of parameter cannot be null (Parameter 'prop')", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullValue_WhenThrowIsNullOrEmpty()
        {
            try
            {
                Guard.ThrowIsNullOrEmpty(null, "prop");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value of parameter cannot be null or empty (Parameter 'prop')", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenEmptyValue_WhenThrowIsNullOrEmpty()
        {
            try
            {
                Guard.ThrowIsNullOrEmpty(string.Empty, "prop");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value of parameter cannot be null or empty (Parameter 'prop')", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenValue_WhenThrowMinimumLength()
        {
            try
            {
                Guard.ThrowIsLessThan("1234", 10, "prop");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Parameter 'prop' must have minimum 10 characters", ex.Message);
                throw;
            }
        }
    }
}