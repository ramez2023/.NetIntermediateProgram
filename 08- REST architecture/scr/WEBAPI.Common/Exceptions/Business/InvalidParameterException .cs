using WEBAPI.Common.Enums;

namespace WEBAPI.Common.Exceptions.Business
{
    public class InvalidParameterException : BusinessException
    {
        public InvalidParameterException(string parameterName, string parameterValue) 
            : this(parameterName, parameterValue, null)
        {
        }

        public InvalidParameterException(string parameterName, string parameterValue, string moreDetails)
        {
            Code = (int)ErrorCodes.InvalidParameter;
            Message = string.Format("Invalid value: " + parameterValue + " for parameter: " + parameterName);
            MoreDetails = moreDetails;
        }
    }
}