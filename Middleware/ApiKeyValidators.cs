namespace DSF.AspNetCore.Api.Template.Middleware
{
    public class ApiKeyValidators
    {
        private readonly RequestDelegate _next;
        private IConfiguration _configuration;
        private readonly ILogger<ApiKeyValidators> _logger;
        private IApiKeyRepository ApiKeyRepo { get; set; }

        public ApiKeyValidators(RequestDelegate next, IApiKeyRepository _repo, IConfiguration config, ILogger<ApiKeyValidators> logger)
        {
            _next = next;
            ApiKeyRepo = _repo;
            _configuration = config;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            if (!context.Request.Headers.Keys.Contains("client-key"))
            {
                bool isAllowed = false;

                if (context.Request.Path.HasValue)
                {
                    if (context.Request.Path.Value.ToLower().EndsWith("/healthcheck"))
                    {
                        isAllowed = true;
                    }
                }

                if (!isAllowed)
                {
                    context.Response.StatusCode = 400; //Bad Request                
                    await context.Response.WriteAsync("API Key is missing");
                    return;
                }
            }
            else
            {
                if (!ApiKeyRepo.CheckValidApiKey(context.Request.Headers["client-key"]))
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Invalid API Key");
                    return;
                }

                if (context.Request.Path.HasValue)
                {
                    if (!ApiKeyRepo.CheckValidApiKeyAuthorization(
                            context.Request.Headers["client-key"], 
                            context.Request.Path.Value))
                    {
                        context.Response.StatusCode = 401; //UnAuthorized
                        await context.Response.WriteAsync("Unauthorized API Key");
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }
    }
}