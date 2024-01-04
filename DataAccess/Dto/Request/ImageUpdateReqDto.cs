using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Dto.Request
{
    public class ImageUpdateReqDto
    {
        [Required]
        public int empCode { get; set; }
        [Required]
        public String pPhoto { get; set; } 
    }
}
