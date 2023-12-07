using Miniviableproject.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Miniviableproject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly SchoolDbContext _dbContext;

        public TeacherController()
        {
            _dbContext = new SchoolDbContext();
        }

        public ActionResult Index()
        {
            var teachers = GetTeachersFromDatabase();
            return View(teachers);
        }

        public ActionResult List()
        {
            var teachers = GetTeachersFromDatabase();
            return View("List", teachers);
        }

        public ActionResult Show(int id)
        {
            var teacher = GetTeacherById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View("Show", teacher);
        }

        // GET: /Teacher/Add
        public ActionResult Add()
        {
            
            var viewModel = new Teacher(); 
            return View(viewModel);
        }

        // POST: /Teacher/AddTeacher
        [HttpPost]
        public ActionResult AddTeacher(Teacher newTeacher)
        {
            if (ModelState.IsValid)
            {
                InsertTeacher(newTeacher);
                return RedirectToAction("List");
            }


            return View("Add", newTeacher);
        }

        public ActionResult Delete(int id)
        {
            var teacher = GetTeacherById(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View(teacher);
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteTeacher(id);
            return RedirectToAction("List");
        }

        public ActionResult Edit(int id)
        {
            var teacher = GetTeacherById(id);

            if (teacher == null)
            {
                return HttpNotFound();
            }

            return View("Edit", teacher);
        }

        [HttpPost]
        public ActionResult Edit(Teacher updatedTeacher)
        {
            if (ModelState.IsValid)
            {
                UpdateTeacher(updatedTeacher);
                return RedirectToAction("List");
            }

            return View("Edit", updatedTeacher);
        }

        private List<Teacher> GetTeachersFromDatabase()
        {
            var teachers = new List<Teacher>();
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();
                var sql = "SELECT * FROM Teachers";
                using (var command = new MySqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var teacher = MapTeacherFromDataReader(reader);
                            teachers.Add(teacher);
                        }
                    }
                }
            }
            return teachers;
        }

        private Teacher GetTeacherById(int id)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();
                var sql = "SELECT * FROM Teachers WHERE teacherid = @teacherid";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@teacherid", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapTeacherFromDataReader(reader);
                        }
                        return null;
                    }
                }
            }
        }

        private void InsertTeacher(Teacher newTeacher)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();
                var sql = "INSERT INTO Teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) " +
                          "VALUES (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    MapTeacherToCommandParameters(newTeacher, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateTeacher(Teacher updatedTeacher)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();
                var sql = "UPDATE Teachers SET teacherfname = @teacherfname, teacherlname = @teacherlname, " +
                          "employeenumber = @employeenumber, hiredate = @hiredate, salary = @salary " +
                          "WHERE teacherid = @teacherid";
                using (var command = new MySqlCommand(sql, connection))
                {
                    MapTeacherToCommandParameters(updatedTeacher, command);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteTeacher(int id)
        {
            using (var connection = _dbContext.AccessDatabase())
            {
                connection.Open();
                var sql = "DELETE FROM Teachers WHERE teacherid = @teacherid";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@teacherid", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        private Teacher MapTeacherFromDataReader(MySqlDataReader reader)
        {
            return new Teacher
            {
                teacherid = Convert.ToInt32(reader["teacherid"]),
                teacherfname = reader["teacherfname"].ToString(),
                teacherlname = reader["teacherlname"].ToString(),
                employeenumber = reader["employeenumber"].ToString(),
                hiredate = Convert.ToDateTime(reader["hiredate"]),
                salary = Convert.ToDecimal(reader["salary"])
            };
        }

        private void MapTeacherToCommandParameters(Teacher teacher, MySqlCommand command)
        {
            command.Parameters.AddWithValue("@teacherfname", teacher.teacherfname);
            command.Parameters.AddWithValue("@teacherlname", teacher.teacherlname);
            command.Parameters.AddWithValue("@employeenumber", teacher.employeenumber);
            command.Parameters.AddWithValue("@hiredate", teacher.hiredate);
            command.Parameters.AddWithValue("@salary", teacher.salary);
            command.Parameters.AddWithValue("@teacherid", teacher.teacherid);
        }
    }
}
