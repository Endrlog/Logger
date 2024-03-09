using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log logger = new Log(5);
            SubscribingClass s = new SubscribingClass(logger);
            logger.Write(new LoggedItem("test", "000", "test"));
        }
    }

    public class LoggedItem
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private DateTime dateTime;
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        public LoggedItem(string m, string c, string t)
        {
            Message = m;
            Code = c;
            Type = t;
            DateTime = DateTime.Now;
        }
    }

    public delegate void LoggerHandler();

    public class Log
    {
        public event LoggerHandler LogAdded;
        LoggedItem[] buffer;
        LoggedItem[] tempBuffer;
        private int bufferIndexMax;
        private int bufferIndex;
        public int BufferIndex
        {
            get 
            {
                return bufferIndex; 
            }
            set 
            {
                bufferIndex = value;
                while (bufferIndex > bufferIndexMax)
                {
                    bufferIndex -= bufferIndexMax + 1;
                } 
            }
        }
        public Log(int bufferLength) 
        {
            bufferIndex = 0;
            buffer = new LoggedItem[bufferLength];
            tempBuffer = new LoggedItem[bufferLength];
            bufferIndexMax = bufferLength - 1;
        }

        public void SetBufferSize(int bufferLength)
        {
            tempBuffer = (LoggedItem[])buffer.Clone();
            buffer = new LoggedItem[bufferLength];
            int repeat;
            if (buffer.Length > tempBuffer.Length)
            {
                repeat = tempBuffer.Length;
            }
            else
            {
                repeat = buffer.Length;
            }
            BufferIndex--;
            while (BufferIndex < 0) 
            {
                BufferIndex += bufferIndexMax + 1;
            }
            for (int i = 0; i < repeat + 1; i++)
            {
                buffer[i] = ((LoggedItem[])tempBuffer.Clone())[BufferIndex];
                BufferIndex++;
            }
            bufferIndexMax = bufferLength - 1;
            BufferIndex = 0;
            tempBuffer = new LoggedItem[bufferLength];
        }

        public void ClearBuffer()
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = null;
            }
            bufferIndex = 0;
        }

        public void Write(LoggedItem log)
        {
            buffer[bufferIndex] = log;
            BufferIndex++;
            LogAdded?.Invoke();
        }

        public LoggedItem GetLog(int placeInOrder) 
        {
            int index = BufferIndex - placeInOrder - 1;
            while (index < 0)
            {
                index += bufferIndexMax + 1;
            }
            return buffer[index];
        }

        public LoggedItem[] GetLogs(int amount, int amountToSkip)
        {
            LoggedItem[] returnedLogs = new LoggedItem[amount];
            int index = BufferIndex - amountToSkip - 1;
            for (int f = 0; f < amount; f++)
            {
                while (index < 0)
                {
                    index += bufferIndexMax + 1;
                }
                returnedLogs[f] = buffer[index];
                index--;
            }
            return returnedLogs;
        }
    }

    public class SubscribingClass
    {
        public SubscribingClass(Log l) 
        {
            l.LogAdded += ConfirmWrite;
        }

        private void ConfirmWrite()
        {
            Console.WriteLine("Log succesfully written");
        }
    }
}
