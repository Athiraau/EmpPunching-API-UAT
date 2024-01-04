using Microsoft.AspNetCore.Mvc;
using DataAccess.Contracts;
using DataAccess.Dto;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Business.Contracts;
using System;
using FluentValidation;
using DataAccess.Dto.Request;
using Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace Punching.Controllers
{  
    [AllowAnonymous]
    [Route("api/helper")]
    [EnableCors("MyPolicy")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IServiceWrapper _service;
        private readonly DtoWrapper _dto;
        private readonly ServiceHelper _helper;
        private readonly IValidator<HostCheckReqDto> _hcValidator;
        private readonly IValidator<UserCredReqDto> _ucValidator;

        public HelperController(ILoggerService logger, IServiceWrapper service, DtoWrapper dto, IValidator<HostCheckReqDto> hcValidator,
            IValidator<UserCredReqDto> ucValidator, ServiceHelper helper)
        {
            _logger = logger;
            _service = service;
            _dto = dto;
            _helper = helper;
            _hcValidator = hcValidator;
            _ucValidator = ucValidator;
        }

        [HttpGet("CheckCredentials/{empCode}/{password}", Name = "CheckCredentials")]
        public async Task<IActionResult> CheckEmployeeCredentials([FromRoute] int empCode, string password)
        {
            _dto.userCredkReq.empCode = empCode;
            _dto.userCredkReq.password = password;
            var validationResult = await _ucValidator.ValidateAsync(_dto.userCredkReq);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid user credentials sent from client.");
                return BadRequest(errorRes);
            }

            var employeeCnt = await _service.Helper.CheckEmployeeCredentials(empCode, password);
            
            if (employeeCnt == null)
            {
                _logger.LogError($"Employee with employee code: {empCode}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
               _logger.LogInfo($"Returned employee with empoloyee code: {empCode}");
                return Ok(JsonConvert.SerializeObject(employeeCnt));
            }
        }

        [HttpGet("HostNameCheck/{hostname}", Name = "HostNameCheck")]
        public async Task<IActionResult> HostnameCheck([FromRoute] string hostname)
        {
            _dto.hostCheckReq.hostname = hostname;
            var validationResult = await _hcValidator.ValidateAsync(_dto.hostCheckReq);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid host name sent from client.");
                return BadRequest(errorRes);
            }

            var employeeCnt = await _service.Helper.HostnameCheck(hostname);
            if (employeeCnt == null)
            {
                _logger.LogError($"Details with hostname: {hostname}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned details with hostname: {hostname}");
                return Ok(JsonConvert.SerializeObject(employeeCnt));
            }
        }
    }
}