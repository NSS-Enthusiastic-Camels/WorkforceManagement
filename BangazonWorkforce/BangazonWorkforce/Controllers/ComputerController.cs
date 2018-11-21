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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            

            string sql = $@"SELECT
            
                                c.Id,
                                c.PurchaseDate,
                                c.DecomissionDate,
                                c.Make,
                                c.Manufacturer
                            FROM Computer c
                            WHERE c.Id = {id}; ";

            using (IDbConnection conn = Connection)

                // not going to use view model here?
            //ComputerDetailViewModel model = new ComputerDetailViewModel();
            {
                Computer computerQuery = await conn.QuerySingleAsync<Computer>(sql);

                if (computerQuery == null)
                {
                    return NotFound();
                }
                return View(computerQuery);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseDate, DecommissionDate, Make, Manufacturer")] Computer computer)
        {
            if (ModelState.IsValid)
            {
                string sql = $@"
                    INSERT INTO Computer
                        ( PurchaseDate, DecommissionDate, Make, Manufacturer )
                        VALUES
                        ( '{computer.PurchaseDate}', null, '{computer.Make}', '{computer.Manufacturer}' )
                    ";

                using (IDbConnection conn = Connection)
                {
                    int rowsAffected = await conn.ExecuteAsync(sql);

                    if (rowsAffected > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            return View(computer);
        }
    }
}

    









     