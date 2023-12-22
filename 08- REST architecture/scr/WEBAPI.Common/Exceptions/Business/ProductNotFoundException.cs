using WEBAPI.Common.Enums;

namespace WEBAPI.Common.Exceptions.Business
{
    public class ProductNotFoundException : BusinessException
    {
        public ProductNotFoundException(int id)
        {
            Code = (int)WebApiErrorCodes.ProductNotFound;
            Message = $"No invoice found with id {id}";
        }
    }
}