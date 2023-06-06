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
    /// <summary>
    /// API controller for managing students.
    /// </summary>
    public class StudentsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of students.
        /// </summary>
        /// <returns>IQueryable of Student objects representing the students.</returns>
        // GET: api/StudentsData/ListStudents
        [HttpGet]
        public IQueryable<Student> Liststudents()
        {
            return db.students;
        }

        /// <summary>
        /// Retrieves a specific student by their ID.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>The Student object representing the student.</returns>
        // GET: api/StudentsData/FindStudent/5
        [ResponseType(typeof(Student))]
        [HttpGet]
        public IHttpActionResult FindStudent(int id)
        {
            Student student = db.students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        /// <summary>
        /// Updates a specific student.
        /// </summary>
        /// <param name="id">The ID of the student to update.</param>
        /// <param name="student">The Student object containing the updated data.</param>
        /// <returns>No content if the update is successful.</returns>
        // POST: api/StudentsData/UpdateStudent/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStudent(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.StudentID)
            {
                return BadRequest();
            }

            db.Entry(student).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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

        /// <summary>
        /// Adds a new student.
        /// </summary>
        /// <param name="student">The Student object representing the new student.</param>
        /// <returns>The created student.</returns>
        // POST: api/StudentsData/AddStudent
        [ResponseType(typeof(Student))]
        [HttpPost]
        public IHttpActionResult AddStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.students.Add(student);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = student.StudentID }, student);
        }

        /// <summary>
        /// Deletes a specific student.
        /// </summary>
        /// <param name="id">The ID of the student to delete.</param>
        /// <returns>The deleted student.</returns>
        // POST: api/StudentsData/DeleteStudent/2
        [ResponseType(typeof(Student))]
        [HttpPost]
        public IHttpActionResult DeleteStudent(int id)
        {
            Student student = db.students.Find(id);
            if (student == null)
            {
                return NotFound();
            }

            db.students.Remove(student);
            db.SaveChanges();

            return Ok(student);
        }

        /// <summary>
        /// Associate a student with a Course (populates the bridging table)
        /// </summary>
        /// <param name="StudentID">The ID of the student.</param>
        /// <param name="CourseId">The ID of the course.</param>
        /// <returns>The HTTP result indicating the success of the operation.</returns>
        // POST: api/StudentsData/RegisterForClass/{StudentID}/{CourseId}
        [ResponseType(typeof(StudentXCourse))]
        [HttpPost]
        [Route("api/StudentsData/RegisterForClass/{StudentID}/{CourseId}")]
        public IHttpActionResult RegisterForClass(int StudentID, int CourseId)
        {
            Course course = db.courses.Find(CourseId);
            Student student = db.students.Find(StudentID);

            if (course == null || student == null)
            {
                return NotFound();
            }

            StudentXCourse studentXCourse = new StudentXCourse()
            {
                StudentID = student.StudentID,
                CourseID = course.CourseID
            };

            db.studentXCourses.Add(studentXCourse);
            db.SaveChanges();

            return Ok();
        }



        /// <summary>
        /// Unassociate a student with a Course (Removes entry from the bridging table)
        /// </summary>
        /// <param name="StudentID">The ID of the student.</param>
        /// <param name="CourseId">The ID of the course.</param>
        /// <returns>The HTTP result indicating the success of the operation.</returns>
        // POST: api/StudentsData/DropAClass/{StudentID}/{CourseId}
        [ResponseType(typeof(StudentXCourse))]
        [HttpPost]
        [Route("api/StudentsData/DropAClass/{StudentID}/{CourseId}")]
        public IHttpActionResult DropAClass(int StudentID, int CourseId)
        {
            Course course = db.courses.Find(CourseId);
            Student student = db.students.Find(StudentID);

            if (course == null || student == null)
            {
                return NotFound();
            }

            StudentXCourse studentXCourse = db.studentXCourses
                .FirstOrDefault(sxc => sxc.StudentID == student.StudentID && sxc.CourseID == course.CourseID);

            if (studentXCourse == null)
            {
                return NotFound();
            }

            db.studentXCourses.Remove(studentXCourse);
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

        private bool StudentExists(int id)
        {
            return db.students.Count(e => e.StudentID == id) > 0;
        }
    }
}
