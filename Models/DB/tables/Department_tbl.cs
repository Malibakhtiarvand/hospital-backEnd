using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospitalBackend.Models.DB.tables
{
    public class Department_tbl
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string departmentName { get; set; }
        public ICollection<Users_tbl> Users_tbl { get; set; }
    }
}
