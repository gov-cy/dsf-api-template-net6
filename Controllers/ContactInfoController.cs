using dsf_api_template_net6.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace dsf_api_template_net6.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly ContactInfoContext _context;
        private IConfiguration _configuration;
        private readonly ILogger<ContactInfoController> _logger;

        public ContactInfoController(IConfiguration config,
                                    ILogger<ContactInfoController> logger,
                                    ContactInfoContext context)
        {
            _configuration = config;
            _logger = logger;
            _context = context;
        }

        // GET: api/ContactInfo/{id}
        /// <summary>
        /// GET Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// GET is used to request data from a specified resource.
        /// <br/>
        /// This endpoint retrieves the contact information by the Identity retrived from the access token.
        /// </remarks>
        [Authorize]
        [HttpGet]
        public async Task<BaseResponse<ContactInfo>> GetContactInfo()
        {
            var response = new BaseResponse<ContactInfo>
            {
                Data = null
            };

            try
            {
                //Get the unique_identifier from the authorized user                  
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null)
                {
                    
                    //get the unique identifier (Civil ID)
                    var unique_identifier = identity.FindFirst("unique_identifier");
                    if (unique_identifier != null)
                    {                        
                        var contactInfo = await _context.ContactInfo.FindAsync((Int32.Parse(unique_identifier.Value)));

                        if (contactInfo != null)
                        {
                            response.Data = contactInfo;
                        }
                        else
                        {
                            //return NotFound();
                            response.ErrorCode = NotFound().StatusCode;
                            response.ErrorMessage = "Contact Info not found";
                        }
                    }
                    else
                    {
                        response.ErrorCode = Unauthorized().StatusCode;
                        response.ErrorMessage = "Unauthorized: unique_identifier not found";
                    }
                }
                else
                {
                    response.ErrorCode = Unauthorized().StatusCode;
                    response.ErrorMessage = "Unauthorized: No identity found";

                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("ContactInfoController - GetContactInfo(long id): " + ex.ToString());
            }


            return response;
        }

        /// <summary>
        /// POST Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// POST is used to send data to a server to create/update a resource.
        /// <br/>
        /// This endpoint creates/updates a single ToDo item by Id.
        /// <br/>
        /// POST: api/TodoItems
        /// <br/>
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </remarks>
        [Authorize]
        [HttpPost]
        public async Task<BaseResponse<ContactInfo>> PostContactInfo(ContactInfo contactInfo)
        {

            var response = new BaseResponse<ContactInfo>
            {
                Data = null
            };

            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    var unique_identifier = identity.FindFirst("unique_identifier");

                    if (unique_identifier != null)
                    {
                        contactInfo.Id = Int32.Parse(unique_identifier.Value);

                        if (!ContactInfoExists(Int32.Parse(unique_identifier.Value)))
                        {                            
                            _context.ContactInfo.Add(contactInfo);
                            await _context.SaveChangesAsync();
                            response.Data = contactInfo;                            
                        }
                        else
                        {
                            //Replace the Item
                            //This could be done with a PUT method but we can use the POST to do the update.
                            _context.Entry(contactInfo).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            response.Data = contactInfo;

                            //If it exists we can return an error instead of replacing
                            //response.ErrorCode = NotFound().StatusCode;
                            //response.ErrorMessage = "Contact Info already exists";
                        }                        
                    }
                    else
                    {
                        response.ErrorCode = Unauthorized().StatusCode;
                        response.ErrorMessage = "Unauthorized: unique_identifier not found";
                    }
                }
                else
                {
                    response.ErrorCode = Unauthorized().StatusCode;
                    response.ErrorMessage = "Unauthorized: No identity found";

                    return response;
                }

            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("ContactInfoController - PostContactInfo(ContactInfo contactInfo): " + ex.ToString());
            }

            return response;


        }

        private bool ContactInfoExists(int id)
        {
            return _context.ContactInfo.Any(e => e.Id == id);
        }
    }
}
