using BonusSystemApplication.BLL.Processes;

namespace BonusSystemApplication.Test.BLL
{
    /// <summary>
    /// The name of your test should consist of three parts:
    /// The name of the method being tested.
    /// The scenario under which it's being tested.
    /// The expected behavior when the scenario is invoked.
    /// </summary>
    public class KeyCheckHandlerTests
    {
        [Theory]
        [InlineData(KeyChecks.KeyCheckNA, "N/A")]
        [InlineData(KeyChecks.KeyCheckKO, "KO")]
        [InlineData(KeyChecks.KeyCheckOK, "OK")]
        [InlineData(KeyChecks.KeyCheckError, "???")]
        [InlineData(KeyChecks.KeyCheckErrorNan, "NaN")]
        [InlineData(KeyChecks.KeyCheckErrorMonotonic, "Error: Non monotonic")]
        public void GetKeyCheck_KeyCheck_ReturnsPredefinedString(KeyChecks keyCheck, string expected)
        {
            // Arrange
            var keyCheckHandler = new KeyChecksHandler();

            // Act
            var actual = keyCheckHandler.GetKeyCheck(keyCheck);

            // Assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GetKeyChecks_ReturnsDictionaryWithItems()
        {
            // Arrange
            var keyCheckHandler = new KeyChecksHandler();

            // Act
            var actual = keyCheckHandler.GetKeyChecks();

            // Assert
            Assert.True(actual.Count > 0);
        }
    }
}