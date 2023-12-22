using System;

namespace WEBAPI.Common.Exceptions.Business
{
    public class BaseException : Exception
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public string MoreDetails { get; set; }

        public BaseException()
        { }

        public BaseException(string message)
            : base(message)
        { }

        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
