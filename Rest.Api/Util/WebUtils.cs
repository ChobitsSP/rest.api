using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace Rest.Api.Util
{
    public static class WebUtils
    {
        public static string UrlCombine(params string[] urls)
        {
            return string.Join("/", urls.Where(t => !string.IsNullOrEmpty(t)).Select(t => t.TrimStart('/').TrimStart('\\')));
        }

        /// <summary>
        /// 获取应用程序物理根路径
        /// </summary>
        public static string PhysicalApplicationPath
        {
            get { return System.Web.HttpContext.Current.Request.PhysicalApplicationPath; }
        }

        public static string GetSaveFileUrl(Action<string> saveAs, string floder, string fileName)
        {
            string fullName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            string longPath = Path.Combine(PhysicalApplicationPath, floder.TrimStart('\\', '/'));
            saveAs(Path.Combine(longPath, fullName));
            string fullUrl = "/" + UrlCombine(floder, fullName);
            return fullUrl;
        }

        public static T TryGet<T>(this HttpSessionState session, string key)
        {
            var value = session[key];

            if (value != null && value is T)
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }
    }
}
