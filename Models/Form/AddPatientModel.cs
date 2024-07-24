namespace hospitalBackend.Models.Form
{
    public class AddPatientModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Comment { get; set; }
        public int VisitTimeID { get; set; }
    }
}
