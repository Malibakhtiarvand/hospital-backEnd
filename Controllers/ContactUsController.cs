using hospitalBackend.Models.DB;
using hospitalBackend.Models.DB.tables;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace hospitalBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly UserManager<Users_tbl> _userManager;
        private readonly DBContext _Context;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<AuthController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public ContactUsController(UserManager<Users_tbl> userManager, DBContext context, IDataProtectionProvider protectionProvider, ILogger<AuthController> logger, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _userManager = userManager;
            _Context = context;
            _dataProtector = protectionProvider.CreateProtector("O↭r↦~aVHGr1⇟s♞↝♫↛We⟳5♪ag4X❆&1⌅↷J2↞Qev࿓⇞↺u➛$࿓(mHUL↳");
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [HttpPost]
        public object AddContactMsg([FromBody]ContactUsMsg_tbl model)
        {
            if (ModelState.IsValid)
            {
                _Context.contactUsMsg_Tbl.Add(model);
                _Context.SaveChanges();
                return true;
            }
            else return BadRequest();
        }
    }
}
