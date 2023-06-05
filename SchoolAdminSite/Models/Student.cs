using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolAdminSite.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [Required]
        //Student ID
        public string Fname { get; set; }
        //Student First name
        public string Lname { get; set; }
        //Studnet Last name
        public int Grade { get; set; }
        //Student Grade (e.g: 9, 10, 11, 12)
    }
}