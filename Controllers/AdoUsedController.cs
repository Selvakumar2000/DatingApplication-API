using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoUsedController : BaseApiController
    {
        private readonly IConfiguration _config;
        public AdoUsedController(IConfiguration config)
        {
            _config = config;      
        }

        [HttpGet("FemaleUsers")]
        public ActionResult<List<string>> GetFemaleUser()
        {
            List<string> names = new List<string>();
            try
            {
                string connectionString = _config.GetConnectionString("DatingAppCon");
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("select UserName from AspNetUsers where Gender='female';", con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader(); //connected architecture

                while (sdr.Read())
                {
                    names.Add((string)sdr["UserName"]);
                }
                con.Close();

                return names;

            }
            catch (Exception ex)
            {
                return BadRequest("Operation Failed  " + ex);
            }
        }
    }
}
