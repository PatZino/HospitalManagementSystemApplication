using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HMSApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HMSApp.Controllers
{
    public class AccountController : Controller
    {
        public async Task<IActionResult> Index()
        {
                       
                List<User> users = new List<User>();
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://localhost:44330/users"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    }
                }
                return View(users);
            
        }

        public ViewResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            User register = new User();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44330/users/register", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        register = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                }
            }
            return View(register);
        }


        public ViewResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateModel authenticateModel)
        {
            AuthenticateModel authenticate = new AuthenticateModel();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(authenticateModel), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44330/users/authenticate", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        authenticate = JsonConvert.DeserializeObject<AuthenticateModel>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Result = apiResponse;
                        return View();
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }


     



    }
}