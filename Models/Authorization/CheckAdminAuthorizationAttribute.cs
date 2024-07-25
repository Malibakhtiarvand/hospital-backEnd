using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace hospitalBackend.Models.Authorization
{
    public class CheckAdminAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Claim _Claim;
        public CheckAdminAuthorizationAttribute(string type,string value) {
            _Claim = new Claim(type,value);
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var user = httpContext.User;
            bool checkAdmin = user.HasClaim(x => x.Type == _Claim.Type && x.Value == _Claim.Value);
            if(!checkAdmin) {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
