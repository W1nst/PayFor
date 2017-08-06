using System;
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

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PayFor.Controllers.Api
{
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPayForRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GroupController> _logger;

        public PaymentController(IPayForRepository repository, UserManager<User> userManager, ILogger<GroupController> logger)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: api/values
        [HttpGet("")]
        public async Task<IActionResult> GetUserPayments()
        {
            try
            { 
                var payments = await _repository.GetUserPayments(_userManager.GetUserId(this.User));
                return Ok(Mapper.Map<IEnumerable<PaymentViewModel>>(payments));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all user payments: {ex}");
            }
            return BadRequest("Error while getting all user payments!");
        }

        // GET api/values/5
        [HttpGet("{pid}")]
        public async Task<IActionResult> GetPayment(int pid)
        {
            try
            { 
                var payment = await _repository.GetPayment(pid, _userManager.GetUserId(this.User));
                if (payment == null)
                    return StatusCode(403);
                return Ok(Mapper.Map<PaymentViewModel>(payment));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get payment: {ex}");
            }
            return BadRequest("Error while getting payment!");
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePayment([FromBody]PaymentCreateViewModel payment)
        {
            try { 
                if (!ModelState.IsValid || payment == null)
                    return BadRequest(ModelState);
                var newPayment = Mapper.Map<Payment>(payment);
                if (!await _repository.CreatePayment(newPayment, _userManager.GetUserId(this.User)))
                    return StatusCode(403);
                if (await _repository.SaveChangesAsync())
                    return await GetPayment(newPayment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create payment: {ex}");
            }
            return BadRequest("Error while creating payment!");
        }

        // POST api/values
        [HttpPost("{pId}/delete")]
        public async Task<IActionResult> DeletePayment(int pId)
        {
            try { 
                if (!await _repository.DeletePayment(pId, _userManager.GetUserId(this.User)))
                    return StatusCode(403);
                if (await _repository.SaveChangesAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete payment: {ex}");
            }
            return BadRequest("Error while deleting payment!");
        }

        [HttpPost("{pId}/edit")]
        public async Task<IActionResult> EditPayment([FromBody]PaymentEditViewModel payment)
        {
            try { 
                var p = Mapper.Map<Payment>(payment);
                if (!await _repository.EditPayment(p, _userManager.GetUserId(this.User)))
                    return StatusCode(403);
                if (await _repository.SaveChangesAsync())
                    return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete payment: {ex}");
            }
            return BadRequest("Error while deleting payment!");
        }
    }
}
