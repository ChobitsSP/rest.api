using Rest.Api.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Rest.Api
{
    public class TopHandler : IHttpHandler, IRequiresSessionState
    {
        public static readonly string AssemblyName = ConfigurationManager.AppSettings["AssemblyName"];
        public static readonly string ApiNamespace = ConfigurationManager.AppSettings["ApiNamespace"];

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            string method = request.QueryString["method"];
            //string format = context.Request.QueryString["format"] ?? "json";
            var parameters = request.HttpMethod.ToUpper() == "GET" ? request.QueryString : request.Form;

            var type = Assembly.Load(AssemblyName).GetType(ApiNamespace + "." + method, false, true);

            string body;

            try
            {
                body = parameters.GetJsonResponse(type);
            }
            catch (TopException ex)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                body = new ErrorResponse(ex.ErrorCode, ex.ErrorMsg).ToJson();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debugger.Break();
#endif
                body = new ErrorResponse(1, ex.Message).ToJson();
            }

            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.Write(body);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
