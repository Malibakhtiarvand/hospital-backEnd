using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace hospitalBackend.Models.DB.tables
{
    public class Visit_tbl
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Patients_tbl))]
        public int patientId { get; set; }
        [ForeignKey(nameof(visitTime_Tbl))]
        public int visitTimeID { get; set; }
        public string Comment { get; set; }
        public Patients_tbl Patients_tbl { get; set; }
        public VisitTime_tbl visitTime_Tbl { get; set; }
    }
}
