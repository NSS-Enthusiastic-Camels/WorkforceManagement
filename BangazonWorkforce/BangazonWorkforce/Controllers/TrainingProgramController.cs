﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using BangazonWorkforce.Models;
namespace BangazonWorkforce.Controllers
{
    public class TrainingProgramController : Controller
    {
        private IConfiguration _config;
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public TrainingProgramController(IConfiguration config)
        {
            _config = config;
        }
    
        // GET: Training
        public async Task<ActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = @"SELECT tp.Name, tp.StartDate
            FROM TrainingProgram as tp
            Where GetDate() < StartDate";
                IEnumerable<TrainingProgram> trainingprograms = await conn.QueryAsync<TrainingProgram>(sql);


                return View(trainingprograms);
            }
        }

        // GET: Training/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Training/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Training/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Training/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Training/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Training/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Training/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}