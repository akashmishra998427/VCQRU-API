using System.ComponentModel.DataAnnotations;

namespace Login.Requests
{
	public class Registration
	{
		[Required]
		[StringLength(100)]
		public required string Comp_name { get; set; }

		[Required]
		[EmailAddress]
		public required string Email_Id { get; set; }

		[Required]
		[StringLength(100)]
		public required string Contact_Person { get; set; }

		[Required]
		[Phone]
		public required string Mobileno { get; set; }
	}
}
