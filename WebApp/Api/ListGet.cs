using Rest.Api;
using Rest.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace WebApp.Api
{
    public class ListGet : IParamsRequest
    {
        public TopResponse GetResponse(NameValueCollection parameters)
        {
            dynamic item = parameters.ValidateJson<dynamic>("item");

            return new ResultResponse(new
            {
                item = item,
                array = new int[] { 1, 2, 3 },
                total_result = 10,
            });
        }
    }
}
