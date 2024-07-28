using System.ComponentModel.DataAnnotations;

namespace hospitalBackend.Models.Form
{
    public class AddAdminModel
    {
        [Required, DataType(DataType.Text), MinLength(5)]
        public string UserName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfrimPassword { get; set; }
        [Required,DataType(DataType.Text)]
        public int Skill { get; set; }
    }
}
