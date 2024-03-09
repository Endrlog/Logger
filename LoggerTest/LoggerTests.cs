using Logger;
using Xunit.Abstractions;

namespace LoggerTest
{
    public class LoggerTests
    {
        public Log logger = new Log(5);

        [Fact]
        public void TestWriteGet()
        {
            logger.Write(new LoggedItem("test message", "000", "test"));
            logger.Write(new LoggedItem("test message", "001", "test"));
            logger.Write(new LoggedItem("test message", "002", "test"));
            logger.Write(new LoggedItem("test message", "003", "test"));
            logger.Write(new LoggedItem("test message", "004", "test"));
            Assert.Equal("004", logger.GetLog(0).Code);
            Assert.Equal("000", logger.GetLog(4).Code);
            logger.Write(new LoggedItem("test message", "005", "test"));
            logger.Write(new LoggedItem("test message", "006", "test"));
            logger.Write(new LoggedItem("test message", "007", "test"));
            logger.Write(new LoggedItem("test message", "008", "test"));
            logger.Write(new LoggedItem("test message", "009", "test"));
            Assert.Equal("009", logger.GetLog(0).Code);
            Assert.Equal("005", logger.GetLog(4).Code);
            Assert.Equal("009", logger.GetLogs(4, 0)[0].Code);
            Assert.Equal("006", logger.GetLogs(4, 0)[3].Code);
            Assert.Equal("008", logger.GetLogs(4, 1)[0].Code);
            Assert.Equal("005", logger.GetLogs(4, 1)[3].Code);
        }

        [Fact]
        public void TestChangeSize()
        {
            logger.Write(new LoggedItem("test message", "000", "test"));
            logger.Write(new LoggedItem("test message", "001", "test"));
            logger.Write(new LoggedItem("test message", "002", "test"));
            logger.Write(new LoggedItem("test message", "003", "test"));
            logger.Write(new LoggedItem("test message", "004", "test"));
            Assert.Equal("004", logger.GetLog(0).Code);
            Assert.Equal("000", logger.GetLog(4).Code);
            logger.SetBufferSize(6);
            Assert.Equal("004", logger.GetLog(0).Code);
            Assert.Equal("000", logger.GetLog(4).Code);
            logger.ClearBuffer();
            Assert.Null(logger.GetLog(0));
            Assert.Null(logger.GetLog(4));
        }
    }
}