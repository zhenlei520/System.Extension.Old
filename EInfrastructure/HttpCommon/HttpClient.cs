/*

HttpClient client=new HttpClient(url);
string html=client.GetString();

GetString()函数内部会查找Http Headers, 以及HTML的Meta标签,试图找出获取的内容的编码信息.
如果都找不到,它会使用client.DefaultEncoding, 这个属性默认为utf-8, 也可以手动设置.

* 自动保持Cookie, Referer
HttpClient client=new HttpClient(url1, null, true);
string html1=client.GetString();
client.url=url2;
string html2=client.GetString();
这里HttpClient的第三个参数,keepContext设置为真时,
HttpClient会自动记录每次交互时服务器对Cookies进行的操作,同时会以前一次请求的Url为Referer.
在这个例子里,获取html2时,会把url1作为Referer, 同时会向服务器传递在获取html1时服务器设置的Cookies. 
当然,你也可以在构造HttpClient时直接提供第一次请求要发出的Cookies与Referer:
HttpClient client=new HttpClient(url, new WebContext(cookies, referer), true);
或者,在使用过程中随时修改这些信息:
client.Context.Cookies=cookies;
client.Context.referer=referer;

* 模拟HTML表单提交
HttpClient client=new HttpClient(url);
client.PostingData.Add(fieldName1, filedValue1);
client.PostingData.Add(fieldName2, fieldValue2);
string html=client.GetString();
上面的代码相当于提交了一个有两个input的表单. 
在PostingData非空,或者附加了要上传的文件时(请看下面的上传和文件), 
HttpClient会自动把HttpVerb改成POST, 并将相应的信息附加到Request上.

* 向服务器上传文件
HttpClient client=new HttpClient(url);
client.AttachFile(fileName, fieldName);
client.AttachFile(byteArray, fileName, fieldName);
string html=client.GetString();
这里面的fieldName相当于<input type="file" Name="fieldName" />里的fieldName. fileName当然就是你想要上传的文件路径了. 
你也可以直接提供一个byte[] 作为文件内容, 但即使如此,你也必须提供一个文件名,以满足HTTP规范的要求.

* 不同的返回形式
字符串: string html = client.GetString();
流: Stream stream = client.GetStream();
字节数组: byte[] data = client.GetBytes();
保存到文件:  client.SaveAsFile(fileName);
或者,你也可以直接操作HttpWebResponse: HttpWebResponse res = client.GetResponse();

每调用一次上述任何一个方法,都会导致发出一个HTTP Request, 
也就是说,你不能同时得到某个Response的两种返回形式.
另外,调用后它们任意一个之后,
你可以通过client.ResponseHeaders来获取服务器返回的HTTP头.

* 下载资源的指定部分(用于断点续传,多线程下载)
HttpClient client=new HttpClient(url);
//发出HEAD请求,获取资源长度
int length=client.HeadContentLength();
//只获取后一半内容
client.StartPoint=length/2;
byte[] data=client.GetBytes();

* HeadContentLength()只会发出HTTP HEAD请求.
根据HTTP协议, HEAD与GET的作用等同, 但是,只返回HTTP头,而不返回资源主体内容. 
也就是说,用这个方法,你 无法获取 一个需要通过POST才能得到的 资源的长度,
如果你确实有这样的需求,
建议你可以通过GetResponse(),然后从ResponseHeader里获取Content-Length.
 */
