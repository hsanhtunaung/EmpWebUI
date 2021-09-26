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
using Model;
using Newtonsoft.Json;


namespace EmployeeProj.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string apiURL = ConfigurationManager.AppSettings["webapiurl"];
        #region Populate
        private static List<SelectListItem> PopulateDepartment()
        {
           BLL.DepartmentBLL bll = new DepartmentBLL();
            List<DepartmentModel> lst = new List<DepartmentModel>();
            lst = bll.GetALL();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (DepartmentModel m in lst)
                items.Add(new SelectListItem
                {
                    Text = m.DESCRIPTION,
                    Value = m.CODE
                });
            return items;
        }

        private static List<SelectListItem> PopulateSkills()
        {
            BLL.EmployeeBLL bll = new EmployeeBLL();
            List<EmployeeRequestModel> lst = new List<EmployeeRequestModel>();
            lst = bll.GetALLSkills();
            List<SelectListItem> skillitems = new List<SelectListItem>();        
            foreach (EmployeeRequestModel s in lst)
               skillitems.Add(new SelectListItem
                {
                    Text = s.Skillmodel.Skills,
                    Value = s.Skillmodel.Skills
               });           
            return skillitems;
        }       
        #endregion

        public ActionResult Index()
        {            
            ViewBag.lstDepartment = PopulateDepartment();          
            ViewBag.lstSkills = PopulateSkills();
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
                model.Skillmodel.Skills = skillset;

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
            return RedirectToAction("Index"); 
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
                //return RedirectToAction("EmployeeList");
                //Storing the response details recieved from web api
                 var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                 lst = JsonConvert.DeserializeObject<List<EmployeeResponseModel>>(EmpResponse);              
            }
            return View(lst);
        }

        public ActionResult Edit()
        {
            return View();
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
                //Passing service base url
                client.BaseAddress = new Uri(apiURL);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = client.PostAsync(apiURL + string.Format("API/Employee/Put"), json).Result;
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("EmployeeList");
                }
            }
                return View();
        }
    }
}