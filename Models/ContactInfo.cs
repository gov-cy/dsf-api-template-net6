using System.ComponentModel.DataAnnotations.Schema;

namespace dsf_api_template_net6.Models
{
    public class ContactInfo
    {        
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? MobileTelephone { get; set; }
    }
}
