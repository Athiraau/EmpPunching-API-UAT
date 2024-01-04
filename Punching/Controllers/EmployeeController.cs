using Microsoft.AspNetCore.Mvc;
using DataAccess.Contracts;
using DataAccess.Dto;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Business.Contracts;
using DataAccess.Dto.Request;
using FluentValidation;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace Punching.Controllers
{
    [AllowAnonymous]
    [Route("api/employee")]
    [EnableCors("MyPolicy")]
    [ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly ILoggerService _logger;
		private readonly IServiceWrapper _service;
        private readonly DtoWrapper _dto;
        private readonly ServiceHelper _helper;
        private readonly IValidator<EmpGetReqDto> _empIdValidator;
        private readonly IValidator<DailyAttendUpdateDto> _upsertValidator;
        private readonly IValidator<ImageUpdateReqDto> _imageVlaidator;
        private readonly IValidator<RetreiveBlobReqDto> _retreiveBlobValidator;

        public EmployeeController(ILoggerService logger, IServiceWrapper service, DtoWrapper dto, ServiceHelper helper,
            IValidator<EmpGetReqDto> empIdValidator, IValidator<DailyAttendUpdateDto> upsertValidator, IValidator<ImageUpdateReqDto> imageVlaidator, 
            IValidator<RetreiveBlobReqDto> retreiveBlobValidator)
		{
			_logger = logger;
			_service = service;
            _dto = dto;
            _helper = helper;
            _empIdValidator = empIdValidator;
            _upsertValidator = upsertValidator;
            _imageVlaidator = imageVlaidator;
            _retreiveBlobValidator = retreiveBlobValidator;
            
        }

        [HttpGet("EmployeeByCode/{empCode}", Name = "EmployeeByCode")]
        public async Task<IActionResult> GetEmployee([FromRoute] int empCode)
        {
            _dto.empGetReq.empCode = empCode;
            var validationResult = await _empIdValidator.ValidateAsync(_dto.empGetReq);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid employee code sent from client.");
                return BadRequest(errorRes);
            }

            var employee = await _service.Employee.GetEmployeeByCode(empCode);
            if (employee == null)
            {
                _logger.LogError($"Employee with employee code: {empCode}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned employee with empoloyee code: {empCode}");                
                return Ok(JsonConvert.SerializeObject(employee));
            }
        }

        [HttpGet("EmployeeFirm/{empCode}", Name = "EmployeeFirm")]
        public async Task<IActionResult> GetEmployeeFirm([FromRoute] int empCode)
        {
            _dto.empGetReq.empCode = empCode;
            var validationResult = await _empIdValidator.ValidateAsync(_dto.empGetReq);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid employee code sent from client.");
                return BadRequest(errorRes);
            }

            var firm = await _service.Employee.GetEmployeeFirm(empCode);
            if (firm == null)
            {
               _logger.LogError($"employee firm details with employee code: {empCode}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                _logger.LogInfo($"Returned employee firm details with empoloyee code: {empCode}");
                return Ok(JsonConvert.SerializeObject(firm));
            }
        }

        [HttpPost("Upsert")]
		public async Task<IActionResult> UpdateDailyAttend([FromBody] DailyAttendUpdateDto dailyAttend)
		{
            var validationResult = await _upsertValidator.ValidateAsync(dailyAttend);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);            

            if (dailyAttend is null)
            {
               _logger.LogError("employee object sent from client is null.");
                return BadRequest("employee object is null");
            }
            else if (errorRes.errorMessage.Count > 0)
            {
               _logger.LogError("Invalid employee details sent from client.");
                return BadRequest(errorRes);
            }

            var dailyAttendRes = await _service.Employee.UpdateDailyAttend(dailyAttend);
			_logger.LogInfo($"employee with employee code: {dailyAttend.empCode}, has been updated in db.");
			return Ok(JsonConvert.SerializeObject(dailyAttendRes));
		}

        [HttpPost("UpdateImage")]
        public async Task<IActionResult> UpdateImage([FromBody] ImageUpdateReqDto imageUpdate)
        {
            var validationResult = await _imageVlaidator.ValidateAsync(imageUpdate);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (imageUpdate is null)
            {
                _logger.LogError("Image object sent from client is null.");
                return BadRequest("Image object is null");
            }
            else if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid image details sent from client.");
                return BadRequest(errorRes);
            }

            await _service.Employee.UpdateImageBlob(imageUpdate);
           _logger.LogInfo($"Image for employee with employee code: {imageUpdate.empCode}, has been updated in db.");
            return Ok();
        }

        [HttpGet("RetrieveImageBlob/{empCode}/{imgDate}", Name = "RetrieveImageBlob")]
        public async Task<IActionResult> RetrieveImageBlob([FromRoute] string empCode, string imgDate)
        {
             _dto.RetreiveBlobReq.empCode = empCode;
            _dto.RetreiveBlobReq.imgDate = imgDate;
            var validationResult = await _retreiveBlobValidator.ValidateAsync(_dto.RetreiveBlobReq);
            var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (errorRes.errorMessage.Count > 0)
            {
                _logger.LogError("Invalid request data sent from client.");
                return BadRequest(errorRes);
            }

            var blob = await _service.Employee.RetrieveImageBlob(empCode, Convert.ToDateTime(imgDate));
            if (blob == null)
            {
               _logger.LogError($"employee image details with employee code: {empCode}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
               _logger.LogInfo($"Returned employee image details with empoloyee code: {empCode}");
                return Ok(JsonConvert.SerializeObject(blob));
            }
        }

        [HttpGet("GetBranchDetails/{flag}/{firmId}/{branchId}/{userId}/{sessionId}/{macId}/{fromDate}/{toDate}", Name = "GetBranchDetails")]
        public async Task<IActionResult> GetBranchDetails([FromRoute] string flag, int firmId, int branchId, int userId, string sessionId, string macId, string fromDate, string toDate)
        {
            _dto.DetailsReq.flag = flag;
            _dto.DetailsReq.firmId = firmId;
            _dto.DetailsReq.branchId = branchId;
            _dto.DetailsReq.userId = userId;
            _dto.DetailsReq.sessionId = sessionId;
            _dto.DetailsReq.macId = macId;
            _dto.DetailsReq.fromDate = Convert.ToDateTime(fromDate);
            _dto.DetailsReq.toDate = Convert.ToDateTime(toDate);

             // var validationResult = await _pdVlaidator.ValidateAsync(_dto.DetailsReq);
           //  var validationResult = await _portalDetailValidator.ValidateAsync(_dto.DetailsReq);
             //var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            //  public class BranchInfoValidator : AbstractValidator<PortalDetailReqDto>
            //var errorRes = _helper.VHelper.ReturnErrorRes(validationResult);

            if (_dto.DetailsReq.macId == null || _dto.DetailsReq.sessionId == null)
            //  if (errorRes.errorMessage.Count > 0)
            {
                 _logger.LogError("Invalid portal details sent from client.");
                return BadRequest(null);
            }

            var portal = await _service.Employee.GetPortalDetails(flag, firmId, branchId, userId, sessionId, macId, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate));
            if (portal == null)
            {
                //  _logger.LogError($"Branch details with user id: {userId}, hasn't been found in db.");
                return NotFound();
            }
            else
            {
                //   _logger.LogInfo($"Returned branch details successfully with user id: {userId}");
                return Ok(JsonConvert.SerializeObject(portal));
            }
        }



    }
}
