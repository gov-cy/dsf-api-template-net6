using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;

namespace dsf_api_template_net6.Models
{
    public class ContactInfoContext : DbContext
    {
        public ContactInfoContext()
        {
        }

        public ContactInfoContext(DbContextOptions<ContactInfoContext> options)
            : base(options)
        {

        }

        //"Null Forgiving Operator (null!)"
        //Assigning null! effectively says
        //"I know this should never be null, but guess what, I'm doing it anyway". 
        public DbSet<ContactInfo> ContactInfo { get; set; } = null!;

    }
}