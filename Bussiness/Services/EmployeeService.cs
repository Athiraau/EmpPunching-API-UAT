using Business.Contracts;
using DataAccess.Contracts;
using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly ServiceHelper _service;
        private readonly IConfiguration _config;
        private readonly DtoWrapper _dto;

        public EmployeeService(IRepositoryWrapper repository, ServiceHelper service, IConfiguration config, DtoWrapper dto)
        {
            _repository = repository;
            _service = service;
            _config= config;
            _dto = dto;
        }        

        public async Task<Employee> GetEmployeeByCode(int empCode)
        {
            Employee employee = await _repository.Employee.GetEmployeeByCode(empCode);
            return employee;
        }

        public async Task<Firm> GetEmployeeFirm(int empCode)
        {
            Firm firm = await _repository.Employee.GetEmployeeFirm(empCode);
            return firm;
        }

        public async Task<DailyAttendResDto> UpdateDailyAttend(DailyAttendUpdateDto dailyAttend)
        {
            DailyAttendResDto dailyAttendRes = null;
            dailyAttendRes = await _repository.Employee.UpdateDailyAttend(dailyAttend);

            return dailyAttendRes;
        }

        public async Task UpdateImageBlob(ImageUpdateReqDto imageUpdate)
        {


            int compressSize = Convert.ToInt32(_config["Image:CompressionSize"]);
            ImageUpdateRepoDto repoReq = new ImageUpdateRepoDto();
            repoReq.empCode = imageUpdate.empCode;

            byte[] imageBytes = Convert.FromBase64String(imageUpdate.pPhoto);
            imageBytes = _service.PHelper.ReduceImageSize(imageBytes, compressSize);
            repoReq.pPhoto = imageBytes;

            await _repository.Employee.UpdateImageBlob(repoReq);


            //ImageUpdateRepoDto repoReq = new ImageUpdateRepoDto();
            //repoReq.empCode = imageUpdate.empCode;
            //byte[] imageBytes = Convert.FromBase64String(imageUpdate.pPhoto);
            //repoReq.pPhoto = imageBytes;

            //await _repository.Employee.UpdateImageBlob(repoReq);

        }

        public async Task<GeneralDataResDto> RetrieveImageBlob(string empCode, DateTime imgDate)
        {
            int decompressSize = Convert.ToInt32(_config["Image:DecompressionSize"]);
            byte[] blob = await _repository.Employee.RetrieveImageBlob(empCode, imgDate);
            blob = _service.PHelper.IncreaseImageSize(blob, decompressSize);
            _dto.GeneralDataRes.response = Convert.ToBase64String(blob);
            return _dto.GeneralDataRes;
        }

        public async Task<dynamic> GetPortalDetails(string flag, int firmId, int branchId, int userId, string sessionId, string macId, DateTime fromDate, DateTime toDate)
        {
            var portal = await _repository.Employee.GetPortalDetails(flag, firmId, branchId, userId, sessionId, null, macId, fromDate, toDate);
            return portal;
        }
    }
}
