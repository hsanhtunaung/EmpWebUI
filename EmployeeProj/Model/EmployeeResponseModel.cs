using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeProj.Model
{
    public class EmployeeResponseModel
    {
        [Key]
        public int EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_NO { get; set; }

        public DateTime DOB { get; set; }
        public DateTime JOIN_DATE { get; set; }
        public string DEPARTMENT { get; set; }
        public decimal SALARY { get; set; }
        public string SKILLS { get; set; }

    }
}
