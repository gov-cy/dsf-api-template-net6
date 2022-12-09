using dsf_api_template_net6.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dsf_api_template_net6.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private IConfiguration _configuration;
        private readonly ILogger<ValidationController> _logger;

        public TodoItemsController(IConfiguration config,
                                    ILogger<ValidationController> logger,
                                    TodoContext context)
        {
            _configuration = config;
            _logger = logger;
            _context = context;
        }

        // GET: api/TodoItems 
        /// <summary>
        /// GET Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// GET is used to request data from a specified resource.
        /// <br/>
        /// This endpoint retrieves all ToDo items.
        /// </remarks>
        [Authorize]
        [HttpGet]
        public async Task<BaseResponse<IEnumerable<TodoItem>>> GetTodoItems()
        {
            var response = new BaseResponse<IEnumerable<TodoItem>>
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
                        response.Data = await _context.TodoItems.ToListAsync();
                        response.InformationMessage = unique_identifier.Value.ToString();
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

                _logger.LogError("TodoItemsController - GetTodoItems: " + ex.ToString());
            }
            
            return response;
        }

        // GET: api/TodoItems/{id}
        /// <summary>
        /// GET Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// GET is used to request data from a specified resource.
        /// <br/>
        /// This endpoint retrieves a single ToDo item by Id.
        /// </remarks>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<BaseResponse<TodoItem>> GetTodoItem(long id)
        {
            var response = new BaseResponse<TodoItem>
            {
                Data = null
            };

            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);

                if (todoItem != null)
                {
                    response.Data = todoItem;
                    
                }
                else
                {
                    //return NotFound();
                    response.ErrorCode = NotFound().StatusCode;
                    response.ErrorMessage = "Item not found";
                }
                
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("TodoItemsController - GetTodoItem(long id): " + ex.ToString());
            }
            

            return response;
        }

        /// <summary>
        /// PUT Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// PUT is used to send data to a server to create/update a resource.
        /// <br/>
        /// This endpoint replaces a single ToDo item by Id.        
        /// <br/>
        /// PUT: api/TodoItems/5
        /// <br/>
        /// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// </remarks>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<BaseResponse<TodoItem>> PutTodoItem(long id, TodoItem todoItem)
        {
            var response = new BaseResponse<TodoItem>
            {
                Data = null
            };

            if (id != todoItem.Id)
            {
                //return BadRequest();
                response.ErrorCode = BadRequest().StatusCode;
                response.ErrorMessage = "Bad Request";
            }            

            try
            {
                _context.Entry(todoItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Data = todoItem;
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("TodoItemsController - PutTodoItem(long id, TodoItem todoItem): " + ex.ToString());
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
        public async Task<BaseResponse<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            //_context.TodoItems.Add(todoItem);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
            
            var response = new BaseResponse<TodoItem>
            {
                Data = null
            };                        

            try
            {
                _context.TodoItems.Add(todoItem);
                await _context.SaveChangesAsync();
                response.Data = todoItem;
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("TodoItemsController - PostTodoItem(TodoItem todoItem): " + ex.ToString());
            }

            return response;


        }

        /// <summary>
        /// DELETE Method [Authorize]
        /// </summary>                
        /// <remarks>
        /// The DELETE method deletes the specified resource.
        /// <br/>
        /// This endpoint deletes a single ToDo item by Id.
        /// <br/>
        /// DELETE: api/TodoItems/5        
        /// </remarks>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<BaseResponse<int?>> DeleteTodoItem(long id)
        {           
            var response = new BaseResponse<int?>
            {
                Data = null
            };

            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    response.ErrorCode = NotFound().StatusCode;
                    response.ErrorMessage = "Item not found";
                }
                else
                {
                    _context.TodoItems.Remove(todoItem);
                    await _context.SaveChangesAsync();
                    response.Data = (int?)id;
                }                
            }
            catch (Exception ex)
            {
                response.ErrorCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.ToString();

                _logger.LogError("TodoItemsController - PostTodoItem(TodoItem todoItem): " + ex.ToString());
            }

            return response;
        }

        //private bool TodoItemExists(long id)
        //{
        //    return _context.TodoItems.Any(e => e.Id == id);
        //}
    }
}