using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Linq;

namespace dsf_api_template_net6.Controllers
{
   
    [Route("api/v{version:apiVersion}/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        //const string scopeRequiredByApi = "dsf.submission";

        public IdentityController(ILogger<IdentityController> logger)
        {
            _logger = logger;
        }

        /// <summary> 
        /// This action simply echoes the identity claims back to the client [Authorize]
        /// </summary>

        [Authorize]
        [HttpGet("identity-echo")]
        //[RequiredScope(scopeRequiredByApi)]        
        public ActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            //_logger.LogInformation("claims: {claims}", claims);
            return new JsonResult(claims);

        }
    }
}