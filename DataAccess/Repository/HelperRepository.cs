using Dapper;
using Dapper.Oracle;
using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Dto;
using DataAccess.Dto.Request;
using DataAccess.Dto.Response;
using DataAccess.Entities;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class HelperRepository : IHelperRepository
	{
		private readonly DapperContext _context;
		private DtoWrapper _dto;

		public HelperRepository(DapperContext context, DtoWrapper dto)
		{
			_context = context;
			_dto = dto;
		}

		public async Task<CheckCredentialResDto> EmpPasswordCheck(int empCode, string password)
		{
          
            var userId = Convert.ToString(empCode);

			

			//var query = "select count(*) from employee_master where emp_code=:userId and password=:password and status_id=1 ";
            var query = "select count(*) from employee_master where emp_code=:userId and password=:password and status_id=1 ";

            using var connection = _context.CreateConnection();
			_dto.checkCredRes.flag = await connection.QuerySingleOrDefaultAsync<int>(query, new { userId, password });

			return _dto.checkCredRes;
		}

		public async Task<CheckCredentialResDto> HostnameCheck(string hostname)
		{
			var query = "select count(1) from BRANCH_SYS_INFO_request t, employee_master em where t.req_by = em.emp_code and (em.grade_id < 7 or (em.branch_id=3531 and em.grade_id<9)) and em.status_id = 1  and t.status = 2  and ((upper(REPLACE(t.mac_id, ':', '')) = upper(replace(:hostname,':', ''))) or (upper(REPLACE(t.mac_id, '-', '')) = upper(replace(:hostname,':', ''))))";

			using var connection = _context.CreateConnection();
			_dto.checkCredRes.flag = await connection.QuerySingleOrDefaultAsync<int>(query, new { hostname });
			
			return _dto.checkCredRes;
		}
	}
}
