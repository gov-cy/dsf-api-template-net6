using DSF.AspNetCore.Api.Template.Helpers;
using DSF.AspNetCore.Api.Template.Models;
using Microsoft.AspNetCore.Mvc;

namespace DSF.AspNetCore.Api.Template.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(IConfiguration config,
                                    ILogger<ValidationController> logger)
        {
            _configuration = config;
            _logger = logger;
        }

        /// <summary>
        /// CY mobile validation
        /// </summary>
        /// <param name="phone_number">The CY mobile number to validate.</param>
        /// <returns></returns>
        /// <remarks>
        /// Validates a CY mobile number (e.g. 99123456)
        /// 
        /// Returns a base response with the validated mobile number, if valid.  
        /// If the mobile CY number is invalid then an Error Code and Description are returned
        /// 
        /// Reference: https://ocecpr.ee.cy/en/taxonomy/term/141/all
        /// 
        /// Sample request:        
        /// /api/v1/Govcycy-mobile-number-validation/99123456
        ///
        /// </remarks>
        //[Authorize]
        [HttpGet("cy-mobile-number-validation/{phone_number}")]
        public BaseResponse<String> ValidateCyMobileNumber(string phone_number)
        {
            var response = new BaseResponse<String>
            {
                Data = ""
            };

            bool valid = false;

            try
            {

                var isNumeric = int.TryParse(phone_number, out int n);

                if (isNumeric && phone_number.Length == 8)
                {
                    //https://ocecpr.ee.cy/en/taxonomy/term/141/all
                    bool ret = int.TryParse(phone_number.Substring(0, 2), out int d);

                    if (ret)
                    {
                        if (d >= 91 && d <= 99)
                        {
                            valid = true;
                        }
                    }
                }

                if (valid)
                {

                    response.Data = phone_number;
                }
                else
                {
                    response.ErrorCode = BadRequest().StatusCode;
                    response.ErrorMessage = "Invalid Phone Number";
                }

            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = "Exception: " + ex.ToString();

                _logger.LogError("ValidationController - ValidateMobilePhoneNumber: " + ex.ToString());
            }

            return response;
        }

        /// <summary>
        /// Email Validation
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns></returns>
        /// <remarks>
        /// Validates an email (e.g. user@mycompany.com)
        /// 
        /// Returns a base response with the validated email, if valid.  
        /// If the email is invalid then an Error Code and Description are returned
        ///        
        /// Sample request:        
        /// /api/v1/Govcy/email-validation/user@mycompany.com
        ///
        /// </remarks>
        [HttpGet("email-validation/{email}")]
        public BaseResponse<String> ValidateEmail(string email)
        {
            var response = new BaseResponse<String>
            {
                Data = ""
            };

            bool valid = false;

            try
            {

                if (email.Length > 80 || email.Length < 7)
                {
                    response.ErrorCode = BadRequest().StatusCode;
                    response.ErrorMessage = "Invalid Email";

                    return response;
                }

                try
                {

                    var res = EmailValidator.IsValidEmailAddress(email, true, true);

                    if (res)
                    {
                        valid = true;
                    }
                }
                catch (Exception ex)
                {
                    // Exception means is no valid email
                    _logger.LogError("ValidationController - ValidateEmail: " + ex.ToString());
                }

                if (valid)
                {

                    response.Data = email;
                }
                else
                {
                    response.ErrorCode = BadRequest().StatusCode;
                    response.ErrorMessage = "Invalid Email";
                }

            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = "Exception: " + ex.ToString();

                _logger.LogError("ValidationController - ValidateEmail: " + ex.ToString());
            }

            return response;
        }
    }
}
