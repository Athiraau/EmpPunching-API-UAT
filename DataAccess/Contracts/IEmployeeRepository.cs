using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
	public interface IEmployeeRepository
	{
		public Task<Employee> GetEmployeeByCode(int empCode);
		public Task<Firm> GetEmployeeFirm(int empCode);
		public Task<int> GetStatusByHost(string hostName);
		public Task<int> PasswordCheck(int empCode, string password);
		public Task UpdateImageBlob(ImageUpdateRepoDto imageUpdate);
		public Task<DailyAttendResDto> UpdateDailyAttend(DailyAttendUpdateDto dailyAttend);
        public Task<byte[]> RetrieveImageBlob(string empCode, DateTime imgDate);
		public Task<dynamic> GetPortalDetails(string flag, int firmId, int branchId, int userId, string sessionId, string param, string macId, DateTime fromDate, DateTime toDate);
    }
}
