﻿using System;
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
    public class EmployeeController : Controller
    {
        private IConfiguration _config;
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public EmployeeController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = @"SELECT e.Id, 
                                      e.FirstName,
                                      e.LastName, 
                                      e.IsSupervisor,
                                      e.DepartmentId,
                                      d.Id,
                                      d.Name,
                                      d.Budget
                                 FROM Employee e JOIN Department d on e.DepartmentId = d.Id
                             ORDER BY e.Id";
                IEnumerable<Employee> employees = await conn.QueryAsync<Employee, Department, Employee>(
                    sql,
                    (employee, department) =>
                    {
                        employee.Department = department;
                        return employee;
                    });
                EmployeeIndexViewModel model = new EmployeeIndexViewModel();
                model.Employees = employees;
                return View(model);
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
                Employee employee = await GetById(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }
                string sql = $@"SELECT e.Id, 
                                      e.FirstName,
                                      e.LastName, 
                                      e.IsSupervisor,
                                      e.DepartmentId,
                                      d.Id,
                                      d.Name,
                                      d.Budget,
                                      c.Id,
                                      c.PurchaseDate,
                                      c.DecomissionDate,
                                      c.Make,
                                      c.Manufacturer,
                                      tp.Id,
                                      tp.Name,
                                      tp.StartDate,
                                      tp.EndDate,
                                      tp.MaxAttendees
        
                       FROM Employee e 
                               
                                LEFT JOIN ComputerEmployee on ComputerEmployee.EmployeeId = e.Id
                                LEFT JOIN Computer c on c.Id = ComputerEmployee.ComputerId
                                LEFT JOIN Department d on d.Id =e.DepartmentId
                                LEFT JOIN EmployeeTraining on EmployeeTraining.EmployeeId = e.Id
                                LEFT JOIN TrainingProgram tp ON tp.Id = EmployeeTraining.TrainingProgramId 

                WHERE e.Id = {id} ";
                //list async types passed to queary in order that was listed in Select statement

                EmployeeDetailViewModel model = new EmployeeDetailViewModel();


                IEnumerable<Employee> employees = await conn.QueryAsync<Employee, Department, Computer, TrainingProgram, Employee>(
                    sql,
                    (emp, department, computer, trainingProgram) =>
                    {
                        if (model.DepartmentName == null)
                        {

                            model.FirstName = emp.FirstName;
                            model.LastName = emp.LastName;
                            model.DepartmentName = department.Name;
                            
                        }

                        if (computer != null)
                        {
                            model.ComputerManufacturer = computer.Manufacturer;
                            model.ComputerMake = computer.Make;
                        }

                        if (!model.TrainingPrograms.Contains(trainingProgram))
                        {
                            model.TrainingPrograms.Add(trainingProgram);
                        }
                        return employee;
                    });
                return View(model);
            }

        }

     

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            List<Department> allDepartments = await GetAllDepartments();
            Models.ViewModels.EmployeeEditViewModel viewmodel = new Models.ViewModels.EmployeeEditViewModel
            {
                AllDepartments = allDepartments
            };
            return View(viewmodel);
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.ViewModels.EmployeeEditViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                List<Department> allDepartments = await GetAllDepartments();
                viewmodel.AllDepartments = allDepartments;
                return View(viewmodel);
            }

            Employee employee = viewmodel.Employee;

            using (IDbConnection conn = Connection)
            {
                string sql = $@"INSERT INTO Employee (
                                    FirstName, LastName, IsSupervisor, DepartmentId
                                ) VALUES (
                                    '{employee.FirstName}', '{employee.LastName}',
                                    {(employee.IsSupervisor ? 1 : 0)}, {employee.DepartmentId}
                                );";

                await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<Department> allDepartments = await GetAllDepartments();
            List<Computer> allComputers = await GetAllComputers();
            Employee employee = await GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            Models.ViewModels.EmployeeEditViewModel viewmodel = new Models.ViewModels.EmployeeEditViewModel
            {
                Employee = employee,
                AllDepartments = allDepartments,
                AllComputers = allComputers
            };

            return View(viewmodel);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.ViewModels.EmployeeEditViewModel viewmodel)
        {
            if (id != viewmodel.Employee.Id)
            {
                return NotFound();
            }

            //if (!ModelState.IsValid)
            //{
            //    List<Department> allDepartments = await GetAllDepartments();
            //    viewmodel.AllDepartments = allDepartments;
            //    return View(viewmodel);
            //}

            Employee employee = viewmodel.Employee;

            using (IDbConnection conn = Connection)
            {
                string sql = $@"UPDATE Employee 
                                   SET FirstName = '{employee.FirstName}', 
                                       LastName = '{employee.LastName}', 
                                       IsSupervisor = {(employee.IsSupervisor ? 1 : 0)},
                                       DepartmentId = {employee.DepartmentId}
                                 WHERE id = {id}";

                string ComputerSql = $@"INSERT INTO ComputerEmployee 
                                   (EmployeeId, ComputerId, AssignDate) 
                                 VALUES ({id}, {viewmodel.Employee.Computer.Id}, '{DateTime.Now}')";

                await conn.ExecuteAsync(sql + ComputerSql);
                return RedirectToAction(("Index"));
            }
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = await GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"DELETE FROM Employee WHERE id = {id}";
                await conn.ExecuteAsync(sql);
                return RedirectToAction(nameof(Index));
            }
        }


        private async Task<Employee> GetById(int id)
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"SELECT e.Id, 
                                       e.FirstName,
                                       e.LastName, 
                                       e.IsSupervisor,
                                       e.DepartmentId,
                                       d.Id,
                                       d.Name,
                                       d.Budget
                                  FROM Employee e JOIN Department d on e.DepartmentId = d.Id
                                 WHERE e.id = {id}";
                IEnumerable<Employee> employees = await conn.QueryAsync<Employee, Department, Employee>(
                    sql,
                    (employee, department) =>
                    {
                        employee.Department = department;
                        return employee;
                    });

                return employees.SingleOrDefault();
            }
        }

        private async Task<List<Department>> GetAllDepartments()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"SELECT Id, Name, Budget FROM Department";

                IEnumerable<Department> departments = await conn.QueryAsync<Department>(sql);
                return departments.ToList();
            }
        }

        private async Task<List<Computer>> GetAllComputers()
        {
            using (IDbConnection conn = Connection)
            {
                string sql = $@"SELECT c.Id, c.PurchaseDate, c.DecomissionDate, c.Make, c.Manufacturer 
                    FROM Computer c
                     LEFT JOIN ComputerEmployee ce on ce.ComputerId = c.Id
                               WHERE ce.Id IS NULL";;

                IEnumerable<Computer> departments = await conn.QueryAsync<Computer>(sql);
                return departments.ToList();
            }
        }
    }
}
