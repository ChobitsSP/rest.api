using Rest.Api.Util;
using System;
using System.Web;
using System.Web.SessionState;

namespace Rest.Api
{
    public abstract class TopHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            string method = request.QueryString["method"];
            //string format = context.Request.QueryString["format"] ?? "json";
            var parameters = request.HttpMethod.ToUpper() == "GET" ? request.QueryString : request.Form;

            string body;

            try
            {
                var type = GetParamsRequest(method);
                body = parameters.GetJsonResponse(type);
            }
            catch (TopException ex)
            {
                LogException(ex);
                body = new ErrorResponse(ex.ErrorCode, ex.ErrorMsg).ToJson();
            }
            catch (Exception ex)
            {
                LogException(ex);
                body = new ErrorResponse(1, ex.Message).ToJson();
            }

            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.Write(body);
            context.Response.End();
        }

        protected abstract Type GetParamsRequest(string method);
        protected virtual void LogException(Exception ex) { }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
