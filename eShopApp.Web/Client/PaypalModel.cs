using eShopApp.Web.Controllers;
using NuGet.Repositories;

namespace eShopApp.Web.Client
{
    public class PaypalModel
    {
       
    }
    public sealed class AuthResponse
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public string expires_in { get; set; }
        public string nonce { get; set; }
    }
    public sealed class PaypalFee
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
    public sealed class Paypal
    {
        public string email_address { get; set; }
        public string account_id { get; set; }
    }
    public sealed class PaymentSource
    {
        public Paypal paypal { get; set; }
    }
    public sealed class Link
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
    public sealed class Amount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
    public sealed class Capture
    {
        public string id { get; set; }
        public Amount amount { get; set; }
        public string status { get; set; }
        public bool final_capture { get; set; }
        public DateTime created_time { get; set; }
        public DateTime updated_time { get; set; }
        public List<Link> links { get; set; }   
    }
    public sealed class Payments
    {
        public List<Capture> captures { get; set; }
    }
    public sealed class PurchaseUnit
    {
        public Amount amount { get; set; }
        public string reference_id { get; set; }
        public Payments payments { get; set; }
    }
    public sealed class CreateOrderRequest
    {
        public string intent { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }= new List<PurchaseUnit>();
    }
    public sealed class CreateOrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<Link> links { get; set; }
    }
    public sealed class CaptureOrderResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public PaymentSource payment_source { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public List<Link> links { get; set; }
    }
}
