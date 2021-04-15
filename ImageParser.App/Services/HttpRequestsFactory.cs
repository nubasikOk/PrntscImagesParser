using System;
using System.Threading.Tasks;
using ImageParser.App.Services.Contracts;
using RestSharp;

namespace ImageParser.App.Services
{
    public class HttpRequestsFactory : IHttpRequestsFactory
    {
        public Task<IRestResponse> GetHttpRequestTask()
        {
            var path = Guid.NewGuid().ToString("d").Substring(0, 6);

            var client = new RestClient("https://prnt.sc/" + path) {Timeout = -1};

            var request = new RestRequest(Method.GET);
            request.AddHeader("Cookie", $"__cfduid={Guid.NewGuid()}");

            return client.ExecuteAsync(request);
        }
    }
}