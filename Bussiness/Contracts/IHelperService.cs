using DataAccess.Dto.Response;

namespace Business.Contracts
{
    public interface IHelperService
    {
        public Task<CheckCredentialResDto> CheckEmployeeCredentials(int empCode, string password);
        public Task<CheckCredentialResDto> HostnameCheck(string hostName);
    }
}
