namespace hospitalBackend.Models.DB.viewes
{
    public class visit_view
    {
        public int Id { get; set; }
        public int PatientsID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DoctorID { get; set; }
        public int Skill { get; set; }
        public string departmentName { get; set; }
        public int DepartmentID { get; set; }
        public DateTime visitTime { get; set; }
        public int visitTimeID { get; set; }
        public string Comment { get; set; }
        public string AdminUserName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPhoneNumber { get; set; }
    }
}
