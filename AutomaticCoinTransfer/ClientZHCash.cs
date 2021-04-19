using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AutomaticCoinTransfer
{
    public class RpcResponseMessage
    {
        public object result { get; set; }
    }
    public class RpcResponseMessageListAddressGroupPings
    {
        public List<List<List<string>>> result { get; set; }
    }

    public class Wallet
    {
        public string Address { get; set; }
        public double Amount { get; set; }
        public string Label { get; set; }
    }

    class ClientZHCash
    {
        public string Url { get; set; } = Settings.Url;


        private HttpClient _httpclient;
        private string User = Settings.User;
        private string Password = Settings.Password;


        public ClientZHCash(HttpClient httpclient)
        {
            _httpclient = httpclient;
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{User}:{Password}"));
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
        }
        public RpcResponseMessage SendToAddressAsync(string SenderAddress, string Address, double Coins)
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
                // из отправленной суммы вычитается комисия за транзакцию
                // получатель получит меньше указанной суммы
                var str = $"[\"{Address}\", {Coins.ToString().Replace(",", ".")}," +
                    $" \"transfer\", \"transfer\", true," +
                    $" null, null, \"\", \"{SenderAddress}\"," +
                    $" true" +
                    "]}";

                requestMessage.Content = new StringContent("{\"method\": \"sendtoaddress\", \"params\":" +
                    $" {str}",
                    Encoding.UTF8, "application/json");

                HttpResponseMessage response =  _httpclient.SendAsync(requestMessage).Result;

                string apiResponse =  response.Content.ReadAsStringAsync().Result;

                try
                {
                    if (apiResponse != "")
                    {
                        RpcResponseMessage result = Newtonsoft.Json.JsonConvert.DeserializeObject<RpcResponseMessage>(apiResponse);

                        return result;
                    }
                    else
                    { throw new Exception(); }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e, $"Something went wrong when calling WeatherStack.com");
                return null;
            }
        }

        public List<Wallet> GetZhCashListAddressGroupPings()
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url);
                requestMessage.Content = new StringContent("{\"method\":\"listaddressgroupings\"}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpclient.SendAsync(requestMessage).Result;

                string apiResponse = response.Content.ReadAsStringAsync().Result;

                try
                {
                    if (apiResponse != "")
                    {
                        RpcResponseMessageListAddressGroupPings result = Newtonsoft.Json.JsonConvert.DeserializeObject<RpcResponseMessageListAddressGroupPings>(apiResponse);
                        return ZhCashListAddressGroupPingsConvertToList(result.result);
                    }
                    else
                    { throw new Exception(); }
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                //_logger.LogError(e, $"Something went wrong when calling WeatherStack.com");
                return null;
            }
        }

        public List<Wallet> ZhCashListAddressGroupPingsConvertToList(List<List<List<string>>> result)
        {
            List<Wallet> wallets = new List<Wallet>();
            foreach (var listFirst in result)
            {
                foreach (var listSecond in listFirst)
                {
                    try
                    {
                        wallets.Add(new Wallet()
                        {
                            Address = listSecond[0],
                            Amount = double.Parse(listSecond[1]),
                            Label = listSecond.Count == 2 ? null : listSecond[2]
                        });
                    }
                    catch
                    {
                        wallets.Add(new Wallet()
                        {
                            Address = listSecond[0],
                            Amount = double.Parse(listSecond[1].Replace(".", ",")), // на локалке
                            Label = listSecond.Count == 2 ? null : listSecond[2]
                        });
                    }
                }
            }

            return wallets;
        }
    }
}