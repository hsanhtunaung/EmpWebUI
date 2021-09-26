using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BLL;
using EmployeeProj.Data;
using EmployeeProj.Model;
using Newtonsoft.Json;


namespace EmployeeProj.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string apiURL = ConfigurationManager.AppSettings["webapiurl"];
        private DBEntities _db = new DBEntities();
        #region Populate
        private  List<SelectListItem> PopulateDepartment()
        {                   
            var lst=  (from s in _db.DEPARTMENT select s).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (DepartmentModel m in lst)
                items.Add(new SelectListItem
                {
                    Text = m.DESCRIPTION,
                    Value = m.CODE
                });
            return items;
        }

        private  List<SelectListItem> PopulateSkills()
        {
            
            var lst = (from s in _db.skills select s).ToList();
            List<SelectListItem> skillitems = new List<SelectListItem>();
            EmployeeRequestModel m = new EmployeeRequestModel();
            foreach (SkillModel s in lst)
                skillitems.Add(new SelectListItem
                {
                    Text = s.Skills,
                    Value = s.Skills
                });
            return skillitems;
        }
        #endregion

        public ActionResult Index()
        {
            //ViewBag.lstDepartment = PopulateDepartment();
            //ViewBag.lstSkills = PopulateSkills();
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.lstDepartment = PopulateDepartment();
            ViewBag.lstSkills = PopulateSkills();
            return View();
        }


        [HttpPost]
        public ActionResult Create(FormCollection form, EmployeeRequestModel model)
        {
            if (ModelState.IsValid)
            {
                EmployeeResponseModel empresponse = new EmployeeResponseModel();
                List<EmployeeResponseModel> lst = new List<EmployeeResponseModel>();
                string skillset = form["skill"];
                //model.Skillmodel.Skills = skillset;
                model.SKILLS = skillset;
                HttpClient client = new HttpClient();
                var json = new StringContent(new JavaScriptSerializer().Serialize(model).ToString());
                json.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Passing service base url
                client.BaseAddress = new Uri(apiURL);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = client.PostAsync(apiURL + string.Format("API/Employee/Post"), json).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("EmployeeList");
                }
               // return RedirectToAction("EmployeeList");
            }
            return RedirectToAction("Create"); 
        }


        public ActionResult EmployeeList()
        {
           

            List<EmployeeResponseModel> lst = new List<EmployeeResponseModel>();          
            HttpClient client = new HttpClient();      
            //Passing service base url
            client.BaseAddress = new Uri(apiURL);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = client.GetAsync(apiURL + string.Format("API/Employee/Get")).Result;
            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {            
              
                 var EmpResponse = Res.Content.ReadAsStringAsync().Result;                
                 lst = JsonConvert.DeserializeObject<List<EmployeeResponseModel>>(EmpResponse);              
            }
            return View(lst);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.lstDepartment = PopulateDepartment();
            ViewBag.lstSkills = PopulateSkills();

            EmployeeResponseModel model = new EmployeeResponseModel();
            HttpClient client = new HttpClient();          
            client.BaseAddress = new Uri(apiURL);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            HttpResponseMessage Res = client.GetAsync(apiURL + string.Format("API/Employee/Get"+"?id="+Convert.ToInt32(id))).Result;
            
            if (Res.IsSuccessStatusCode)
            {                
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;                
                model = JsonConvert.DeserializeObject<EmployeeResponseModel>(EmpResponse);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection form, EmployeeRequestModel model)
        {
            if (ModelState.IsValid)
            {
                EmployeeResponseModel empresponse = new EmployeeResponseModel();           
                string skillset = form["skill"];
                model.Skillmodel.Skills = skillset;

                HttpClient client = new HttpClient();
                var json = new StringContent(new JavaScriptSerializer().Serialize(model).ToString());
                json.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.BaseAddress = new Uri(apiURL);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                HttpResponseMessage Res = client.PostAsync(apiURL + string.Format("API/Employee/Put"), json).Result;
                
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("EmployeeList");
                }
            }
                return View();
        }


        public ActionResult Delete(string id)
        {
            HttpClient client = new HttpClient();            
            client.BaseAddress = new Uri(apiURL);
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
            HttpResponseMessage Res = client.DeleteAsync(apiURL + string.Format("API/Employee/Delete" + "?id="+ Convert.ToInt32(id))).Result;
         
            if (Res.IsSuccessStatusCode)
            {
                return RedirectToAction("EmployeeList");
         
            }
            return RedirectToAction("EmployeeList");
        }
        }
}