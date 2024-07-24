using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace hospitalBackend.Models.DB.tables
{
    public class Users_tbl : IdentityUser
    {
        [ForeignKey(nameof(Department_tbl))]
        public int? Skill { get; set; }
        public ICollection<Visit_tbl> Visit_tbl { get; set; }
        public ICollection<VisitTime_tbl> VisitTime_tbl { get; set; }

        public Department_tbl Department_tbl { get; set; }
    }
}
