using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// 图片验证码
    /// </summary>
    public class ValidateCodeCommon
    {
        #region 随机字符串
        /// <summary>
        /// 随机生成字符串
        /// </summary>
        /// <param name="number">字符串长度</param>
        /// <returns></returns>
        public static string GenerateCode(int number)
        {
            string checkCode = string.Empty;
            string vChar = "0,1,2,3,4,5,6,7,8,9";
            string[] vcArray = vChar.Split(',');

            int temp = -1;//记录上次随机数值，尽量避免产生几个一样的随机数
            Random rand = new Random();
            for (int i = 1; i < number + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(vcArray.Length);
                if (temp != -1 && temp == t)
                {
                    return GenerateCode(number);
                }
                temp = t;
                checkCode += vcArray[t];
            }
            return checkCode;
        }
        #endregion

        #region 绘制字符串
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="checkCode">随机字符串</param>
        public static byte[] CreateCheckImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == string.Empty)
            {
                //return;
            }

            if (checkCode != null)
            {
                Bitmap image = new Bitmap((int)Math.Ceiling((checkCode.Length * 15.5)), 28);
                Graphics g = Graphics.FromImage(image);
                //try
                //{
                Random radom = new Random();
                g.Clear(Color.White);
                for (int i = 0; i < 2; i++)
                {
                    int x1 = radom.Next(image.Width);
                    int x2 = radom.Next(image.Width);
                    int y1 = radom.Next(image.Height);
                    int y2 = radom.Next(image.Height);
                    g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 50; i++)
                {
                    int x = radom.Next(image.Width);
                    int y = radom.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(radom.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                HttpContext context = HttpContext.Current;
                context.Response.ClearContent();
                context.Response.ContentType = "image/Gif";
                return ms.ToArray();
            }
            return null;
        }
        #endregion

        #region 生成随机字符串
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public static string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = int.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
        #endregion
    }
}

