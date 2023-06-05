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
using System.Web.Management;
using SchoolAdminSite.Models;

namespace SchoolAdminSite.Controllers
{
    /// <summary>
    /// API controller for managing courses.
    /// </summary>
    public class CoursesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of courses.
        /// </summary>
        /// <returns>List of CourseDto objects representing the courses.</returns>
        // GET: api/CoursesData/listCourses
        [HttpGet]
        public IEnumerable<CourseDto> ListCourses()
        {
            List<Course> Courses = db.courses.ToList();
            List<CourseDto> CourseDtos = new List<CourseDto>();

            Courses.ForEach(c => CourseDtos.Add(new CourseDto()
            {
                CourseID = c.CourseID,
                RoomNum = c.RoomNum,
                Subject = c.Subject,
                Time = c.Time,
                TeacherLName = c.Teacher.Lname
            }));

            return CourseDtos;
        }

        /// <summary>
        /// Retrieves a specific course by its ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>The CourseDto object representing the course.</returns>
        // GET: api/CoursesData/FindCourse/5
        [ResponseType(typeof(Course))]
        [HttpGet]
        public IHttpActionResult FindCourse(int id)
        {
            Course course = db.courses.Find(id);
            CourseDto courseDto = new CourseDto()
            {
                CourseID = course.CourseID,
                RoomNum = course.RoomNum,
                Subject = course.Subject,
                Time = course.Time,
                TeacherLName = course.Teacher.Lname
            };

            if (course == null)
            {
                return NotFound();
            }

            return Ok(courseDto);
        }

        /// <summary>
        /// Updates a specific course.
        /// </summary>
        /// <param name="id">The ID of the course to update.</param>
        /// <param name="course">The Course object containing the updated data.</param>
        /// <returns>No content if the update is successful.</returns>
        // POST: api/CoursesData/UpdateCourse/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseID)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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
        /// Adds a new course.
        /// </summary>
        /// <param name="course">The Course object representing the new course.</param>
        /// <returns>The created course.</returns>
        // POST: api/CoursesData/AddCourse
        [ResponseType(typeof(Course))]
        [HttpPost]
        public IHttpActionResult AddCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.CourseID }, course);
        }

        /// <summary>
        /// Deletes a specific course.
        /// </summary>
        /// <param name="id">The ID of the course to delete.</param>
        /// <returns>The deleted course.</returns>
        // DELETE: api/CoursesData/DeleteCourse/5
        [ResponseType(typeof(Course))]
        [HttpPost]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.courses.Count(e => e.CourseID == id) > 0;
        }
    }
}
