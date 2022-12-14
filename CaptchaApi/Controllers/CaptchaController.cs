using CaptchaApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CaptchaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaptchaController : ControllerBase
    {
        [Route("get-captcha-image")]
        [HttpGet]
        public async Task<IActionResult> GetCaptchaImage()
        {
            var resultImg =await Captcha.GenerateCaptchaImageAsync();
            var bytearr = resultImg.CaptchaImageBytes;
            Stream s = new MemoryStream(bytearr);
            return new FileStreamResult(s, "image/png");
        }


        [Route("get-captcha")]
        [HttpGet]
        public async Task<CaptchaItem> GetCaptchaImageItem()
        {
            var capcha = await Captcha.GenerateCaptchaImageAsync();
            capcha.EncryptedCaptchaCode = SecurityHelper.Encryptword(capcha.CaptchaCode);
            capcha.Timestamp = DateTime.UtcNow;
            return capcha;
        }

        [Route("validate-captcha")]
        [HttpGet]
        public async Task<bool> ValidateCaptcha(string userInputCaptcha, string captchaEncrypted)
        {
            try
            {
                if(userInputCaptcha.Trim()=="" || captchaEncrypted.Trim()=="") return false;
                var capcha = await Captcha.ValidateCaptchaCode(userInputCaptcha.Trim(), captchaEncrypted.Trim());
                return capcha;
            }
            catch 
            {
                return false;
            }
        }
    }
}