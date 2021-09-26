using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeProj.Model
{
    public class DepartmentModel
    {
        [Key]
        public string DEPARTMENT_ID { get; set; }
        public string CODE { get; set; }
        public string DESCRIPTION { get; set; }
        


    }
}
