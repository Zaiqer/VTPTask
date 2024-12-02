using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ButaGroupTask.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Duration { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeactive { get; set; }

    }
}
