using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rest.Api
{
    public class JsonResponse : TopResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonIgnore]
        public bool IsError
        {
            get
            {
                return this.Code != 0;
            }
        }
    }

    public class ErrorResponse : JsonResponse
    {
        public ErrorResponse(int code, string msg)
        {
            this.Code = code;
            this.Msg = msg.ToUpper();
        }

        [JsonProperty("msg")]
        public string Msg { get; protected set; }
    }

    public class ResultResponse<T> : JsonResponse
    {
        public ResultResponse(T obj)
        {
            this.Result = obj;
        }

        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
