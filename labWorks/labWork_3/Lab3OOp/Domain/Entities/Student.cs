using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.Domain.Entities
{

    public class Student
    {
        public static int NextId { get; set; } = 1;
        public int Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public Student()
        {
            Id = NextId++;
        }

    }
}
