using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rest.Api
{
    public interface IParamsRequest
    {
        TopResponse GetResponse(NameValueCollection parameters);
    }

    /// <summary>
    /// 验证参数 throw TopException
    /// </summary>
    public interface IParamsValidate
    {
        /// <summary>
        /// 验证参数 throw TopException
        /// </summary>
        void Validate(NameValueCollection parameters);
    }

    /// <summary>
    /// 可被缓存的请求
    /// </summary>
    public interface ICacheRequest : IParamsRequest
    {
        /// <summary>
        /// 获取缓存key
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string GetKey(NameValueCollection parameters);

        /// <summary>
        /// 超时时间(毫秒)
        /// </summary>
        int Timeout { get; }
    }

    public interface ITopCache
    {
        TopResponse Read(string key);
        void Write(string key, TopResponse rsp);
    }
}
