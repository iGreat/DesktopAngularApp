using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace DesktopApp.Common.Util
{
    public static class CaptchaUtil
    {
        public static byte[] CreateCaptcha(string sCaptchaText)
        {
            const int iHeight = 50;
            const int iWidth = 200;

            var random = new Random();

            var fontEmSizes = new[] {18, 20, 25, 30, 35};

            var fontNames = new[]
            {
                "Microsoft YaHei",
                "SimSun",
                "Comic Sans MS",
                "Arial",
                "Times New Roman",
                "Georgia",
                "Verdana",
                "Geneva"
            };

            var fontStyles = new[]
            {
                FontStyle.Bold,
                FontStyle.Italic,
                FontStyle.Regular,
                FontStyle.Underline
            };

            var hatchStyles = (HatchStyle[]) Enum.GetValues(typeof(HatchStyle));

            using (var outputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb))
            {
                using (var graphics = Graphics.FromImage(outputBitmap))
                {
                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                    var rectangleF = new RectangleF(0, 0, iWidth, iHeight);
                    var brush = new HatchBrush(hatchStyles[random.Next(hatchStyles.Length - 1)],
                        Color.FromArgb(random.Next(100, 255),
                            random.Next(100, 255),
                            random.Next(100, 255)), Color.White);
                    graphics.FillRectangle(brush, rectangleF);

                    var matrix = new Matrix();
                    for (var i = 0; i < sCaptchaText.Length; i++)
                    {
                        matrix.Reset();

                        var gridWidth = iWidth / sCaptchaText.Length;
                        var centerX = gridWidth * (i + 1) - gridWidth / 2; //中心点x
                        const int centerY = iHeight / 2; //中心点y

                        matrix.RotateAt(random.Next(-40, 40), new PointF(centerX, centerY)); //左右随机旋转-40°-40°
                        graphics.Transform = matrix;

                        graphics.DrawString(sCaptchaText.Substring(i, 1),
                            new Font(fontNames[random.Next(fontNames.Length - 1)],
                                fontEmSizes[random.Next(fontEmSizes.Length - 1)],
                                fontStyles[random.Next(fontStyles.Length - 1)]),
                            new SolidBrush(
                                Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100))),
                            new PointF(centerX + random.Next(-gridWidth / 2, 0), 0));

                        graphics.ResetTransform();
                    }

                    using (var ms = new MemoryStream())
                    {
                        outputBitmap.Save(ms, ImageFormat.Png);
                        return ms.GetBuffer();
                    }
                }
            }
        }
    }
}