using Login.Models;
using Login.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//    aksh randi
namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DAL dAL;
      
        Company comp = new Company();
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration, DAL dal)
        {
            _configuration = configuration;
            dAL = dal;
           
        }
       

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            DataTable result = await dAL.ExecuteStoredProcedureAsync("USP_VendorDetails", "@EmailId", model.Email, "@Password", model.Password);
            if (result.Rows.Count > 0)
            {
                string Retrunresult = dAL.ConvertDataTabletoString(result);
                return StatusCode(StatusCodes.Status200OK, Retrunresult);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Please Enter Valid Details");
            }   
        }
    }
}
