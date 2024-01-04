using Dapper;
using Dapper.Oracle;
using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Dto;
using DataAccess.Entities;
using System.Data;
using System.Threading.Tasks;
using System;
using DataAccess.Dto.Response;
using DataAccess.Dto.Request;
using Oracle.ManagedDataAccess.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Oracle.ManagedDataAccess.Types;

namespace DataAccess.Repository
{
    public class EmployeeRepository : IEmployeeRepository
	{
		private readonly DapperContext _context;
		private DtoWrapper _dto;

		public EmployeeRepository(DapperContext context, DtoWrapper dto)
		{
			_context = context;
			_dto = dto;
		}

		public async Task<Employee> GetEmployeeByCode(int empCode)
		{
			var query = "select a.emp_name empName,b.shift||'('||b.in_time||'-'||b.out_time||')' shift,to_char(Sysdate, 'hh24:mi:ss') punchTime from employee_master a,time_tab b where a.shift_id=b.shift_id and emp_code=:empCode";

            using var connection = _context.CreateConnection();
            var employee = await connection.QuerySingleOrDefaultAsync<Employee>(query, new { empCode });
            return employee;
        }

		public async Task<Firm> GetEmployeeFirm(int empCode)
		{
			var query = "select count(e.emp_code) empCount, f.firm_id firm from employee_master e inner join employ_firm f on e.emp_code = f.emp_code where e.status_id=1 and f.emp_code=:empCode and e.emp_code>10000  group by f.firm_id";

            using var connection = _context.CreateConnection();
            var empFirm = await connection.QuerySingleOrDefaultAsync<Firm>(query, new { empCode });
            return empFirm;
        }

		public async Task<int> GetStatusByHost(string hostName)
		{
			var query = "select count(1) from BRANCH_SYS_INFO_request t, employee_master em where t.req_by = em.emp_code and (em.grade_id < 7 or (em.branch_id=3531 and em.grade_id<9)) and em.status_id = 1  and t.status = 2  and ((upper(REPLACE(t.mac_id, ':', '')) = upper(replace(:hostname,':', ''))) or  (upper(REPLACE(t.mac_id, ':', '')) = upper(replace(:hostname,':', '')))";

			using var connection = _context.CreateConnection();
			var sysCount = await connection.QuerySingleOrDefaultAsync<int>(query, new { hostName });
			return sysCount;
		}

		public async Task<int> PasswordCheck(int empCode, string password)
		{
			string empCd = empCode.ToString();
			var query = "select count(emp_code) from employee_master where emp_code=:empCd and password= :password and status_id=1";

            using var connection = _context.CreateConnection();
            var empCount = await connection.QuerySingleOrDefaultAsync<int>(query, new { empCd, password });
            return empCount;
        }

		public async Task<DailyAttendResDto> UpdateDailyAttend(DailyAttendUpdateDto dailyAttend)
		{
			var message = string.Empty;
			var dateTime = string.Empty;
			
			var procedureName = "UpdateDailyAttend";
			var parameters = new OracleDynamicParameters();
				parameters.Add("EmpCd", dailyAttend.empCode, OracleMappingType.Int32, ParameterDirection.Input);
				parameters.Add("brno", dailyAttend.branchId, OracleMappingType.Int32, ParameterDirection.Input);
				parameters.Add("punch_time", dailyAttend.punchTime, OracleMappingType.Date, ParameterDirection.Input);
				parameters.Add("ipd", dailyAttend.ipd, OracleMappingType.NVarchar2, ParameterDirection.Input);
				parameters.Add("msg", message, OracleMappingType.Varchar2, ParameterDirection.Output);
				parameters.Add("dtime", dateTime, OracleMappingType.Varchar2, ParameterDirection.Output);
			parameters.BindByName = true;
            using var connection = _context.CreateConnection();
            await connection.QueryFirstOrDefaultAsync
                (procedureName, parameters, commandType: CommandType.StoredProcedure);

            _dto.dailyAttendRes.message = Convert.ToString(parameters.Get<string>("msg"));
            _dto.dailyAttendRes.timestamp = Convert.ToString(parameters.Get<string>("dtime"));

            return _dto.dailyAttendRes;
        }

		public async Task UpdateImageBlob(ImageUpdateRepoDto imageUpdate)
        //{
        //	var message = string.Empty;
        //	var dateTime = string.Empty;

        //	var procedureName = "proc_punch_photo_save";
        //	var parameters = new OracleDynamicParameters();
        //	parameters.Add("empcode", imageUpdate.empCode, OracleMappingType.Int32, ParameterDirection.Input);
        //	parameters.Add("p_photo", imageUpdate.pPhoto, OracleMappingType.Blob, ParameterDirection.Input);
        //	parameters.BindByName = true;
        //          using var connection = _context.CreateConnection();
        //          await connection.QueryFirstOrDefaultAsync
        //              (procedureName, parameters, commandType: CommandType.StoredProcedure);

