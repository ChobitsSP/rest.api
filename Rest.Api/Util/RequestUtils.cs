using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Rest.Api.Util
{
    public static class RequestUtils
    {
        public static TopResponse GetTopResponse(this HttpRequest request, IParamsRequest req, ITopCache cache = null)
        {
            if (req is IParamsRequest)
            {
                var parameters = request.HttpMethod.ToUpper() == "GET" ? request.QueryString : request.Form;
                return (req as IParamsRequest).GetTopResponse(parameters, null);
            }
            throw new TopException(1, "method is null");
        }

        public static TopResponse GetTopResponse(this HttpRequest request, Type type, ITopCache cache = null)
        {
            if (type == null)
            {
                throw new TopException(1, "method is null");
            }

            var req = Activator.CreateInstance(type);

            if (req is IParamsRequest)
            {
                var parameters = request.HttpMethod.ToUpper() == "GET" ? request.QueryString : request.Form;
                return (req as IParamsRequest).GetTopResponse(parameters, null);
            }

            return new JsonResponse();
        }

        public static TopResponse GetTopResponse(this IParamsRequest req, NameValueCollection parameters, ITopCache cache = null)
        {
            TopResponse rsp = null;

            if (req is IParamsValidate)
            {
                (req as IParamsValidate).Validate(parameters);
            }
            if (cache != null && req is ICacheRequest)
            {
                string key = (req as ICacheRequest).GetKey(parameters);
                rsp = cache.Read(key);
            }
            if (rsp == null)
            {
                rsp = req.GetResponse(parameters);
            }

            return rsp;
        }

        public static TopResponse GetTopResponse(this NameValueCollection parameters, Type type, ITopCache cache = null)
        {
            if (type == null)
            {
                throw new TopException(1, "method is null");
            }

            var req = Activator.CreateInstance(type);
            TopResponse rsp = null;

            if (req is IParamsValidate)
            {
                (req as IParamsValidate).Validate(parameters);
            }
            if (cache != null && req is ICacheRequest)
            {
                string key = (req as ICacheRequest).GetKey(parameters);
                rsp = cache.Read(key);
            }
            if (rsp == null && req is IParamsRequest)
            {
                rsp = (req as IParamsRequest).GetResponse(parameters);
            }

            return rsp;
        }

        public static string GetJsonResponse(this NameValueCollection parameters, Type type, ITopCache cache = null)
        {
            var rsp = parameters.GetTopResponse(type, cache);

            if (rsp is JsonResponse)
            {
                return rsp.ToJson();
            }

            throw new TopException(2, "request error");
        }

        #region utils

        public static IDictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var k in col.AllKeys)
            {
                if (!string.IsNullOrEmpty(k) && !dict.ContainsKey(k))
                {
                    dict.Add(k, col[k]);
                }
            }
            return dict;
        }

        public static string BuildErrorJson(string type_name, string err_msg, NameValueCollection col)
        {
            var item = new
            {
                api_name = type_name,
                err_msg = err_msg,
                query = col.ToDictionary().ToJson(),
            };
            return item.ToJson();
        }

        public static string BuildMd5Key(this NameValueCollection parameters)
        {
            string build = parameters.ToDictionary().BuildQuery();
            return StringUtils.GetMD5(build);
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(this IDictionary<string, string> parameters)
        {
            var array = parameters
                .OrderBy(t => t.Key)
                .Select(t => t.Key + "=" + HttpUtility.UrlEncode(t.Value, Encoding.UTF8));
            return string.Join("&", array);
        }

        static string GetInputSteam(Stream stream)
        {
            string body;
            stream.Position = 0;
            using (StreamReader inputStream = new StreamReader(stream))
            {
                body = inputStream.ReadToEnd();
            }
            return body;
        }

        #endregion
    }
}
