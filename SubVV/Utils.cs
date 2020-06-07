using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace SubVV
{ 
    class Utils
    {

        public static JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //public static List<string> VMESS_SECURITY_LIST = new List<string> { @"auto", @"aes-128-gcm", @"chacha20-poly1305", @"none" };

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// HttpWebRequest
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrl(string url)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                req.Timeout = 5000;
                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
                stream.Close();
                resp.Close();
                req.Abort();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {

            }
            return result;
        }

        /// <summary>
        /// Base64 encode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Base64 decode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Decode(string plainText)
        {
            try
            {
                plainText = plainText.Trim()
                  .Replace("\n", "")
                  .Replace("\r\n", "")
                  .Replace("\r", "")
                  .Replace(" ", "")
                  .Replace('-','+')
                  .Replace('_','/');

                if (plainText.Length % 4 > 0)
                {
                    plainText = plainText.PadRight(plainText.Length + 4 - plainText.Length % 4, '=');
                }

                byte[] data = Convert.FromBase64String(plainText);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static Dictionary<string, object> VmessOutboundTemplateNew()
        {
            return javaScriptSerializer.Deserialize<dynamic>(SubVV.Properties.Resources.VmessTemplate);
        }

        /// <summary>
        /// md5 sum
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string MD5Sum(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(txt);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// gen Vmess Config
        /// </summary>
        /// <param name=VmessLink></param>
        /// <returns></returns>
        public static Dictionary<string, object> GenVmessConfig(Vmess VmessLink)
        {
            Dictionary<string, object> VmessProfiles = Utils.VmessOutboundTemplateNew();
            Dictionary<string, object> muxSettings = VmessProfiles["mux"] as Dictionary<string, object>;
            Dictionary<string, object> streamSettings = VmessProfiles["streamSettings"] as Dictionary<string, object>;
            Dictionary<string, object> settings = VmessProfiles["settings"] as Dictionary<string, object>;
            Dictionary<string, object> vnext = (settings["vnext"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> UserInfo = (vnext["users"] as IList<object>)[0] as Dictionary<string, object>;
            Dictionary<string, object> kcpSettings = streamSettings["kcpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> kcpSettingsT = kcpSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> tcpSettings = streamSettings["tcpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> tcpSettingsT = tcpSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> wsSettings = streamSettings["wsSettings"] as Dictionary<string, object>;
            Dictionary<string, object> wsSettingsT = wsSettings["headers"] as Dictionary<string, object>;
            Dictionary<string, object> httpSettings = streamSettings["httpSettings"] as Dictionary<string, object>;
            Dictionary<string, object> quicSettings = streamSettings["quicSettings"] as Dictionary<string, object>;
            Dictionary<string, object> quicSettingsT = quicSettings["header"] as Dictionary<string, object>;
            Dictionary<string, object> tlsSettings = streamSettings["tlsSettings"] as Dictionary<string, object>;

            UserInfo["id"] = VmessLink.id;
            UserInfo["alterId"] = Convert.ToInt32(VmessLink.aid);
            vnext["address"] = VmessLink.add;
            vnext["port"] = Convert.ToInt32(VmessLink.port);
            streamSettings["network"] = VmessLink.net == "h2" ? "http" : VmessLink.net;
            streamSettings["security"] = VmessLink.tls == "" ? "none" : VmessLink.tls;
            tlsSettings["serverName"] = VmessLink.add;
            VmessProfiles["tag"] = VmessLink.ps;
            switch (VmessLink.net)
            {
                case "ws":
                    wsSettingsT["host"] = VmessLink.host;
                    wsSettings["path"] = VmessLink.path;
                    break;
                case "h2":
                    httpSettings["host"] = VmessLink.host.Split(',');
                    httpSettings["path"] = VmessLink.path;
                    break;
                case "tcp":
                    tcpSettingsT["type"] = VmessLink.type;
                    break;
                case "kcp":
                    kcpSettingsT["type"] = VmessLink.type;
                    break;
                case "quic":
                    quicSettingsT["type"] = VmessLink.type;
                    quicSettings["securty"] = VmessLink.host;
                    quicSettings["key"] = VmessLink.path;
                    break;
                default:
                    break;
            }
            return VmessProfiles;
        }
    }
}
