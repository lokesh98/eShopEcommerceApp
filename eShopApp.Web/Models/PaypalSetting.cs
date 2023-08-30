namespace eShopApp.Web.Models
{
    public class PaypalSetting
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenUrl { get; set; }
        public string ApiUrl { get; set; }
        public string Mode { get; set; }
    }
}
