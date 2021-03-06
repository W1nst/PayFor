﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayFor.Context;
using PayFor.Models;
using PayFor.ViewModels;
using PayFor.ExtensionMethods;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PayFor.Controllers.Api
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IPayForRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GroupController> _logger;

        public UserController(IPayForRepository repository, UserManager<User> userManager, ILogger<GroupController> logger)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet("")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                //var user = await _userManager.GetUserAsync(this.User);
                var user = await _repository.GetUser(_userManager.GetUserId(this.User));
                return Ok(Mapper.Map<UserViewModel>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all user Categorys: {ex}");
            }
            return BadRequest(new ErrorResponseViewModel {Message="Error while getting all user Categorys!"});
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            { 
                var Category = await _repository.GetCategory(id, _userManager.GetUserId(this.User));
                if (Category == null)
                    return StatusCode(403, new ErrorResponseViewModel {Message="Do not have premission for this action!"});
                return Ok(Mapper.Map<CategoryViewModel>(Category));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Category: {ex}");
            }
            return BadRequest(new ErrorResponseViewModel {Message="Error while getting Category!"});
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateCategory([FromBody]CategoryViewModel Category)
        {
            try { 
                if (!ModelState.IsValid || Category == null)
                    return BadRequest(new ErrorResponseViewModel {Message = ModelState.ErrorsToString()});
                
                var newCategory = Mapper.Map<Category>(Category);
                if (_repository.CreateCategory(newCategory, _userManager.GetUserId(this.User)))
                    return StatusCode(403, new ErrorResponseViewModel {Message="Do not have premission for this action!"});
                if (await _repository.SaveChangesAsync())
                    return await GetCategory(newCategory.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create Category: {ex}");
            }
            return BadRequest(new ErrorResponseViewModel {Message="Error while creating Category!"});
        }

        // POST api/values
        [HttpPost("{id}/delete")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try { 
                if (!await _repository.DeleteCategory(id, _userManager.GetUserId(this.User)))
                    return StatusCode(403, new ErrorResponseViewModel {Message="Do not have premission for this action!"});
                if (await _repository.SaveChangesAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete Category: {ex}");
            }
            return BadRequest(new ErrorResponseViewModel {Message="Error while deleting Category!"});
        }
    }
}
