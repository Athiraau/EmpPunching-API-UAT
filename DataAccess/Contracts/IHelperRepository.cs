using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace DataAccess.Contracts
{
    public interface IHelperRepository
    {
        public Task<CheckCredentialResDto> EmpPasswordCheck(int empCode, string password);
        public Task<CheckCredentialResDto> HostnameCheck(string hostName); 
    }
}
