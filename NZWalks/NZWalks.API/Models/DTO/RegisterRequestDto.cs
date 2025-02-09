using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string userName {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string[] Roles { get; set; }
    }
}
