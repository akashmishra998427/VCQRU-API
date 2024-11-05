using Login.Models;
using Login.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorRegistrationController : ControllerBase
    {
        private readonly DAL dAL;

        Company comp = new Company();
        private readonly IConfiguration _configuration;

        public VendorRegistrationController(IConfiguration configuration, DAL dal)
        {
            _configuration = configuration;
            dAL = dal;

        }


        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(Registration Reg)
        {

            DataTable result = await dAL.ExecuteStoredProcedureAsync("GetCompRegByEmail", "@Email", Reg.Email_Id, "@Companyname",Reg.Comp_name);
            if (result.Rows[0]["Result"].ToString()== "Already Registered")
            {
                return StatusCode(StatusCodes.Status409Conflict, "Email Already Registered");
            }
            else
            {
                int otpEmail = dAL.RandomNumber(1000, 9999);
                dAL.SendEmail("bipin@vcqru.com", "OTP", "Your Login Otp is :" + otpEmail + "");
                int otpmobile = dAL.RandomNumber(1000, 9999);
                dAL.SendSmsfromknowlarity("Your Otp For Login is : " + otpmobile, Reg.Mobileno);
                DateTime expDate = System.DateTime.Now.AddYears(1);
                DataTable saveotp = await dAL.ExecuteStoredProcedureAsync("USP_SaveOtp", "@OptEmail", otpEmail.ToString(), "@OptMobile", otpmobile.ToString(), "@Mobileno", Reg.Mobileno, "@Email_id", Reg.Email_Id, "@companyname", Reg.Comp_name, "@Expirydate", expDate.ToString("yyyy-MM-dd HH:mm:ss"), "@Status", "0");
                return StatusCode(StatusCodes.Status200OK, "Otp send Sucessfully on registered Email and Mobile Number");
            }
        }

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP(ValidateOtp Otp)
        {
            DataTable saveotp = await dAL.ExecuteStoredProcedureAsync("USP_ValidateOTP", "@MobileOTP", Otp.OTP_Mobile, "@EmailOTP", Otp.OTP_EMAIL, "@Email_id", Otp.EMAIL);
            if (saveotp.Rows[0][0].ToString() == "OTP validation successful")
            {

                return StatusCode(StatusCodes.Status200OK, saveotp.Rows[0][0].ToString());
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid Otp");
            }
        }
    }
}
