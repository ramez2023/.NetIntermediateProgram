using System.Runtime.Serialization;

namespace WEBAPI.Common.ViewModels
{
    [DataContract]
    public class Response<T> : BaseResponse
    {
        public Response()
        {
        }

        [DataMember]
        public new T Data { get; set; }
    }
}