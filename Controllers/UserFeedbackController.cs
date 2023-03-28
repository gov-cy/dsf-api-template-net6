using dsf_api_template_net6.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace DsfWebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserFeedbackController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<UserFeedbackController> _logger;        

        public UserFeedbackController(IConfiguration config,
            ILogger<UserFeedbackController> logger)
        {
            _configuration = config;
            _logger = logger;            
        }

        /// <summary>
        /// Add a user feeback record
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// User feedback event record. Adds a record in the user feedback database for analytics
        /// <br/>
        /// If the request is successful, then 200 Http Status code is returned
        /// <br/>
        /// If the request fails, then an Error Code 400 and Description are returned
        /// <br/>
        /// In case of an unhandled exception, Error Code 500 is returned
        /// </remarks>
        /// 
        [HttpPost("feedback-record")]
        public async Task<BaseResponse<string>> AddFeedbackRecordPostAsync([FromBody] UserFeedbackEngineRequest req)
        {
            var response = new BaseResponse<string>
            {
                Data = ""
            };

            try
            {
                string clientKey = HttpContext.Request.Headers["client-key"];
                string serviceId = HttpContext.Request.Headers["service-id"];

                if (!string.IsNullOrEmpty(serviceId))
                {
                    response.Data = HttpStatusCode.OK.ToString();
                }
                else
                {
                    response.ErrorCode = 400;
                    response.ErrorMessage = "UserFeedbackController:AddFeedbackRecordPostAsync - Service Id is required";
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = 500;
                response.ErrorMessage = ex.Message;

                _logger.LogError("UserFeedbackController - AddFeedbackRecordPostAsync: " + ex.ToString());
            }

            return response;
        }

        /// <summary>
        /// Add a user feeback record for logged-in user
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 
        /// User feedback event record. Adds a record in the user feedback database for analytics.
        /// The authorized endpoint records an encrypted id of the authenticated user
        /// <br/>
        /// If the request is successful, then 200 Http Status code is returned
        /// <br/>
        /// If the request fails, then an Error Code 400 and Description are returned
        /// <br/>
        /// In case of an unhandled exception, Error Code 500 is returned
        /// </remarks>
        /// 
        [Authorize]
        [HttpPost("feedback-record-authorized")]
        public async Task<BaseResponse<string>> AddFeedbackRecordPostAuthorizedAsync([FromBody] UserFeedbackEngineRequest req)
        {
            var response = new BaseResponse<string>
            {
                Data = ""
            };

            if (HttpContext.GetTokenAsync("access_token") != null)
            {
                var value = HttpContext.GetTokenAsync("access_token").Result ?? "";
            }

            //Get the unique_identifier from the access token
            string unique_identifier = "";
            string hashedId = "";
            //string citizenName = "";
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                try
                {
                    unique_identifier = identity.FindFirst("unique_identifier").Value;
                    hashedId = dsf_api_template_net6.Helpers.Encryption.SHA256(unique_identifier);
                }
                catch (Exception ex)
                {
                    response.ErrorCode = 401;
                    response.ErrorMessage = "Unauthorized: No identity found";

                    return response;
                }
            }
            else
            {
                response.ErrorCode = 401;
                response.ErrorMessage = "Unauthorized: No identity found";

                return response;
            }

            try
            {
                string clientKey = HttpContext.Request.Headers["client-key"];
                string serviceId = HttpContext.Request.Headers["service-id"];

                if (!string.IsNullOrEmpty(serviceId))
                {
                    response.Data = HttpStatusCode.OK.ToString();
                }
                else
                {
                    response.ErrorCode = 400;
                    response.ErrorMessage = "UserFeedbackController:AddFeedbackRecordPostAuthorizedAsync - Service Id is required";
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = 500;
                response.ErrorMessage = ex.Message;

                _logger.LogError("UserFeedbackController - AddFeedbackRecordPostAuthorizedAsync: " + ex.ToString());
            }

            return response;
        }
    }
}
