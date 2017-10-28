using System;
using System.ComponentModel.DataAnnotations;

namespace Soonish.Forms.Models
{
    public class ResponseInputModel
    {
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Phone { get; set; }
    }
}
