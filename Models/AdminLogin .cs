using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
	public class AdminLogin
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
