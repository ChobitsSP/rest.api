using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApp
{
    /// <summary>
    /// TopHandler 的摘要说明
    /// </summary>
    public class TopHandler : Rest.Api.TopHandler
    {
        protected override Type GetParamsRequest(string method)
        {
            string ns = this.GetType().Namespace;
            Type type = Assembly.Load(ns).GetType(ns + ".Api." + method, false, true);
            return type;
        }
    }
}