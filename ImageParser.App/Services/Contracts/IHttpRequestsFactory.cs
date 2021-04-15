using System.Threading.Tasks;
using RestSharp;

namespace ImageParser.App.Services.Contracts
{
    public interface IHttpRequestsFactory
    {
        public Task<IRestResponse> GetHttpRequestTask();
    }
}