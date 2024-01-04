using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto.Request
{
    public class PortalDetailReqDto
    {
        public string flag { get; set; }
        public int firmId { get; set; }
        public int branchId { get; set; }
        public int userId { get; set; }
        public string sessionId { get; set; }
        public string macId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}
