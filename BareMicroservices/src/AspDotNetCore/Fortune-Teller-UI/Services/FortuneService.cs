
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Fortune_Teller_UI.Services
{
    public class FortuneService : IFortuneService
    {
        ILogger<FortuneService> _logger;
        private const string RANDOM_FORTUNE_URL = "http://fortuneService/api/fortunes/random";
        private const string FORTUNE_URL_PATH = "api/fortunes/random";
        private string _serviceEndpoint;

        public FortuneService(ILoggerFactory logFactory, string serviceEndpoint) 
        {
            _logger =  logFactory.CreateLogger<FortuneService>();

            _logger.LogInformation("The service endpoint is {_serviceEndpoint} ", serviceEndpoint );
            _serviceEndpoint = buildEndpointUrl( _logger, serviceEndpoint );
        }

        public async Task<string> RandomFortuneAsync()
        {
            _logger.LogInformation("The passed in value is " + _serviceEndpoint);
            var client = GetClient();
            var result = await client.GetStringAsync(_serviceEndpoint);
            _logger.LogInformation("RandomFortuneAsync: {0}", result);
            return result;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            return client;
        }

        private static string buildEndpointUrl(ILogger<FortuneService> log, string serviceEndpoint)
        {
            if (serviceEndpoint == null || serviceEndpoint == "")
            {
                log.LogInformation("Error, the service endpoint is null or empty!");
                throw new System.ArgumentException("serviceEndpoint cannot be null or empty");
            }

             // build the service endpoint url
            var fullEndpoint = serviceEndpoint;
            if (!serviceEndpoint.EndsWith("/"))
            {
                fullEndpoint = fullEndpoint + "/";
            }
            fullEndpoint = fullEndpoint + FORTUNE_URL_PATH;

            return fullEndpoint;               
        }
    }
}
