
namespace dsf_api_template_net6.Middleware
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        //private readonly IConfiguration configuration;
        //ApiContext _context;
        private IConfiguration _configuration;
        private readonly ILogger<ApiKeyRepository> _logger;
        public ApiKeyRepository(IConfiguration config, ILogger<ApiKeyRepository> logger)
        {
            //_context = context;
            _configuration = config;
            _logger = logger;
        }

        public bool CheckValidApiKey(string reqkey)
        {
            var apikeyList = _configuration.GetSection("ApiKeys:client-keys").Get<List<string>>();

            if (apikeyList.Contains(reqkey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckValidApiKeyAuthorization(string apiKey, string endpoint)
        {
            var apikeyAuthList = _configuration.GetSection("ApiKeyAuthorizations:" + apiKey).Get<List<string>>();

            if (apikeyAuthList.Contains("*"))
            {
                return true;
            }

            if (apikeyAuthList != null)
            {
                foreach (var e in apikeyAuthList)
                {
                    if (endpoint.ToLower().StartsWith(e.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckRestrictedApiKey(string reqkey)
        {
            var apiRestrictedKeyList = _configuration.GetSection("ApiKeys:restricted-keys").Get<List<string>>();

            if (apiRestrictedKeyList.Contains(reqkey))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckRestrictedEndpoint(string endpoint)
        {
            var apiRestrictedEndpointList = _configuration.GetSection("RestrictedEndpoints").Get<List<string>>();
            bool ret = false;
            foreach (string s in apiRestrictedEndpointList)
            {
                if (endpoint.Contains(s))
                {
                    ret = true;
                }
            }

            return ret;
        }

    }
}