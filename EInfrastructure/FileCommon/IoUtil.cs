using System;
using System.IO;
using System.Text;
using System.Web;

namespace EInfrastructure.FileCommon
{
    public class IoUtil
    {

        #region 获得文件物理路径
        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                return System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="isAbsolute">是否绝对地址</param>
        public static void Delete(string path, bool isAbsolute = false)
        {
            DeleteExt(!isAbsolute ? HttpContext.Current.Server.MapPath(path) : path);
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        private static void DeleteExt(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

        }
        #endregion


        #region GetBinaryFile：返回所给文件路径的字节数组。
        /// <summary>
        /// GetBinaryFile：返回所给文件路径的字节数组。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GetBinaryFile(string filename)
        {
            if (File.Exists(filename))
            {
                try
                {
                    FileStream s = File.OpenRead(filename);
                    return ConvertStreamToByteBuffer(s);
                }
                catch (Exception)
                {
                    return new byte[0];
                }
            }
            else
            {
                return new byte[0];
            }
        }
        #endregion

        /// <summary>
        /// 把给定的文件流转换为二进制字节数组。
        /// </summary>
        /// <param name="theStream"></param>
        /// <returns></returns>
        public static byte[] ConvertStreamToByteBuffer(Stream theStream)
        {
            byte[] buffer = new byte[0x100];
            var tempStream = new MemoryStream();
            for (int i = theStream.Read(buffer, 0, buffer.Length); i > 0; i = theStream.Read(buffer, 0, buffer.Length))
            {
                tempStream.Write(buffer, 0, i);
            }
            tempStream.Close();
            return tempStream.ToArray();

        }

        #region 输出某个路径文件的字节数组
        /// <summary>
        /// 输出某个路径文件的字节数组
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayByFile(string filepath)
        {
            filepath = HttpContext.Current.Server.MapPath(filepath);
            var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            byte[] filebyte = new byte[fs.Length];
            fs.Read(filebyte, 0, (int)(fs.Length));
            return filebyte;
        }
        #endregion

        #region 将Stream流写入文件
        /// <summary>
        /// 将Stream流写入文件
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="fileName">文件名称</param>
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[] 
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件 
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(bytes);
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
        }
        #endregion

        #region 将文件转为Stream流
        /// <summary>
        /// 将文件转为Stream流
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Stream FileToStream(string fileName)
        {
            // 打开文件 
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileStream.Length];
            try
            {
                fileStream.Read(bytes, 0, bytes.Length);
            }
            finally
            {
                fileStream.Close();
            }
            // 把 byte[] 转换成 Stream 
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        #endregion

        #region 读取文件（默认编码）
        /// <summary>
        /// 读取文件（默认编码）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            return Read(path, Encoding.Default);
        }
        #endregion

        #region 读取文件（指定编码）
        /// <summary>
        /// 读取文件（指定编码）
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string Read(string path, Encoding encode)
        {
            FileStream fs = null;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    fs = new FileStream(FileCommon.GetMapPath(path), FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                }
                catch
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (fs == null) return "";
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fs, encode);
                return sr.ReadToEnd();
            }
            finally
            {
                if (sr != null) sr.Close();
                fs.Close();
            }
        }
        #endregion

    }
}
