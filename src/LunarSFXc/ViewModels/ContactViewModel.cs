using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LunarSFXc.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }        
        [Phone]
        public string Phone { get; set; }
        [Required]
        [StringLength(4096, MinimumLength = 10)]
        public string Message { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Email: {Email}");
            sb.AppendLine($"Phone: {Phone ?? string.Empty}");
            sb.AppendLine($"Message: {Message}");


            return sb.ToString();
        }
    }
}