using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeProj.Model
{
    public class EmployeeRequestModel
    {
        [Key]
        public int EMPLOYEE_ID { get; set; }
        [Required(ErrorMessage = "Name is Required")]
        public string EMPLOYEE_NAME { get; set; }
        [Required(ErrorMessage = "EMPLOYEE NO is Required")]
        public string EMPLOYEE_NO { get; set; }
               
        public DateTime DOB { get; set; }
        public DateTime JOIN_DATE { get; set; }
        public string DEPARTMENT { get; set; }
        public decimal SALARY { get; set; }
        public string SKILLS { get; set; }


        public DepartmentModel Departmentmodel { get => _departmentmodel; set => _departmentmodel = value; }

        public SkillModel Skillmodel { get => _skillmodel; set => _skillmodel = value; }

        private DepartmentModel _departmentmodel;

        private SkillModel _skillmodel;

        public EmployeeRequestModel()
        {
            Departmentmodel = new DepartmentModel();
            Skillmodel = new SkillModel();
        }


    }
}
