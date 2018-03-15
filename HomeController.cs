using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Threading.Tasks;
using MVCApplication.Models;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using Json.NET.Web;

namespace MVCApplication.Controllers
{
    public class HomeController : Controller
    {
        string Baseurl = "http://localhost:52369/";
        List<Employee> EmpInfo = new List<Employee>();
        public async Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Values/GetAllEmployees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }

        public async Task<ActionResult> GetDetails()
        {
            List<Employee> EmpInfo = new List<Employee>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Values/GetSelectedEmployees");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection formCollection)
        {
            Employee emp = new Employee();
            emp.Name = formCollection["Name"];
            emp.Mobile = Convert.ToInt32(formCollection["Mobile"]);
            emp.Email = formCollection["Email"];
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                        client.DefaultRequestHeaders.Clear();
                        //Define request data format  
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var responseMessage = await client.PostAsJsonAsync("api/Values/Post", emp);
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Error");
                    }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(string.Format("api/Values/Get/{0}", id)).Result;
                Employee employee = response.Content.ReadAsAsync<Employee>().Result;
                if (employee == null)
                {
                    return HttpNotFound();
                }
                return View(employee);
                //ActionResult x = await Index();
                //Employee emp = EmpInfo.Single(em => em.Id == id);
                //return View(emp);
            }
        }

        [HttpPost]
        public ActionResult Edit(int id,FormCollection formCollection)
        {
            Employee emp = new Employee();
            emp.Id = Convert.ToInt32(formCollection["Id"]);
            emp.Name = formCollection["Name"];
            emp.Mobile = Convert.ToInt32(formCollection["Mobile"]);
            emp.Email = formCollection["Email"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var mediaType = new MediaTypeHeaderValue("application/json");
                //var jsonSerializerSettings = new JsonSerializerSettings();
                //var jsonFormatter = new JsonNetFormatter(jsonSerializerSettings);
                //HttpRequestMessage requestMessage = new HttpRequestMessage<T>(emp, mediaType, new MediaTypeFormatter[] { jsonFormatter });
                //var responseMessage = await client.PutAsJsonAsync("api/Values/EditDetail", emp);
                HttpResponseMessage responseMessage =  client.PostAsJsonAsync("api/Values/EditDetail/"+id,emp).Result;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Error");
            }
        }

        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(string.Format("api/Values/Get/{0}", id)).Result;
                Employee employee = response.Content.ReadAsAsync<Employee>().Result;
                if (employee == null)
                {
                    return HttpNotFound();
                }
                //return View(employee);
                HttpResponseMessage responseMessage = client.DeleteAsync("api/Values/Delete/"+ id).Result;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Error");
            }
        }

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteById(int id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(Baseurl);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = client.GetAsync(string.Format("api/Values/Get/{0}", id)).Result;
        //        Employee employee = response.Content.ReadAsAsync<Employee>().Result;
        //        if (employee == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        HttpResponseMessage responseMessage = client.DeleteAsync("api/Values/Delete/"+ id).Result;
        //        responseMessage.EnsureSuccessStatusCode();
        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return RedirectToAction("Error");
        //    }
        //}

        public async Task<ActionResult> About()
        {

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Rock/GetRockDetails");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    //EmpInfo = JsonConvert.DeserializeObject<List<Employee>>(EmpResponse);

                }
                //returning the employee list to view  
                return View();
            }
        }

        //public ActionResult About()
        //{

        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}