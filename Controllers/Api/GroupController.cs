using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayFor.Context;
using PayFor.Models;
using PayFor.ViewModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PayFor.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    public class GroupController : Controller
    {
        private readonly IPayForRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GroupController> _logger;

        public GroupController(IPayForRepository repository, UserManager<User> userManager, ILogger<GroupController> logger )
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }

        // GET All groups
        [HttpGet("")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var groups = await _repository.GetUserGroups(_userManager.GetUserId(this.User));
                return Ok(Mapper.Map<IEnumerable<GroupRowViewModel>>(groups));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all groups: {ex}");
            }
            return BadRequest("Error while getting all Groups!");

        }

        //POST Create group
        [HttpPost("")]
        public async Task<IActionResult> CreateGroup([FromBody]GroupRowViewModel groupRow)
        {
            try
            { 
                if (!ModelState.IsValid || groupRow == null) return BadRequest(ModelState);
                var newGroup = Mapper.Map<Group>(groupRow);
                await _repository.CreateGroup(newGroup, _userManager.GetUserId(this.User));
                if (await _repository.SaveChangesAsync())
                    return Created("api/groupRow",Mapper.Map<GroupRowViewModel>(newGroup));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create group: {ex}");
            }
            return BadRequest("Error while create Group!");
        }

        //POST Delete group
        [HttpPost("{groupId}/delete")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                if (!await _repository.DeleteGroup(groupId, _userManager.GetUserId(this.User)))
                    return StatusCode(403);
                if (await _repository.SaveChangesAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete group: {ex}");
            }
            return BadRequest("Error while delete Group!");
        }


        // GET api/group/5
        [HttpGet("{groupId}")]
        public async Task<IActionResult> Get(int groupId)
        {
            try
            { 
                var group = await _repository.GetGroup(groupId, _userManager.GetUserId(this.User));
                if (group == null)
                    return StatusCode(403);
                return Ok(Mapper.Map<GroupViewModel>(group));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get group: {ex}");
            }
            return BadRequest("Error while getting Group!");
        }
    }
}
