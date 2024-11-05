using System.ComponentModel.DataAnnotations;

namespace Login.Requests
{
	public class ValidateOtp
	{
		[Required]
		[EmailAddress]
		public required string EMAIL { get; init; }

		[Required]
		public required string OTP_EMAIL { get; init; }

		[Required]
		public required string OTP_Mobile { get; init; }
	}
}
