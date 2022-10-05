using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Text.Json.Serialization;

namespace CaptchaApi.Helpers
{
   

    public static class Captcha
    {

        const string Letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string GenerateCaptchaCode()
        {
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }


        public static byte[] ImageToByteArray(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static async Task<CaptchaItem> GenerateCaptchaImageAsync()
        {
            Bitmap bmp = new Bitmap(100, 30);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Green);
            string captchaText = GenerateCaptchaCode();
            g.DrawString(captchaText, new Font("Courier", 16),new SolidBrush(Color.WhiteSmoke), 2, 2);
            g.FillRectangle(new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(255, 0, 0, 0), Color.Transparent), g.ClipBounds);
            g.FillRectangle(new HatchBrush(HatchStyle.ForwardDiagonal, Color.FromArgb(255, 0, 0, 0), Color.Transparent), g.ClipBounds);
            var img = ImageToByteArray(bmp);
            return  await Task.FromResult(
                new CaptchaItem
                {
                    CaptchaImageBytes =img ,
                    CaptchaCode=captchaText
                }
                );
        }

        public static async Task<bool> ValidateCaptchaCode(string userInputCaptcha, string captchaEncrypted)
        {
            var isValid = userInputCaptcha == SecurityHelper.Decryptword(captchaEncrypted);
            return await Task.FromResult(isValid);
        }


    }

}
