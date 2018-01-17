using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using EInfrastructure.HelpCommon;

namespace EInfrastructure.FileCommon
{
  /// <summary>
  /// 图片帮助类
  /// </summary>
  public class ImageCommon : Page
  {
    /// <summary>
    /// 水印文字
    /// </summary>
    private static string WaterText { get; set; }

    /// <summary>
    /// 水印图片
    /// </summary>
    private static string WaterThum { get; set; }

    #region 产生缩略图
    /// <summary>
    /// 产生缩略图
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="thumbnailPath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="mode"></param>
    private static void MakeThumbnail(Image originalImage, string thumbnailPath, int width, int height, string mode)
    {
      int towidth = width;
      int toheight = height;
      int x = 0;
      int y = 0;
      int ow = originalImage.Width;
      int oh = originalImage.Height;
      switch (mode)
      {
        case "HW"://指定高宽缩放（可能变形）                
          break;
        case "W"://指定宽，高按比例                    
          toheight = originalImage.Height * width / originalImage.Width;
          break;
        case "H"://指定高，宽按比例
          towidth = originalImage.Width * height / originalImage.Height;
          break;
        case "Cut"://指定高宽裁减（不变形）                
          if (originalImage.Width / (double)originalImage.Height > towidth / (double)toheight)
          {
            oh = originalImage.Height;
            ow = originalImage.Height * towidth / toheight;
            y = 0;
            x = (originalImage.Width - ow) / 2;
          }
          else
          {
            ow = originalImage.Width;
            oh = originalImage.Width * height / towidth;
            x = 0;
            y = (originalImage.Height - oh) / 2;
          }
          break;
      }
      //新建一个bmp图片
      Image bitmap = new Bitmap(towidth, toheight);
      //新建一个画板
      var g = Graphics.FromImage(bitmap);
      //设置高质量插值法
      g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
      //设置高质量,低速度呈现平滑程度
      g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
      //清空画布并以透明背景色填充
      g.Clear(Color.Transparent);
      //在指定位置并且按指定大小绘制原图片的指定部分
      g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
          new Rectangle(x, y, ow, oh),
          GraphicsUnit.Pixel);
      try
      {
        //以jpg格式保存缩略图
        bitmap.Save(thumbnailPath, ImageFormat.Jpeg);
      }
      finally
      {
        originalImage.Dispose();
        bitmap.Dispose();
        g.Dispose();
      }
    }
    #endregion

    #region 生成缩略图
    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="originalImagePath">源图路径（物理路径）</param>
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="mode">生成缩略图的方式</param>    
    public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
    {
      var originalImage = Image.FromFile(originalImagePath);

      MakeThumbnail(originalImage, thumbnailPath, width, height, mode);
    }
    #endregion

    #region 生成缩略图
    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="stream">图片流</param>
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="mode">生成缩略图的方式</param>    
    public static void MakeThumbnail(Stream stream, string thumbnailPath, int width, int height, string mode)
    {
      var originalImage = Image.FromStream(stream);

      MakeThumbnail(originalImage, thumbnailPath, width, height, mode);
    }
    #endregion

    #region 生成缩略图
    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="buffer">图片字节数组</param>
    /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
    /// <param name="width">缩略图宽度</param>
    /// <param name="height">缩略图高度</param>
    /// <param name="mode">生成缩略图的方式</param>    
    public static void MakeThumbnail(byte[] buffer, string thumbnailPath, int width, int height, string mode)
    {
      var imageConverter = new ImageConverter();
      var originalImage = imageConverter.ConvertFrom(buffer) as Image;
      MakeThumbnail(originalImage, thumbnailPath, width, height, mode);
    }
    #endregion

    #region 在图片上增加文字水印

    /// <summary>    
    /// 在图片上增加文字水印    
    /// </summary>    
    /// <param name="path">原服务器图片路径</param>    
    /// <param name="pathSy">生成的带文字水印的图片路径(新图片路径)</param>
    /// <param name="waterText">水印文字</param>    
    public static void AddWater(string path, string pathSy, string waterText = "")
    {
      string waterTextTemp = "";
      if (!string.IsNullOrEmpty(waterText))
        waterTextTemp = waterText;
      else if (!string.IsNullOrEmpty(WaterText))
        waterTextTemp = WaterText;
      else
        Assert.NotNull(WaterText,"水印文字");
      var image = Image.FromFile(path);
      var g = Graphics.FromImage(image);
      g.DrawImage(image, 0, 0, image.Width, image.Height);
      var f = new Font("Verdana", 60);
      Brush b = new SolidBrush(Color.Green);
      g.DrawString(waterTextTemp, f, b, 35, 35);

      g.Dispose();
      image.Save(pathSy);
      image.Dispose();
    }
    #endregion

