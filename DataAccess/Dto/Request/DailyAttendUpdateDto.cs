using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Dto.Request
{
    public class DailyAttendUpdateDto
    {
        public int empCode { get; set; }
        public int branchId { get; set; }
        public DateTime punchTime { get; set; }
        public string ipd { get; set; }
    }
}
