using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HiBot.Midware
{
    public class ImageHelper
    {
        public static byte[] ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor,  int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                // override color  :)) temp
              
                Font font = new Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
             
                graphics.DrawString(txt, font, new SolidBrush(Color.Brown), 5, 20,StringFormat.GenericTypographic);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();
            }

            using (var stream = new MemoryStream())
            {
                bmp.Save(stream,ImageFormat.Jpeg);
                byte[] byteArr = stream.GetBuffer();
                return byteArr;
            }
        }
    }
}