using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace Business.Contracts
{
    public interface IEmployeeService
    {
        public Task<Employee> GetEmployeeByCode(int empCode);
        public Task<Firm> GetEmployeeFirm(int empCode);
        public Task<DailyAttendResDto> UpdateDailyAttend(DailyAttendUpdateDto dailyAttend);
        public Task UpdateImageBlob(ImageUpdateReqDto imageUpdate);

        public Task<GeneralDataResDto> RetrieveImageBlob(string empCode, DateTime imgDate);
        public Task<dynamic> GetPortalDetails(string flag, int firmId, int branchId, int userId, string sessionId, string macId, DateTime fromDate, DateTime toDate);


    }
}
