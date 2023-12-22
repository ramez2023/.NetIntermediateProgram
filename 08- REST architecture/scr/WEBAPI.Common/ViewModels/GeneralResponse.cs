using System;
using System.Collections.Generic;

namespace WEBAPI.Common.ViewModels
{
    public class GeneralResponse<T>
    {
        public GeneralResponse()
        {
            Data = Activator.CreateInstance<T>();
        }
        public bool IsSucceeded { get; set; }
        public List<Error> Errors { get; set; }
        public T Data { get; set; }
    }
}