/* 如果需要显示进度条，则需要使用委托事件ChangeProgressBarStatusEvent，并在委托方法内执行System.Windows.Forms.Application.DoEvents();用来更新界面进度
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using EInfrastructure.FileCommon;
using EInfrastructure.HelpCommon;

namespace EInfrastructure.HttpCommon
{
    /// <summary>
    /// 进度条更新委托
    /// </summary>
    /// <param name="nowNum">当前进度</param>
    /// <param name="minNum">最小进度</param>
    /// <param name="maxNum">最大进度</param>
    public delegate void ChangeProgressBarStatusHandler(int nowNum, int minNum, int maxNum);

    public class HttpClient
    {
        /// <summary>
        /// 定义进度条更新事件
        /// </summary>
        public static event ChangeProgressBarStatusHandler ChangeProgressBarStatusEvent;

        #region deerchao
        #region fields
        private bool _keepContext;
        private string _defaultLanguage = "zh-CN";
        private Encoding _defaultEncoding = Encoding.UTF8;
        private string _accept = "*/*";
        //private string _userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        private string _userAgent = "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Mobile Safari/537.36";
        private HttpVerb _verb = HttpVerb.GET;
        private HttpClientContext _context;
        private readonly List<HttpUploadingFile> _files = new List<HttpUploadingFile>();
        private readonly PostDataList _postingData = new PostDataList();
        private string _url;
        private WebHeaderCollection _responseHeaders;
        private int _startPoint;
        private int _endPoint;
        private bool _keepAlive;
        private string _defalutdomain = "";     //?应该是 defaultdomain
        private int _timeOut;
        private WebProxy _webProxy;

        /// <summary>
        /// 默认域名
        /// </summary>
        private static string DefalutDomain { get; set; }

        #endregion

        #region events
        public event EventHandler<StatusUpdateEventArgs> StatusUpdate;

        private void OnStatusUpdate(StatusUpdateEventArgs e)
        {
            EventHandler<StatusUpdateEventArgs> temp = StatusUpdate;

            if (temp != null)
                temp(this, e);
        }
        #endregion

        #region properties

        public string Defalutdomain
        {
            set { _defalutdomain = value; }
        }
        /// <summary>
        /// 是否自动在不同的请求间保留Cookie, Referer
        /// </summary>
        public bool KeepContext
        {
            get { return _keepContext; }
            set { _keepContext = value; }
        }

        /// <summary>
        /// 期望的回应的语言
        /// </summary>
        public string DefaultLanguage
        {
            get { return _defaultLanguage; }
            set { _defaultLanguage = value; }
        }

        /// <summary>
        /// GetString()如果不能从HTTP头或Meta标签中获取编码信息,则使用此编码来获取字符串
        /// </summary>
        public Encoding DefaultEncoding
        {
            get { return _defaultEncoding; }
            set { _defaultEncoding = value; }
        }

        /// <summary>
        /// 指示发出Get请求还是Post请求
        /// </summary>
        public HttpVerb Verb
        {
            get { return _verb; }
            set { _verb = value; }
        }

        /// <summary>
        /// 要上传的文件.如果不为空则自动转为Post请求
        /// </summary>
        public List<HttpUploadingFile> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// 要发送的Form表单信息
        /// </summary>
        public PostDataList PostingData
        {
            get { return _postingData; }
        }

        /// <summary>
        /// 获取或设置请求资源的地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 用于在获取回应后,暂时记录回应的HTTP头
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return _responseHeaders; }
        }

        /// <summary>
        /// 获取或设置期望的资源类型
        /// </summary>
        public string Accept
        {
            get { return _accept; }
            set { _accept = value; }
        }

        /// <summary>
        /// 获取或设置请求中的Http头User-Agent的值
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set { _userAgent = value; }
        }

        /// <summary>
        /// 获取或设置Cookie及Referer
        /// </summary>
        public HttpClientContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// 获取或设置获取内容的起始点,用于断点续传,多线程下载等
        /// </summary>
        public int StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        /// <summary>
        /// 获取或设置获取内容的结束点,用于断点续传,多下程下载等.
        /// 如果为0,表示获取资源从StartPoint开始的剩余内容
        /// </summary>
        public int EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

        /// <summary>
        /// 超时时间，单位秒
        /// </summary>
        public int TimeOut
        {
            get { return _timeOut; }
            set { _timeOut = value; }
        }

        /// <summary>
        /// 设置代理
        /// </summary>
        public WebProxy WebProxy
        {
            get { return _webProxy; }
            set { _webProxy = value; }
        }

        #endregion

        #region constructors
        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        public HttpClient()
            : this(null)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        public HttpClient(string url)
            : this(url, null)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        /// <param name="url">要获取的资源的地址</param>
        /// <param name="context">Cookie及Referer</param>
        public HttpClient(string url, HttpClientContext context)
            : this(url, context, true)
        {
        }

        /// <summary>
        /// 构造新的HttpClient实例
        /// </summary>
        /// <param name="url">要获取的资源的地址</param>
        /// <param name="context">Cookie及Referer</param>
        /// <param name="keepContext">是否自动在不同的请求间保留Cookie, Referer</param>
        public HttpClient(string url, HttpClientContext context, bool keepContext)
        {
            _url = url;
            _context = context;
            _keepContext = keepContext;
            if (_context == null)
            {
                _context = new HttpClientContext();
                _context.Cookies = new CookieCollection();
            }
        }
        #endregion

        #region AttachFile
        /// <summary>
        /// 在请求中添加要上传的文件
        /// </summary>
        /// <param name="fileName">要上传的文件路径</param>
        /// <param name="fieldName">文件字段的名称(相当于&lt;input type=file Name=fieldName&gt;)里的fieldName)</param>
        public void AttachFile(string fileName, string fieldName)
        {
            HttpUploadingFile file = new HttpUploadingFile(fileName, fieldName);
            _files.Add(file);
        }

        /// <summary>
        /// 在请求中添加要上传的文件
        /// </summary>
        /// <param name="data">要上传的文件内容</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fieldName">文件字段的名称(相当于&lt;input type=file Name=fieldName&gt;)里的fieldName)</param>
        /// <param name="contentType"></param>
        public void AttachFile(byte[] data, string fileName, string fieldName, string contentType)
        {
            HttpUploadingFile file = new HttpUploadingFile(data, fileName, fieldName, contentType);
            _files.Add(file);
        }
        #endregion

        /// <summary>
        /// 清空PostingData, Files, StartPoint, EndPoint, ResponseHeaders, 并把Verb设置为Get.
        /// 在发出一个包含上述信息的请求后,必须调用此方法或手工设置相应属性以使下一次请求不会受到影响.
        /// </summary>
        public void Reset()
        {
            _verb = HttpVerb.GET;
            _files.Clear();
            _postingData.Clear();
            _responseHeaders = null;
            _keepAlive = false;
            _startPoint = 0;
            _endPoint = 0;
            _timeOut = 0;
        }

        private HttpWebRequest CreateRequest()
        {
            if (_url.Contains("https://"))
            {
                ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(_url);

            req.AllowAutoRedirect = false;
            req.CookieContainer = new CookieContainer();
            req.Headers.Add("Accept-Language", _defaultLanguage);
            req.Accept = _accept;
            req.UserAgent = _userAgent;
            req.KeepAlive = _keepAlive;
            req.ServicePoint.Expect100Continue = false;

            if (_timeOut > 0)
            {
                req.Timeout = _timeOut * 1000;
            }

            if (_webProxy != null)
            {
                req.Proxy = _webProxy;
            }

            if (_context.Cookies != null)
                req.CookieContainer.Add(_context.Cookies);
            if (!string.IsNullOrEmpty(_context.Referer))
                req.Referer = _context.Referer;

            if (_verb == HttpVerb.HEAD)
            {
                req.Method = "HEAD";
                return req;
            }

            if (_postingData.Count > 0 || _files.Count > 0)
                _verb = HttpVerb.POST;

            if (_verb == HttpVerb.POST)
            {
                req.Method = "POST";

                MemoryStream memoryStream = new MemoryStream();
                StreamWriter writer = new StreamWriter(memoryStream);

                if (_files.Count > 0)
                {
                    string newLine = "\r\n";
                    string boundary = Guid.NewGuid().ToString().Replace("-", "");
                    req.ContentType = "multipart/form-data; boundary=" + boundary;

                    foreach (var item in _postingData)
                    {
                        writer.Write("--" + boundary + newLine);
                        writer.Write("Content-Disposition: form-data; Name=\"{0}\"{1}{1}", item.Name, newLine);
                        writer.Write(item.Value + newLine);
                    }
                    int i = 0;
                    foreach (HttpUploadingFile file in _files)
                    {
                        i++;
                        writer.Write("--" + boundary + newLine);
                        writer.Write(
                            "Content-Disposition: form-data; Name=\"{0}\"; filename=\"{1}\"{2}",
                            file.FieldName,
                            file.ShortName,
                            newLine
                            );
                        //writer.Write("Content-Type: " + file.ContentType + "" + newLine + newLine);
                        //writer.Write("Content-Type: application/octet-stream" + newLine + newLine);//正确
                        if (file.ContentType != "" && file.ContentType != null)
                        {
                            writer.Write("Content-Type: image/jpeg" + newLine + newLine);
                        }
                        else
                        {
                            writer.Write("Content-Type: application/octet-stream" + newLine + newLine);//正确
                        }
                        writer.Flush();
                        memoryStream.Write(file.Data, 0, file.Data.Length);
                        writer.Write(newLine);

                        if (i == _files.Count)
                        {
                            writer.Write("--" + boundary + "--" + newLine);
                        }
                        else
                        {
                            writer.Write("--" + boundary + newLine);
                        }
                    }
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in _postingData)
                    {
                        sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(item.Name, _defaultEncoding), HttpUtility.UrlEncode(item.Value, _defaultEncoding));
                    }
                    if (sb.Length > 0)
                        sb.Length--;
                    writer.Write(sb.ToString());
                }

                writer.Flush();

                using (Stream stream = req.GetRequestStream())
                {
                    memoryStream.WriteTo(stream);
                }
            }

            if (_startPoint != 0 && _endPoint != 0)
                req.AddRange(_startPoint, _endPoint);
            else if (_startPoint != 0 && _endPoint == 0)
                req.AddRange(_startPoint);

            return req;
        }

        /// <summary>
        /// 发出一次新的请求,并返回获得的回应
        /// 调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>相应的HttpWebResponse</returns>
        public HttpWebResponse GetResponse()
        {
            HttpWebRequest req = CreateRequest();

            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            _responseHeaders = res.Headers;

            string cookiestr = _responseHeaders.Get("Set-Cookie");
            if (_keepContext)
            {
                if (_context.Cookies == null)
                {
                    _context.Cookies = new CookieCollection();
                }
                if (!string.IsNullOrEmpty(cookiestr))
                {
                    SetCookie(cookiestr);
                }
                //_context.Cookies = res.Cookies;
                _context.Referer = _url;
            }
            return res;
        }

        private void SetCookie(string cookiestr)
        {
            string[] cookies = cookiestr.Replace(", ", "!").Split(',');

            foreach (string cookie in cookies)
            {
                if (cookie.StartsWith(" "))
                {
                    continue;
                }
                try
                {
                    string[] cookieitems = cookie.Replace("!", ", ").Split(';');

                    string cookie1 = cookieitems[0];
                    string name = cookie1.Split('=')[0];
                    string value = cookie1.Substring(cookie1.IndexOf('=') + 1).Replace("\"", "");
                    string domain = "";
                    string path = "";

                    string pathitem = (from p in cookieitems where p.ToLower().Contains("path") select p).SingleOrDefault();
                    if (!string.IsNullOrEmpty(pathitem))
                    {
                        path = pathitem.Split('=')[1];
                    }
                    string domainitem = (from d in cookieitems where d.ToLower().Contains("domain") && d.ToLower().StartsWith(" d") select d).SingleOrDefault();
                    if (!string.IsNullOrEmpty(domainitem))
                    {
                        domain = domainitem.Split('=')[1];
                    }
                    if (string.IsNullOrEmpty(domain))
                    {
                        if (string.IsNullOrEmpty(_defalutdomain))
                            _defalutdomain = DefalutDomain;
                        domain = _defalutdomain;
                    }

                    _context.Cookies.Add(new Cookie(name, value, path, domain));
                }
                catch
                {
                    // ignored
                }
            }

        }

        /// <summary>
        /// 发出一次新的请求,并返回回应内容的流

        /// 调用此方法永远不会触发StatusUpdate事件.
        /// </summary>
        /// <returns>包含回应主体内容的流</returns>
        public Stream GetStream()
        {
            return GetResponse().GetResponseStream();
        }

        /// <summary>
        /// 发出一次新的请求,并以字节数组形式返回回应的内容

        /// 调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <returns>包含回应主体内容的字节数组</returns>
        public byte[] GetBytes()
        {
            HttpWebResponse res = GetResponse();
            int length = (int)res.ContentLength;

            MemoryStream memoryStream = new MemoryStream();
            byte[] buffer = new byte[0x100];
            Stream rs = res.GetResponseStream();
            if (rs != null)
            {
                for (int i = rs.Read(buffer, 0, buffer.Length); i > 0; i = rs.Read(buffer, 0, buffer.Length))
                {
                    memoryStream.Write(buffer, 0, i);
                    OnStatusUpdate(new StatusUpdateEventArgs((int)memoryStream.Length, length));
                }
                rs.Close();
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// 发出一次新的请求,以Http头,或Html Meta标签,或DefaultEncoding指示的编码信息对回应主体解码
        /// 调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <returns>解码后的字符串</returns>
        public string GetString()
        {
            try
            {
                var stream = GetStream();

                string encodingName = GetEncodingFromHeaders();

                if (encodingName != null && encodingName.ToLower().Contains("gzip"))
                {
                    stream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                    //string source = "";
                    //using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("GB2312")))
                    //{
                    //    source = reader.ReadToEnd();
                    //}
                    //return source;
                }

                byte[] data = IoUtil.ConvertStreamToByteBuffer(stream);

                if (encodingName == null || encodingName == "gzip")
                {
                    encodingName = GetEncodingFromBody(data);
                }

                Encoding encoding;
                if (string.IsNullOrEmpty(encodingName))
                {
                    encoding = _defaultEncoding;
                }
                else
                {
                    try
                    {
                        encoding = Encoding.GetEncoding(encodingName);
                    }
                    catch (ArgumentException)
                    {
                        encoding = _defaultEncoding;
                    }
                }
                return encoding.GetString(data);

            }
            catch (Exception ex)
            {
                return "异常:" + ex.Message;
            }
        }

        /// <summary>
        /// 发出一次新的请求,对回应的主体内容以指定的编码进行解码
        /// 调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <param name="encoding">指定的编码</param>
        /// <returns>解码后的字符串</returns>
        public string GetString(Encoding encoding)
        {
            byte[] data = GetBytes();
            return encoding.GetString(data);
        }

        private string GetEncodingFromHeaders()
        {
            string encoding = null;
            if (_responseHeaders.AllKeys.Contains("Content-Encoding"))
            {
                return _responseHeaders["Content-Encoding"];
            }
            string contentType = _responseHeaders["Content-Type"];
            if (contentType != null)
            {
                int i = contentType.IndexOf("charset=", StringComparison.Ordinal);
                if (i != -1)
                {
                    encoding = contentType.Substring(i + 8);
                }
            }
            return encoding;
        }

        private string GetEncodingFromBody(byte[] data)
        {
            string encodingName = null;
            string dataAsAscii = Encoding.ASCII.GetString(data);
            {
                int i = dataAsAscii.IndexOf("charset=", StringComparison.Ordinal);
                if (i != -1)
                {
                    int j = dataAsAscii.IndexOf("\"", i, StringComparison.Ordinal);
                    if (j != -1)
                    {
                        int k = i + 8;
                        encodingName = dataAsAscii.Substring(k, (j - k) + 1);
                        char[] chArray = new char[2] { '>', '"' };
                        encodingName = encodingName.TrimEnd(chArray);
                    }
                }
            }
            return encodingName;
        }

        /// <summary>
        /// 发出一次新的Head请求,获取资源的长度

        /// 此请求会忽略PostingData, Files, StartPoint, EndPoint, Verb
        /// </summary>
        /// <returns>返回的资源长度</returns>
        public int HeadContentLength()
        {
            Reset();
            HttpVerb lastVerb = _verb;
            _verb = HttpVerb.HEAD;
            using (HttpWebResponse res = GetResponse())
            {
                _verb = lastVerb;
                return (int)res.ContentLength;
            }
        }

        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件

        /// 调用此方法会触发StatusUpdate事件
        /// 如果指定的文件存在,它会被覆盖

        /// </summary>
        /// <param name="fileName">要保存的文件路径</param>
        public void SaveAsFile(string fileName)
        {
            SaveAsFile(fileName, FileExistsAction.Overwrite);
        }

        /// <summary>
        /// 发出一次新的请求,把回应的主体内容保存到文件

        /// 调用此方法会触发StatusUpdate事件
        /// </summary>
        /// <param name="fileName">要保存的文件路径</param>
        /// <param name="existsAction">指定的文件存在时的选项</param>
        /// <returns>是否向目标文件写入了数据</returns>
        public bool SaveAsFile(string fileName, FileExistsAction existsAction)
        {
            byte[] data = GetBytes();
            switch (existsAction)
            {
                case FileExistsAction.Overwrite:
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)))
                        writer.Write(data);
                    return true;

                case FileExistsAction.Append:
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Append, FileAccess.Write)))
                        writer.Write(data);
                    return true;

                default:
                    if (!File.Exists(fileName))
                    {
                        using (
                            BinaryWriter writer =
                                new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                            writer.Write(data);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }
        #endregion

        #region wangjun

        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">提交的地址</param>
        /// <param name="postDataStr">表单对象</param>
        /// <returns>服务器返回值</returns>
        public static string SendDataByPost(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = new CookieContainer();
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.Default);
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (myResponseStream != null)
            {
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.Default);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();

                return retString;
            }
            else
            {
                return "";
            }
        }


        /// <summary> 下载文件</summary>        
        /// <param name="url">下载文件地址</param>        
        /// <param name="filename">下载后的存放地址</param>
        public static void DownloadFile(string url, string filename)
        {
            try
            {
                var myrq = (HttpWebRequest)WebRequest.Create(url);
                var myrp = (HttpWebResponse)myrq.GetResponse();

                var st = myrp.GetResponseStream();
                Stream so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                if (st != null)
                {
                    int osize = st.Read(@by, 0, @by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;

                        so.Write(@by, 0, osize);

                        osize = st.Read(@by, 0, @by.Length);
                    }
                }
                so.Close();
                if (st != null) st.Close();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary> 下载文件</summary>        
        /// <param name="url">下载文件地址</param>
        /// <param name="filename">文件名称</param>
        /// <param name="isShowProgressBar">是否显示进度条</param>
        public static void DownloadFile(string url, string filename, bool isShowProgressBar = false)
        {
            try
            {
                var myrq = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myrp = (HttpWebResponse)myrq.GetResponse();
                long totalBytes = myrp.ContentLength;
                //if (isShowProgressBar)
                //{
                //    prog.Maximum = (int)totalBytes;
                //}
                var st = myrp.GetResponseStream();
                Stream so = new FileStream(filename, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] by = new byte[1024];
                if (st != null)
                {
                    int osize = st.Read(@by, 0, @by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        so.Write(@by, 0, osize);
                        if (isShowProgressBar)
                            if (ChangeProgressBarStatusEvent != null)
                                ChangeProgressBarStatusEvent((int)totalDownloadedByte, 0, (int)totalBytes);
                        osize = st.Read(@by, 0, @by.Length);
                    }
                }
                so.Close();
                if (st != null) st.Close();
            }
            catch
            {
                // ignored
            }
        }
        /// <summary>
        /// 发送post请求上传文件
        /// </summary>
        /// <param name="address">url地址</param>
        /// <param name="fileNamePath">本机文件路径</param>
        /// <param name="saveName">上传保存的文件名</param>
        /// <returns>1请求成功0失败</returns>
        public static int Upload_Request(string address, string fileNamePath, string saveName)
        {
            int returnValue = 0;
            // 要上传的文件          
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            //时间戳           
            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
            //请求头部信息             
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; Name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
            // 根据uri创建HttpWebRequest对象        
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存           
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（300秒）         
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            httpReq.ContentLength = length;
            try
            {

                //每次上传4k               
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];
                //已上传的字节数 
                //开始上传时间 
                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();
                //发送请求头部消息               
                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);

                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳            
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();
                //获取服务器端的响应 
                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                if (s != null)
                {
                    StreamReader sr = new StreamReader(s);
                    //读取服务器端返回的消息 
                    String sReturnString = sr.ReadLine();
                    s.Close();
                    sr.Close();
                    if (sReturnString == "Success")
                    {
                        returnValue = 1;
                    }
                    else if (sReturnString == "Error")
                    {
                        returnValue = 0;
                    }
                }
            }
            catch
            {
                returnValue = 0;
            }
            finally
            {
                fs.Close();
                r.Close();
            }
            return returnValue;
        }

        // <summary>
        /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
        /// 
        /// <param name="address">文件上传到的服务器</param>
        /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <param name="isShowProgressBar">是否显示进度条</param>
        /// <returns>成功返回1，失败返回0</returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static int Upload_Request(string address, string fileNamePath, string saveName, bool isShowProgressBar = false)
        {
            int returnValue = 0;

            // 要上传的文件
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);

            //时间戳

            string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");

            //请求头部信息
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(strBoundary);
            sb.Append("\r\n");
            sb.Append("Content-Disposition: form-data; Name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("\r\n");
            sb.Append("\r\n");
            string strPostHeader = sb.ToString();
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);

            // 根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";

            //对发送的数据不使用缓存

            httpReq.AllowWriteStreamBuffering = false;

            //设置获得响应的超时时间（300秒）
            httpReq.Timeout = 300000;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
            httpReq.ContentLength = length;
            try
            {
                if (isShowProgressBar)
                    if (ChangeProgressBarStatusEvent != null)
                        ChangeProgressBarStatusEvent(0, 0, int.MaxValue);

                //每次上传4k
                int bufferLength = 4096;
                byte[] buffer = new byte[bufferLength];

                //已上传的字节数

                long offset = 0;

                //开始上传时间

                int size = r.Read(buffer, 0, bufferLength);
                Stream postStream = httpReq.GetRequestStream();

                //发送请求头部消息

                postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                while (size > 0)
                {
                    postStream.Write(buffer, 0, size);
                    offset += size;
                    if (isShowProgressBar)
                        if (ChangeProgressBarStatusEvent != null)
                            ChangeProgressBarStatusEvent((int)(offset * (int.MaxValue / length)), 0, int.MaxValue);
                    size = r.Read(buffer, 0, bufferLength);
                }
                //添加尾部的时间戳
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Close();

                //获取服务器端的响应

                WebResponse webRespon = httpReq.GetResponse();
                Stream s = webRespon.GetResponseStream();
                if (s != null)
                {
                    StreamReader sr = new StreamReader(s);

                    //读取服务器端返回的消息

                    String sReturnString = sr.ReadLine();
                    s.Close();
                    sr.Close();
                    if (sReturnString == "Success")
                    {
                        returnValue = 1;
                    }
                    else if (sReturnString == "Error")
                    {
                        returnValue = 0;
                    }
                }
            }
            catch
            {
                returnValue = 0;
            }
            finally
            {
                fs.Close();
                r.Close();
            }

            return returnValue;
        }
        #endregion

        #region 初始化Http请求类
        /// <summary>
        /// 初始化Http请求类
        /// </summary>
        /// <param name="defalutDomain">默认域名</param>
        public static void InitRegularConfig(string defalutDomain = "")
        {
            if (string.IsNullOrEmpty(defalutDomain))
                Assert.IsNotNull("默认请求域名");
            else
                DefalutDomain = defalutDomain;
        }
        #endregion

    }

    //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
    internal class AcceptAllCertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint sPoint,

           X509Certificate cert, WebRequest wRequest, int certProb)
        {

            // Always accept 

            return true;

        }

    }


    public class HttpClientContext
    {
        private CookieCollection _cookies;
        private string _referer;

        public CookieCollection Cookies
        {
            get { return _cookies; }
            set { _cookies = value; }
        }

        public string Referer
        {
            get { return _referer; }
            set { _referer = value; }
        }
    }

    public class PostData
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class PostDataList : List<PostData>
    {
        public void Add(string name, string value)
        {
            base.Add(new PostData { Name = name, Value = value });
        }
    }

    public enum HttpVerb
    {
        GET,
        POST,
        HEAD,
    }

    public enum FileExistsAction
    {
        Overwrite,
        Append,
        Cancel,
    }

    public class HttpUploadingFile
    {
        private string _fileName;
        private string _fieldName;
        private string _contentType;
        private byte[] _data;
        private string _shortName;

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public HttpUploadingFile(string fileName, string fieldName)
        {
            //如果文件不存在就直接返回
            if (!File.Exists(fileName))
            {
                return;
            }
            _fileName = fileName;
            _fieldName = fieldName;
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                byte[] inBytes = new byte[stream.Length];
                stream.Read(inBytes, 0, inBytes.Length);
                _data = inBytes;
            }
            _shortName = Path.GetFileName(fileName);
        }

        public HttpUploadingFile(byte[] data, string fileName, string fieldName, string contentType)
        {
            _data = data;
            _fileName = fileName;
            _fieldName = fieldName;
            _contentType = contentType;
        }
    }

    public class StatusUpdateEventArgs : EventArgs
    {
        private readonly int _bytesGot;
        private readonly int _bytesTotal;

        public StatusUpdateEventArgs(int got, int total)
        {
            _bytesGot = got;
            _bytesTotal = total;
        }

        /// <summary>
        /// 已经下载的字节数
        /// </summary>
        public int BytesGot
        {
            get { return _bytesGot; }
        }

        /// <summary>
        /// 资源的总字节数
        /// </summary>
        public int BytesTotal
        {
            get { return _bytesTotal; }
        }
    }

}
