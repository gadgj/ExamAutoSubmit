using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Threading.Tasks;

using System.Data;

namespace ConsoleApplicationExamAutoSubmit
{
    class HttpHelper
    {
        public static string Post(string url, string paramData = "{}", string contentType = "application/x-www-form-urlencoded")
        {
            return Post(url, paramData, contentType, Encoding.UTF8);
        }

        public static string Post(string url, string paramData, string contentType, Encoding encoding)
        {
            string result;

            if (url.ToLower().IndexOf("https", System.StringComparison.Ordinal) > -1)
            {
                ServicePointManager.ServerCertificateValidationCallback =
                               new RemoteCertificateValidationCallback((sender, certificate, chain, errors) => { return true; });
            }

            try
            {

                if (contentType == "multipart/form-data")
                {
                    var wc = new WebClient();


                    ///string postString = "arg1=a&arg2=b";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
                    ////byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
                    byte[] postData = encoding.GetBytes(paramData);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
                    ///string url = "http://localhost/register.php";//地址  
                    ///WebClient webClient = new WebClient();
                    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
                    byte[] responseData = wc.UploadData(url, "POST", postData);//得到返回字符流  
                    ////string srcString = Encoding.UTF8.GetString(responseData);//解码
                    result = encoding.GetString(responseData);//解码
                }
                else
                {
                    var wc = new WebClient();
                    if (string.IsNullOrEmpty(wc.Headers["Content-Type"]))
                        wc.Headers.Add("Content-Type", contentType);
                    wc.Encoding = encoding;
                    result = wc.UploadString(url, "POST", paramData);
                }

            }
            catch (Exception e)
            {
                throw;
            }

            return result;
        }

        public static string Get(string url)
        {
            return Get(url, Encoding.UTF8);
        }

        public static string Get(string url, Encoding encoding)
        {
            try
            {
                var wc = new WebClient { Encoding = encoding };
                var readStream = wc.OpenRead(url);
                using (var sr = new StreamReader(readStream, encoding))
                {
                    var result = sr.ReadToEnd();
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
