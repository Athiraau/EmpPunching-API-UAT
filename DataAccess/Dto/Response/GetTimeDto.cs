using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Dto.Response
{
    public class GetTimeDto
    {
        public DateTime early_time { get; set; }

        public int shift_id { get; set; }

        public DateTime start_time { get; set; }
    }
}
