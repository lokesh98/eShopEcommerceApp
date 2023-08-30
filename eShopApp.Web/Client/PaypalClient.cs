using eShopApp.Web.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace eShopApp.Web.Client
{
    public class PaypalClient
    {
        private readonly IOptions<PaypalSetting> _paypalSettings;
        public PaypalClient(IOptions<PaypalSetting> paypalSettings)
        {
            _paypalSettings = paypalSettings;
        }

        public async Task<AuthResponse> Authenticate()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(_paypalSettings.Value.ClientId + ":" + _paypalSettings.Value.ClientSecret));

            var content = new List<KeyValuePair<string, string>>
            {
                new ("grant_type","client_credentials")
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(_paypalSettings.Value.TokenUrl),
                Method = HttpMethod.Post,
                Headers =
                {
                    {"Authorization",$"Basic {auth}" }
                },
                Content = new FormUrlEncodedContent(content)
            };

            var httpClient = new HttpClient();
            var httpResponse = await httpClient.SendAsync(request);
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);
            return response!;
        }

        public async Task<CreateOrderResponse> CreateOrder(string value, string currency, string reference)
        {
            var auth = await Authenticate();

            var request = new CreateOrderRequest
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>
                {
                    new()
                    {
                        reference_id=reference,
                        amount=new Amount()
                        {
                            currency_code=currency,
                            value=value
                        }
                    }
                }
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");
            var httpResponse = await httpClient.PostAsJsonAsync(_paypalSettings.Value.ApiUrl, request);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CreateOrderResponse>(jsonResponse);

            return response!;
        }

        public async Task<CaptureOrderResponse> CaptureOrder(string orderNo)
        {
            var auth = await Authenticate();

           

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");

            var contentType = new StringContent("", Encoding.Default, "application/json");

            var httpResponse = await httpClient.PostAsJsonAsync(_paypalSettings.Value.ApiUrl+ "/"+ orderNo + "/capture", contentType);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CaptureOrderResponse>(jsonResponse);

            return response!;
        }
    }
}
