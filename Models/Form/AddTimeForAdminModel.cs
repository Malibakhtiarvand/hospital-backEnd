using System.ComponentModel.DataAnnotations;

namespace hospitalBackend.Models.Form
{
    public class AddTimeForAdminModel
    {
        [Required]
        public DateTime time { get; set; }
    }
}
