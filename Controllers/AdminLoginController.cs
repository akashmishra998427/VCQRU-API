using Login.Models;
using Login.Requests; // Make sure to include the Requests namespace
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminLoginController : ControllerBase
	{
		private readonly DAL _dal;

		public AdminLoginController(DAL dal)
		{
			_dal = dal;
		}

		[HttpPost("Signin")]
		public async Task<IActionResult> Login([FromBody] AdminLogin loginRequest)
		{
			if (loginRequest == null || string.IsNullOrEmpty(loginRequest.UserId) || string.IsNullOrEmpty(loginRequest.Password))
			{
				return BadRequest("Invalid login request.");
			}

			// Validate admin login
			bool isValidUser = await _dal.ValidateAdminLogin(loginRequest.UserId, loginRequest.Password);

			if (isValidUser)
			{
				// Here you could create a JWT token or set up a session if needed
				return Ok("Login successful.");
			}
			else
			{
				return Unauthorized("Invalid credentials or region.");
			}
		}
	}
}
