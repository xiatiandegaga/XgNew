using System;

namespace Cloud.Models
{
    public class MyException : ApplicationException
    {
        public int _isLog { get; }
        public MyException() { }
        public MyException(string msg, int isLog = 1) : base(msg) { _isLog = isLog; }
        public MyException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
