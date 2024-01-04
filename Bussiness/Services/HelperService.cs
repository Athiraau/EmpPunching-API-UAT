using Business.Contracts;
using DataAccess.Contracts;
using DataAccess.Dto.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Business.Services
{
    public class HelperService: IHelperService
    {
        public readonly IConfiguration _config;
        private readonly IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _env;

        private readonly ServiceHelper _helper;

        public HelperService(IRepositoryWrapper repository, ServiceHelper helper, IWebHostEnvironment env, 
            IConfiguration config)
        {
            _repository = repository;
            _helper = helper;
            _env = env;
            _config = config;
        }

        public async Task<CheckCredentialResDto> CheckEmployeeCredentials(int empCode, string password)
        {
            if (!_env.IsDevelopment())
            {
                password = _helper.PHelper.RemoveSpecialCharacters(_helper.PHelper.Encrypt(Convert.ToString(empCode), password));
            }
            CheckCredentialResDto checkCredRes = await _repository.Helper.EmpPasswordCheck(empCode, password);
            return checkCredRes;
        }

        public async Task<CheckCredentialResDto> HostnameCheck(string hostName)
        {
            CheckCredentialResDto checkCredRes = await _repository.Helper.HostnameCheck(hostName);
            return checkCredRes;
        }
    }
}
