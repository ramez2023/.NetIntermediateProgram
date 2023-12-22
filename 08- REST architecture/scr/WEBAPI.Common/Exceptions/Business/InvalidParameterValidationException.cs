namespace WEBAPI.Common.Exceptions.Business
{
    public class InvalidParameterValidationException : BusinessException
    {
        public InvalidParameterValidationException(string message)
        {
            Code = 100;
            Message = message;
        }
    }
}
