using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hospitalBackend.Models.DB.tables
{
    public class VisitTime_tbl
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Usesr_tbl))]
        public string doctorId { get; set; }
        public bool isActive { get; set; }
        public DateTime visitTime { get; set; }
        public Users_tbl Usesr_tbl { get; set; }
        public ICollection<Visit_tbl> Visit_tbl { get; set; }
    }
}
