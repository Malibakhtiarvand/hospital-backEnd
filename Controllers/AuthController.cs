using hospitalBackend.Models.Authorization;
using hospitalBackend.Models.DB;
using hospitalBackend.Models.DB.tables;
using hospitalBackend.Models.DB.viewes;
using hospitalBackend.Models.Form;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace hospitalBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Users_tbl> _userManager;
        private readonly DBContext _Context;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<AuthController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Users_tbl> userManager, DBContext context, IDataProtectionProvider protectionProvider, ILogger<AuthController> logger, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _userManager = userManager;
            _Context = context;
            _dataProtector = protectionProvider.CreateProtector("O↭r↦~aVHGr1⇟s♞↝♫↛We⟳5♪ag4X❆&1⌅↷J2↞Qev࿓⇞↺u➛$࿓(mHUL↳");
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IResult> AddPatient([FromBody] AddPatientModel model)
        {
            var userForThisTime = _Context.visit_View.Where(x => x.visitTimeID == model.VisitTimeID).SingleOrDefault();
            if (userForThisTime != null)
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

            Patients_tbl user = new Patients_tbl()
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
            };

            _Context.Patients_tbl.Add(user);
            _Context.SaveChanges();
            var lastuser = _Context.Patients_tbl.Where(x => x.Email == user.Email && x.PhoneNumber == user.PhoneNumber).OrderBy(x => x.Id).LastOrDefault();
            var timeVisit = _Context.visitTime_Tbl.Where(x => x.Id == model.VisitTimeID).Single();
            timeVisit.isActive = false;

            Visit_tbl visit_Tbl = new Visit_tbl()
            {
                Comment = model.Comment,
                patientId = lastuser.Id,
                visitTimeID = model.VisitTimeID,
            };
            _Context.visit_Tbls.Add(visit_Tbl);
            _Context.SaveChanges();
            var lastVist = _Context.visit_View.Where(x => x.PatientsID == lastuser.Id && x.visitTimeID == model.VisitTimeID).Single();
            var decodeData = JsonSerializer.Serialize(lastVist);
            var decreaptData = _dataProtector.Protect(decodeData);

            string Issuer = _configuration["jwt:Issuer"];
            string Audience = _configuration["jwt:Audience"];
            string key = _configuration["jwt:key"];
            byte[] key1 = Encoding.UTF8.GetBytes(key);
            var SecurityKey = new SymmetricSecurityKey(key1);
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim("data", decreaptData) };
            var token1 = new JwtSecurityToken(Issuer, Audience, claims, null, DateTime.Now.AddDays(10), Credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(token1);
            return Results.Json(token);
        }

        [HttpGet("CancelVisit")]
        public void CancelVisit()
        {
            string userClaims = HttpContext.User.FindFirst(x => x.Type == "data").Value;
            var decreapt = _dataProtector.Unprotect(userClaims);
            var json = JsonSerializer.Deserialize<visit_view>(decreapt);
            var deleteVisit = _Context.Patients_tbl.Where(x => x.Id == json.PatientsID).Single();
            _Context.Patients_tbl.Remove(deleteVisit);
            var visitTime = _Context.visitTime_Tbl.Where(x => x.Id == json.visitTimeID).Single().isActive = true;
            _Context.SaveChanges();
        }

        [HttpGet("dataOfUser")]
        public IResult DataOfUser()
        {
            string userClaims = HttpContext.User.FindFirst(x => x.Type == "data").Value;
            var decreapt = _dataProtector.Unprotect(userClaims);
            var json = JsonSerializer.Deserialize<visit_view>(decreapt);
            var data = new { json.DoctorID, json.Comment, json.PhoneNumber, json.UserName, json.Email, json.visitTimeID, json.DepartmentID };
            return Results.Json(json);
        }

        [HttpGet("allInfo")]
        public IResult GetAllDepartments()
        {
            var departments = _Context.Department_Tbl.ToList();
            return Results.Json(departments);
        }

        [HttpGet("doctors/{id:int:required}")]
        public List<Users_tbl> getAllDoctors(int id)
        {
            var docors = _userManager.Users.Where(x => x.Skill == id).ToList();
            return docors;
        }

        [HttpGet("times/{id:guid:required}")]
        public List<VisitTime_tbl> getTimesOfDoctor(string id)
        {
            var times = _Context.visitTime_Tbl.Where(x => x.doctorId == id && x.isActive).ToList();
            return times;
        }

        [CheckAdminAuthorization("Manager", "Manager")]
        [HttpPost("admin/Add")]
        public async Task<IResult> AddAdmin([FromBody] AddAdminModel model)
        {
            if (ModelState.IsValid)
            {
                Users_tbl user = new Users_tbl()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    Skill = model.Skill,
                };
                var userAdd = await _userManager.CreateAsync(user, model.Password);
                if (userAdd.Succeeded)
                {
                    var lastUser = await _userManager.FindByEmailAsync(model.Email);
                    string id = lastUser.Id;
                    var email = _dataProtector.Protect(model.Email);

                    Claim[] AdminClaims = new Claim[] {
                        new Claim("Admin","Admin"),
                        new Claim("email",email),
                        new Claim("id",id),
                    };

                    foreach (Claim i in AdminClaims)
                    {
                        var AddClaim = await _userManager.AddClaimAsync(user,i);
                        if (!AddClaim.Succeeded) {
                            return Results.StatusCode(StatusCodes.Status500InternalServerError);
                        }
                    }
                    string Issuer = _configuration["jwt:Issuer"];
                    string Audience = _configuration["jwt:Audience"];
                    string key = _configuration["jwt:key"];
                    byte[] key1 = Encoding.UTF8.GetBytes(key);
                    var keySecurity = new SymmetricSecurityKey(key1);

                    SigningCredentials credentials = new SigningCredentials(keySecurity, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(Issuer, Audience, AdminClaims, null, DateTime.Now.AddDays(10), credentials);
                    var token1 = new JwtSecurityTokenHandler().WriteToken(token);
                    return Results.Text(token1);
                }
                else return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            else return Results.BadRequest();
        }

        [HttpPost("admin/token")]
        public async Task<IResult> AdminLogin([FromBody] AdminLoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user?.PhoneNumber == model.PhoneNumber)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (checkPassword)
                {
                    string Issuer = _configuration["jwt:Issuer"];
                    string Audience = _configuration["jwt:Audience"];
                    string key = _configuration["jwt:key"];
                    byte[] key1 = Encoding.UTF8.GetBytes(key);
                    var keySecurity = new SymmetricSecurityKey(key1);

                    var lastUser = await _userManager.FindByEmailAsync(model.Email);
                    var AdminClaims = await _userManager.GetClaimsAsync(lastUser);

                    SigningCredentials credentials = new SigningCredentials(keySecurity, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(Issuer, Audience, AdminClaims, null, DateTime.Now.AddDays(10), credentials);
                    string token1 = new JwtSecurityTokenHandler().WriteToken(token);
                    return Results.Text(token1);
                }
                else return Results.BadRequest();
            }
            else return Results.BadRequest();
        }

        [HttpGet("Admin/patients")]
        [CheckAdminAuthorization("Admin","Admin")]
        public void GetPatients()
        {
            string emailUser = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var email = _dataProtector.Unprotect(emailUser);
            _logger.Log(LogLevel.Information, email);
        }
    }
}
