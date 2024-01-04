using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Dto.Request
{
    public class ImageUpdateRepoDto
    {
        [Required]
        public int empCode { get; set; }
        [Required]
        public byte[] pPhoto { get; set; }
    }
}