    #region 在图片上生成图片水印
    /// <summary>    
    /// 在图片上生成图片水印    
    /// </summary>    
    /// <param name="path">原服务器图片路径</param>    
    /// <param name="pathSyp">生成的带图片水印的图片路径,新图片路径</param>    
    /// <param name="waterThum">水印图片路径</param>    
    public static void AddWaterPic(string path, string pathSyp, string waterThum = "")
    {
      var image = Image.FromFile(path);
      string waterThumTemp = "";//水印图片路径
      if (string.IsNullOrEmpty(waterThum))
        waterThumTemp = waterThum;
      else if (!string.IsNullOrEmpty(WaterThum))
        waterThumTemp = WaterThum;
      else
        Assert.NotNull(WaterThum, "水印图片地址");
      if (waterThumTemp != null)
      {
        var copyImage = Image.FromFile(waterThumTemp);
        var g = Graphics.FromImage(image);
        g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
        g.Dispose();
      }
      image.Save(pathSyp);
      image.Dispose();
    }
    #endregion

    #region 给图片上水印
    /// <summary>    
    /// 给图片上水印    
    /// </summary>
    /// <param name="filePaths"></param>
    /// <param name="waterFile">水印图片地址</param>    
    public static void MarkWater(string filePaths, string waterFile)
    {
      //GIF不水印    
      int i = filePaths.LastIndexOf(".", StringComparison.Ordinal);
      string ex = filePaths.Substring(i, filePaths.Length - i);
      if (String.Compare(ex, ".gif", StringComparison.OrdinalIgnoreCase) == 0)
      {
        return;
      }
      string modifyImagePath = filePaths;// FilePath + filePaths;//修改的图像路径    
      int lucencyPercent = 25;
      Image modifyImage = null;
      Image drawedImage = null;
      Graphics g = null;
      try
      {
        //建立图形对象    
        modifyImage = Image.FromFile(modifyImagePath, true);
        //  drawedImage = System.Drawing.Image.FromFile(FilePath + waterFile, true);   
        drawedImage = Image.FromFile(waterFile, true);
        g = Graphics.FromImage(modifyImage);
        //获取要绘制图形坐标    
        int x = modifyImage.Width - drawedImage.Width;
        int y = modifyImage.Height - drawedImage.Height;
        //设置颜色矩阵    
        float[][] matrixItems ={
        new float[] {1, 0, 0, 0, 0},
        new float[] {0, 1, 0, 0, 0},
        new float[] {0, 0, 1, 0, 0},
        new[] {0, 0, 0, lucencyPercent/1f, 0},
        new float[] {0, 0, 0, 0, 1}};
        ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
        ImageAttributes imgAttr = new ImageAttributes();
        imgAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        //绘制阴影图像    
        g.DrawImage(drawedImage, new Rectangle(x, y, drawedImage.Width, drawedImage.Height), 0, 0, drawedImage.Width, drawedImage.Height, GraphicsUnit.Pixel, imgAttr);
        //保存文件    
        FileInfo fi = new FileInfo(modifyImagePath);
        ImageFormat imageType = ImageFormat.Gif;
        switch (fi.Extension.ToLower())
        {
          case ".jpg":
            imageType = ImageFormat.Jpeg;
            break;
          case ".gif":
            imageType = ImageFormat.Gif;
            break;
          case ".png":
            imageType = ImageFormat.Png;
            break;
          case ".bmp":
            imageType = ImageFormat.Bmp;
            break;
          case ".tif":
            imageType = ImageFormat.Tiff;
            break;
          case ".wmf":
            imageType = ImageFormat.Wmf;
            break;
          case ".ico":
            imageType = ImageFormat.Icon;
            break;
        }
        MemoryStream ms = new MemoryStream();
        modifyImage.Save(ms, imageType);
        byte[] imgData = ms.ToArray();
        modifyImage.Dispose();
        drawedImage.Dispose();
        g.Dispose();
        File.Delete(modifyImagePath);
        var fs = new FileStream(modifyImagePath, FileMode.Create, FileAccess.Write);
        fs.Write(imgData, 0, imgData.Length);
        fs.Close();
      }
      finally
      {
        try
        {
          if (drawedImage != null) drawedImage.Dispose();
          if (modifyImage != null) modifyImage.Dispose();
          if (g != null) g.Dispose();
        }
        catch
        {
          // ignored
        }
      }
    }
    #endregion

    #region 初始化规则配置
    /// <summary>
    /// 初始化规则配置
    /// </summary>
    /// <param name="waterText">水印 文字</param>
    /// <param name="waterThum">水印 图片</param>
    public static void InitRegularConfig(string waterText = "", string waterThum = "")
    {
      if (!string.IsNullOrEmpty(waterText))
        WaterText = waterText;
      if (!string.IsNullOrEmpty(waterThum))
        WaterThum = waterThum;
    }
    #endregion
  }
}
