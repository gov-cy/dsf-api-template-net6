
namespace dsf_api_template_net6.Middleware
{
    public interface IApiKeyRepository
    {
        bool CheckValidApiKey(string reqkey);
        bool CheckValidApiKeyAuthorization(string apiKey, string endpoint);
        bool CheckRestrictedApiKey(string reqkey);
        bool CheckRestrictedEndpoint(string endpoint);
    }
}