using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApi.API.Data;
using DatingApi.API.DTO;
using DatingApi.API.Helpers;
using DatingApi.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApi.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {            
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _repo.GetUser(userId);
            userParams.UserId = userId;

            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = (user.Gender=="male" ? "female" : "male");
            }
            var users = await _repo.GetUsers(userParams);
            var userList = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(userList);
        }

        [HttpGet("{id}", Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userDetails = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userDetails);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdate){
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            
            var uesrFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdate, uesrFromRepo);

            if(await _repo.SaveAll()){
                return NoContent();
            }

            throw new Exception($"Updating user {id} is failed on saving!");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)){
                return Unauthorized();
            }
            if(await _repo.GetUser(recipientId) == null)
            {
                return NotFound();
            }

            var like = await _repo.GetLike(id, recipientId);
            if(like != null)
            {
                return BadRequest("You already like this user");
            }

            like = new Like{
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if( await _repo.SaveAll()){
                return Ok();
            }

            return BadRequest("User request to like is failed!");
        }
    }
}