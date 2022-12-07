using Microsoft.EntityFrameworkCore;

namespace dsf_api_template_net6.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext()
        {
        }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {

        }

        //"Null Forgiving Operator (null!)"
        //Assigning null! effectively says
        //"I know this should never be null, but guess what, I'm doing it anyway". 
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        
    }
}