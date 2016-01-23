using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rest.Api.Util
{
    public static class StringUtils
    {
        public static T JsonParser<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T obj, bool lower = false)
        {
            var settings = new JsonSerializerSettings();

            if (lower)
            {
                settings.ContractResolver = new LowercaseContractResolver();
            }

            var json = JsonConvert.SerializeObject(obj,
#if DEBUG
            Formatting.Indented
#else
            Formatting.None
#endif
, settings);
            return json;
        }

        public static string[] SplitString(this string str, char chr = ',')
        {
            string[] array = new string[0];

            if (!string.IsNullOrEmpty(str))
            {
                array = str.Split(new[] { chr });
            }

            return array;
        }

        public static IEnumerable<int> SplitIntDistinct(this string str, int count, char chr = ',')
        {
            IEnumerable<int> array = new int[0];
            if (!string.IsNullOrEmpty(str))
            {
                array = str.Split(new[] { chr }, count, StringSplitOptions.RemoveEmptyEntries).Select(t => t.ToInt()).Distinct();
            }
            return array;
        }

        public static string GetMD5(string query)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("X");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }
            return result.ToString();
        }

        #region 字符串转int

        public static int? ToNullInt(this object obj)
        {
            int? int_null = null;
            return obj == null ? int_null : obj.ToString().ToNullInt();
        }

        public static int ToInt(this object obj, int def = 0)
        {
            return obj == null ? def : obj.ToString().ToInt(def);
        }

        public static int? ToNullInt(this string s)
        {
            int? int_null = null;
            int i = 0;
            return int.TryParse(s, out i) ? new int?(i) : int_null;
        }

        /// <summary>
        /// 字符串转int
        /// </summary>
        public static int ToInt(this string s, int def = 0)
        {
            var i = s.ToNullInt();
            return i.HasValue ? i.Value : def;
        }

        public static double ToDouble(this string s, double def = 0)
        {
            var i = s.ToNullDouble();
            return i.HasValue ? i.Value : def;
        }

        public static double? ToNullDouble(this string s)
        {
            double? int_null = null;
            double i = 0;
            return double.TryParse(s, out i) ? new double?(i) : int_null;
        }

        public static DateTime? ToDateTime(this string s)
        {
            var dt = JsonConvert.DeserializeObject<DateTime?>(string.Format(@"""{0}""", s));
            return dt;
        }

        public static string ToString(this DateTime? dt, string format, string def = null)
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(format);
            }
            else
            {
                return def;
            }
        }

        #endregion
    }

    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
