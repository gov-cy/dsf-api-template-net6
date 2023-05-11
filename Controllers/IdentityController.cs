using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSF.AspNetCore.Api.Template.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(ILogger<IdentityController> logger)
        {
            _logger = logger;
        }

        /// <summary> 
        /// This action simply echoes the identity claims back to the client [Authorize]
        /// </summary>
        [Authorize]
        [HttpGet("identity-echo")]  
        public ActionResult Get()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            //_logger.LogInformation("claims: {claims}", claims);
            return new JsonResult(claims);

        }
    }
}