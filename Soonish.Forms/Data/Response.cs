using System.ComponentModel.DataAnnotations;

namespace Soonish.Forms.Data
{
    public class Response
    {

        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Phone { get; set; }
    }
}