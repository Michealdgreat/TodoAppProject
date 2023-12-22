using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.Models
{
    public class TodoModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int AssignedTo { get; set; }
        public bool IsCompleted { get; set;}
    }
}
