using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SchoolAdminSite.Models;

namespace SchoolAdminSite.Controllers
{
    public class TeacherDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// returns a list of all teachers in the database
        /// </summary>
        /// <returns>
        /// List of all teachers in the database
        /// </returns>
        // GET: api/TeacherData/ListTeachers
        [HttpGet]
        public IEnumerable<TeacherDto> ListTeachers()
        {

            List<Teacher> Teachers = db.Teachers.ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();

            Teachers.ForEach(t => TeacherDtos.Add(new TeacherDto() {
                TeacherId = t.TeacherId,
                Fname = t.Fname,
                Lname = t.Lname,
                HireDate = t.HireDate,
                Salary = t.Salary
            }));
            return TeacherDtos; 
        }

        // GET: api/TeacherData/FindAnimal/5
        [ResponseType(typeof(Teacher))]
        [HttpGet]
        public IHttpActionResult FindTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            TeacherDto teacherDto = new TeacherDto()
            {
                TeacherId = teacher.TeacherId,
                Fname = teacher.Fname,
                Lname = teacher.Lname,
                HireDate = teacher.HireDate,
                Salary = teacher.Salary
            };
            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }

        // POST: api/TeacherData/UpdateTeacher/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateTeacher(int id, Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }

            db.Entry(teacher).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TeacherData/AddTeacher
        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult AddTeacher(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teachers.Add(teacher);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = teacher.TeacherId }, teacher);
        }

        // POST: api/TeacherData/DeleteTeacher/5
        [ResponseType(typeof(Teacher))]
        [HttpPost]
        public IHttpActionResult DeleteTeacher(int id)
        {
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teachers.Remove(teacher);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(int id)
        {
            return db.Teachers.Count(e => e.TeacherId == id) > 0;
        }
    }
}