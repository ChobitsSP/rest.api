using Rest.Api;
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
            return new ResultResponse(new
            {
                array = new int[] { 1, 2, 3 },
                total_result = 10,
            });
        }
    }
}
