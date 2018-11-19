using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonWorkforce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using BangazonWorkforce.Models.ViewModels;

namespace BangazonWorkforce.Controllers
{
    public class ComputerController : Controller
    {
        private IConfiguration _config;
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public ComputerController(IConfiguration config)
        {
            _config = config;
        }
    [HttpGet]
    public async Task<IActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"SELECT c.Id, 
                                       c.PurchaseDate, 
                                       c.DecomissionDate, 
                                       c.Make, 
                                       c.Manufacturer 
                                       FROM Computer c;";
                              


                List<Computer> computer = (await conn.QueryAsync<Computer>(sql)).ToList();

                return View(computer);
            }
        }

    }
}

    
     