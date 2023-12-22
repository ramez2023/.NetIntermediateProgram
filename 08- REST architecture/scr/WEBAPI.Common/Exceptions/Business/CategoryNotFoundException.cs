using WEBAPI.Common.Enums;

namespace WEBAPI.Common.Exceptions.Business
{
    public class CategoryNotFoundException : BusinessException
    {
        public CategoryNotFoundException(int id)
        {
            Code = (int)WebApiErrorCodes.CategoryNotFound;
            Message = $"No appointment found with id {id}";
        }
    }
}