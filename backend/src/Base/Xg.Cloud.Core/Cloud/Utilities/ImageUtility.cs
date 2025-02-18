using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Cloud.Utilities
{
    public class ImageUtility
    {
        public static MemoryStream CreateImageVerifyCode(out string code, int numbers = 4)
        {
            code = RandomCodeUtility.GetRandomCode(numbers);
            MemoryStream ms = null;
            Random random = new Random();
            //验证码颜色集合  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            using (var img = new Bitmap((int)code.Length * 18, 32))
            {
                using (var g = Graphics.FromImage(img))
                {
                    g.Clear(Color.White);//背景设为白色
                    //在随机位置画背景点  
                    for (int i = 0; i < 100; i++)
                    {
                        int x = random.Next(img.Width);
                        int y = random.Next(img.Height);
                        g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
                    }
                    //验证码绘制在g中  
                    for (int i = 0; i < code.Length; i++)
                    {
                        int cindex = random.Next(7);//随机颜色索引值  
                        int findex = random.Next(5);//随机字体索引值  
                        Font f = new Font(fonts[findex], 15, FontStyle.Bold);//字体  
                        Brush b = new SolidBrush(c[cindex]);//颜色  
                        int ii = 4;
                        if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                        {
                            ii = 2;
                        }
                        g.DrawString(code.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符  
                    }
                    ms = new MemoryStream();//生成内存流对象  
                    img.Save(ms, ImageFormat.Jpeg);//将此图像以Png图像文件的格式保存到流中  
                }
            }
            return ms;
        }
    }
}
