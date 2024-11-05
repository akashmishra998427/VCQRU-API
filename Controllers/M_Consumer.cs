using Login.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Login.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ConsumerController : ControllerBase
	{
		private readonly DAL _dal;

		public ConsumerController(DAL dal)
		{
			_dal = dal;
		}

		[HttpPost("M_ConsumerLogin")]
		public async Task<IActionResult> ConsumerLogin([FromBody] ConsumerLoginDto loginDto)
		{
			if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.MobileNo) || string.IsNullOrWhiteSpace(loginDto.Name) || string.IsNullOrWhiteSpace(loginDto.PinCode))
			{
				return BadRequest("Invalid login parameters.");
			}

			// Fetch city and state from the external API
			string apiUrl = $"https://api.postalpincode.in/pincode/{loginDto.PinCode}";
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(apiUrl);
				if (!response.IsSuccessStatusCode)
				{
					return BadRequest("Invalid pincode.");
				}

				var content = await response.Content.ReadAsStringAsync();

				// Deserialize the response
				dynamic result = JsonConvert.DeserializeObject(content);

				// Check if there are PostOffice entries in the response
				if (result[0].PostOffice != null && result[0].PostOffice.Count > 0)
				{
					// Extract the first PostOffice object
					var postOffice = result[0].PostOffice[0];

					// Get city and state from the response
					string city = postOffice["District"];
					string state = postOffice["State"];

					// Prepare SQL query for insertion
					string query = @"INSERT INTO M_Consumer (ConsumerName, MobileNo, City, PinCode, State, Entry_Date, IsActive)
                                     VALUES (@ConsumerName, @MobileNo, @City, @PinCode, @State, @EntryDate, @IsActive)";

					var parameters = new Dictionary<string, object>
					{
						{ "@ConsumerName", loginDto.Name },
						{ "@MobileNo", loginDto.MobileNo },
						{ "@City", city },
						{ "@PinCode", loginDto.PinCode },
						{ "@State", state },
						{ "@EntryDate", DateTime.Now },
						{ "@IsActive", 1 }
					};

					// Execute the SQL query
					await _dal.ExecuteQueryAsync(query, parameters);
					return Ok("Consumer login successful and data inserted.");
				}
				else
				{
					return BadRequest("No data found for the provided pincode.");
				}
			}
		}
	}
}
