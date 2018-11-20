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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (IDbConnection conn = Connection)
            {
                Computer computer = await GetById(id.Value);
                if (computer == null)
                {
                    return NotFound();
                }
                string sql = $@"SELECT 
                                      c.Id,
                                      c.PurchaseDate,
                                      c.DecomissionDate,
                                      c.Make,
                                      c.Manufacturer,
                                      
        
                         FROM Computer c 
                              
                         WHERE c.Id = {id} ";

                ComputerDetailViewModel model = new ComputerDetailViewModel();

                IEnumerable<Computer> computers = (await conn.QueryAsync<Computer>(sql)).ToList();

                return View(computer);
            }

        }



        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Computer computer = await GetById(id.Value);
            if (computer == null)
            {
                return NotFound();
            }
            return View(computer);
        }

        private Task<Computer> GetById(int value)
        {
            throw new NotImplementedException();
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"DELETE FROM Computer WHERE id = {id}";
                await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}




     