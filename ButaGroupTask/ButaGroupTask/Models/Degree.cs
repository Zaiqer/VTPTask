using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButaGroupTask.Models
{
    public class Degree
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; set; }
    }
}
