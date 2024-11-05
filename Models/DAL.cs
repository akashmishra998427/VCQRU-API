using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Data;

using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Newtonsoft.Json;

namespace Login.Models
{
	public class DAL
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly string _connectionString;

		public DAL(IWebHostEnvironment webHostEnvironment, string connectionString)
		{
			_connectionString = connectionString;
			_webHostEnvironment = webHostEnvironment;
		}
		public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, string Paraname1 = "", string Paravalue1 = "", string Paraname2 = "", string Paravalue2 = "", string Paraname3 = "", string Paravalue3 = "", string Paraname4 = "", string Paravalue4 = "", string Paraname5 = "", string Paravalue5 = "", string Paraname6 = "", string Paravalue6 = "", string Paraname7 = "", string Paravalue7 = "", string Paraname8 = "", string Paravalue8 = "", string Paraname9 = "", string Paravalue9 = "", string Paraname10 = "", string Paravalue10 = "")
		{
			using (var connection = new SqlConnection(_connectionString))
			using (var command = new SqlCommand(procedureName, connection))
			using (var adapter = new SqlDataAdapter(command))
			{
				command.CommandType = CommandType.StoredProcedure;
				if (Paraname1 != "" && Paravalue1 != "")
					command.Parameters.AddWithValue(Paraname1, Paravalue1);
				if (Paraname2 != "" && Paravalue2 != "")
					command.Parameters.AddWithValue(Paraname2, Paravalue2);
				if (Paraname3 != "" && Paravalue3 != "")
					command.Parameters.AddWithValue(Paraname3, Paravalue3);
				if (Paraname4 != "" && Paravalue4 != "")
					command.Parameters.AddWithValue(Paraname4, Paravalue4);
				if (Paraname5 != "" && Paravalue5 != "")
					command.Parameters.AddWithValue(Paraname5, Paravalue5);
				if (Paraname6 != "" && Paravalue6 != "")
					command.Parameters.AddWithValue(Paraname6, Paravalue6);
				if (Paraname7 != "" && Paravalue7 != "")
					command.Parameters.AddWithValue(Paraname7, Paravalue7);
				if (Paraname8 != "" && Paravalue8 != "")
					command.Parameters.AddWithValue(Paraname8, Paravalue8);
				if (Paraname9 != "" && Paravalue9 != "")
					command.Parameters.AddWithValue(Paraname9, Paravalue9);
				if (Paraname10 != "" && Paravalue10 != "")
					command.Parameters.AddWithValue(Paraname10, Paravalue10);

				var dataTable = new DataTable();
				await connection.OpenAsync();
				adapter.Fill(dataTable);
				return dataTable;
			}
		}

		public string ConvertDataTabletoString(DataTable dt)
		{
			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;
			foreach (DataRow dr in dt.Rows)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in dt.Columns)
				{
					row.Add(col.ColumnName, dr[col]);
				}
				rows.Add(row);
			}
			return JsonConvert.SerializeObject(rows);
		}

		public static bool hasSpecialChar(string input)
		{
			string specialChar = @"$";
			foreach (var item in specialChar)
			{
				if (input.Contains(item)) return true;
			}

			if (input.Contains("--")) return true;

			return false;
		}

		public void SendEmail(string sendTo, string subject, string body)
		{
			try
			{
				var msg = new MailMessage();
				msg.From = new MailAddress("sales@vcqru.com", "VCQRU");
				msg.To.Add(sendTo);
				msg.IsBodyHtml = true;
				msg.Subject = subject;
				msg.Body = body;

				var sc = new SmtpClient();
				sc.Host = "smtp.gmail.com";
				sc.EnableSsl = true;
				sc.UseDefaultCredentials = false;
				sc.Credentials = new NetworkCredential("sales@vcqru.com", "iaqesbfqqugepseh");
				sc.Port = 587;
				sc.DeliveryMethod = SmtpDeliveryMethod.Network;
				sc.Send(msg);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error sending email: " + ex.Message);
			}
		}

		public int RandomNumber(int min, int max)
		{
			Random random = new Random();
			return random.Next(min, max);
		}

		public void LogSMS(string message)
		{
			try
			{
				string logFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "LogManager", "smslog.txt");

				using (StreamWriter sr = new StreamWriter(logFilePath, true))
				{
					sr.WriteLine($"{System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt")} : {message}");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error logging SMS: " + ex.Message);
				// Handle the exception as needed
			}
		}

		public void SendSmsfromknowlarity(string Message, string phone)
		{
			string str = "";
			try
			{
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				str = "https://api.infobip.com/sms/1/text/query?username=accomplish&password=Password@123&from=iVCQRU&to=" + phone + "&text=" + Message;

				WebRequest request = WebRequest.Create(str);
				request.Method = "POST";
				string postData = "";
				byte[] byteArray = Encoding.UTF8.GetBytes(postData);
				request.Headers.Add("auth_key", "fJeVv745L-rI6TPSZqgb-Z3U6mvgZ-ODYI0");
				request.ContentType = "application/x-www-form-urlencoded"; // application/x-www-form-urlencoded
				request.ContentLength = byteArray.Length;
				Stream dataStream = request.GetRequestStream();
				dataStream.Write(byteArray, 0, byteArray.Length);
				dataStream.Close();
				WebResponse response = request.GetResponse();
				Console.WriteLine(((HttpWebResponse)response).StatusDescription);
				dataStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(dataStream);
				string responseFromServer = reader.ReadToEnd();
				Console.WriteLine(responseFromServer);
				reader.Close();
				dataStream.Close();

				LogSMS("Mobile : " + phone + ",   " + "Content : " + Message + ",   " + "Response : " + responseFromServer);

				response.Close();
			}
			catch (Exception ex)
			{
				LogSMS(DateTime.Now.ToLongDateString() + ":" + ex.Message);
			}
		}

		#region Method to validate admin login
		public async Task<bool> ValidateAdminLogin(string username, string password)
		{
			// SQL query to check if the admin user exists with the provided username and password or not
			string query = "SELECT * FROM Admin_login WHERE User_Id = @User_Id AND Password = @Password";

			using (var connection = new SqlConnection(_connectionString))
			using (var command = new SqlCommand(query, connection))
			{
				// Adding parameters to the command to prevent SQL injection
				command.Parameters.AddWithValue("@User_Id", username);
				command.Parameters.AddWithValue("@Password", password);

				await connection.OpenAsync();

				// Execute the query and get the result
				var result = await command.ExecuteScalarAsync();

				// Return true if the count is greater than 0, indicating valid login
				return Convert.ToInt32(result) > 0;
			}
		}

		#endregion
		#region method to execute all queries

		public async Task<int> ExecuteQueryAsync(string query, Dictionary<string, object> parameters)
		{
			using (var connection = new SqlConnection(_connectionString)) // Assuming you have a connection string field
			{
				await connection.OpenAsync();

				using (var command = new SqlCommand(query, connection))
				{
					// Add parameters to the SQL command
					foreach (var param in parameters)
					{
						command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
					}

					return await command.ExecuteNonQueryAsync(); // Execute the query
				}
			}
		}
		#endregion

		#region Method ti insert the Consumer registration data in M_Consumer Table

		public async Task<bool> InsertConsumerAsync(string consumerName, string mobileNo, string city, string pinCode, string state)
		{
			string query = @"INSERT INTO M_Consumer (ConsumerName, MobileNo, City, PinCode, State, Entry_Date, IsActive)
                     VALUES (@ConsumerName, @MobileNo, @City, @PinCode, @State, @EntryDate, @IsActive)";

			var parameters = new Dictionary<string, object>
	{
		{ "@ConsumerName", consumerName },
		{ "@MobileNo", mobileNo },
		{ "@City", city },
		{ "@PinCode", pinCode },
		{ "@State", state },
		{ "@EntryDate", DateTime.Now },
		{ "@IsActive", 1 }
	};

			try
			{
				await ExecuteQueryAsync(query, parameters);
				return true; // Insert successful
			}
			catch (Exception ex)
			{
				// Log the exception details (you can use your logging framework here)
				Console.WriteLine($"Error inserting consumer data: {ex.Message}");
				return false; // Insert failed
			}
		}


		#endregion
	}
}

