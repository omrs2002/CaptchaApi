using System.Text.Json.Serialization;

namespace CaptchaApi.Helpers
{
    public class CaptchaItem
    {
        [JsonIgnore]
        public string CaptchaCode { get; set; }
        public string EncryptedCaptchaCode { get; set; }
        [JsonIgnore]
        public byte[] CaptchaImageBytes { get; set; }

        public string CaptchBase64Data => Convert.ToBase64String(CaptchaImageBytes);

        public DateTime Timestamp { get; set; }

    }

}
