using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ButaGroupTask.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string University { get; set; }
        public string Specialty { get; set; }
        public string Year { get; set; }
        public bool IsDeactive { get; set; }
        public Gender Gender { get; set; }
        public int GenderId { get; set; }
        public Degree Degree { get; set; }
        public int DegreeId { get; set; }
    }
}