        //          return;
        //      }


        {
            var sql = " ";

            using var connection = _context.CreateConnection();
            connection.Open();
            OracleParameter[] prm = new OracleParameter[1];
            OracleCommand cmd = (OracleCommand)connection.CreateCommand();

            var sql1 = "select a.early_time,a.shift_id,a.start_time from time_tab a where exists(select b.shift_id from daily_attend b where b.shift_id= a.shift_id and b.emp_code='" + imageUpdate.empCode + "')";
                       
            var result = connection.Query<GetTimeDto>(sql1).SingleOrDefault();
            DateTime ErlyTm = result.early_time; // Early time of Employee
            int ShifTM = result.shift_id; // Early time of Employee
            DateTime StrTm = result.start_time;  // Early time of Employee   

            DateTime CurrTm = DateTime.Parse(DateTime.Now.TimeOfDay.ToString());   // Current time - Punching Time



            if (ShifTM != 7 & ShifTM != 12 & ShifTM != 67 & ShifTM != 29 & ShifTM != 16 & ShifTM != 17 & ShifTM != 83 & ShifTM != 64 & ShifTM != 99)
            {
                
            int res = DateTime.Compare(ErlyTm, CurrTm);                                               // Comparing timing

           
           
            if (res > 0)                // IF  PUNCHING TIME < EARLY TIME  - MORNING PHOTO PUNCH
            {
                sql = "update dms.TBL_ATTEND_PHOTO set m_photo =:ph where emp_code =  '" + imageUpdate.empCode + "'  and curr_date = to_date(sysdate) and m_photo is null";
            }
            else                        // IF  PUNCHING TIME > EARLY TIME   -  EVENING PHOTO PUNCH
            {
                sql = "update dms.TBL_ATTEND_PHOTO set e_photo =:ph where emp_code =  '" + imageUpdate.empCode + "'  and curr_date = to_date(sysdate) and e_photo is null";
            }

            }
            else
            {

                int res1 = DateTime.Compare(ErlyTm, CurrTm);

                int res2 = DateTime.Compare(CurrTm, StrTm);

                // if (CurrTm >= ErlyTm & CurrTm < StrTm)

                if (res1>0 & res2>0)

                {
                    sql = "update dms.TBL_ATTEND_PHOTO set e_photo=:ph where emp_code =  '" + imageUpdate.empCode + "'  and curr_date = to_date(sysdate) and e_photo is null";
                }
                else                        // IF  PUNCHING TIME > EARLY TIME   -  EVENING PHOTO PUNCH
                {
                    sql = "update dms.TBL_ATTEND_PHOTO set m_photo =:ph where emp_code =  '" + imageUpdate.empCode + "'  and curr_date = to_date(sysdate) and m_photo is null";
                 }

            }

            prm[0] = cmd.Parameters.Add("ph", OracleDbType.Blob, imageUpdate.pPhoto, ParameterDirection.Input);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            return;
            
        }
        public async Task<byte[]> RetrieveImageBlob(string empCode, DateTime imgDate)
        {
            var query = "select t.m_photo,t.curr_date,t.emp_code from DMS.TBL_ATTEND_PHOTO t where t.emp_code=:empCode and t.m_photo is not null and t.curr_date=:imgDate";

            using var connection = _context.CreateConnection();
            var blob = await connection.QuerySingleOrDefaultAsync<byte[]>(query, new { empCode, imgDate });
            return blob;
        }


        public async Task<dynamic> GetPortalDetails(string flag, int firmId, int branchId, int userId, string sessionId, string param, string macId, DateTime fromDate, DateTime toDate)
        {
            OracleRefCursor result = null;

            var procedureName = "PLP_Portal_Select";
            var parameters = new OracleDynamicParameters();
            parameters.Add("as_flag", flag, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("al_firmid", firmId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("al_bracnchid", branchId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("al_userid", userId, OracleMappingType.Int32, ParameterDirection.Input);
            parameters.Add("as_sessionid", sessionId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("as_param", param, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("as_macid", macId, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("ad_frmdate", fromDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("ad_todate", toDate, OracleMappingType.Date, ParameterDirection.Input);
            parameters.Add("as_outresult", result, OracleMappingType.RefCursor, ParameterDirection.Output);

            parameters.BindByName = true;
            using var connection = _context.CreateConnection();
            var data = await connection.QueryFirstOrDefaultAsync<dynamic>
                    (procedureName, parameters, commandType: CommandType.StoredProcedure);

            return data;
        }
    }
}
