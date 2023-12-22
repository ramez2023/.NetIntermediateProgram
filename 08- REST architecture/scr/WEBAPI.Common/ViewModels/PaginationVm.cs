using System.Runtime.Serialization;

namespace WEBAPI.Common.ViewModels
{
    [DataContract]
    public class PaginationVm
    {
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        public int PageNumber { get; set; }
    }
}
