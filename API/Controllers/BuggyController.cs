using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController(DataContext context) : APIBaseController {
        
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth() {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound() {
            var user = context.Users.Find(-1);

            if (user == null) return NotFound();

            return user;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError() {
            throw new Exception("Unplanned server error");
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() {
            return BadRequest("Not a good request");
        }
    }
}