using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController(IUserReposotory userReposotory, IMapper mapper) : APIBaseController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() {
            var users = await userReposotory.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username) {
            var user = await userReposotory.GetMemberAsync(username);

            if (user == null) return NotFound();

            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto) {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (username == null) return BadRequest("No username found in token");

            var user = await userReposotory.GetUserByUsernameAsync(username);
            if (user == null) return BadRequest("Could not find user");

            mapper.Map(memberUpdateDto, user);  // EF mutates user object in anticipation of saveChanges DB call
            if (await userReposotory.SaveAllAsync()) {
                return NoContent();
            } else {
                return BadRequest("Failed to update user");
            }
        }
    }
}
