using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class ServerUser
    {
        public int Id { get;set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string SchoolInfo { get; set; }
        public string Fio { get; set; }
        public string TeacherFio { get; set; }
        public bool IsAdmin { get; set; }
    }
}
