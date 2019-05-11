using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FinDash.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;

namespace FinDash.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinancialContext _context;
        public HomeController(FinancialContext context)
        {
            _context = context;
        }
        private User ActiveUser 
        {
            get 
            {
                return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            }
        }
        [HttpGet("")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("registeruser")]
        public IActionResult RegisterUser(RegisterUser newuser)
        {
            User CheckEmail = _context.Users
                .SingleOrDefault(u => u.Email == newuser.Email);

            if(CheckEmail != null)
            {
                ViewBag.errors = "That email already exists";
                return RedirectToAction("Register","Home");
            }
            if(ModelState.IsValid)
            {
                PasswordHasher<RegisterUser> Hasher = new PasswordHasher<RegisterUser>();
                User newUser = new User
                {
                    UserId = newuser.UserId,
                    FirstName = newuser.FirstName,
                    LastName = newuser.LastName,
                    Email = newuser.Email,
                    Password = Hasher.HashPassword(newuser, newuser.Password)
                  };
                _context.Add(newUser);
                _context.SaveChanges();
                ViewBag.success = "Successfully registered";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                return View("Register");
            }
        }

        [HttpPost("loginuser")]
        public IActionResult LoginUser(LoginUser loginUser) 
        {
            User CheckEmail = _context.Users
                .SingleOrDefault(u => u.Email == loginUser.Email);
            if(CheckEmail != null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(CheckEmail, CheckEmail.Password, loginUser.Password))
                {
                    HttpContext.Session.SetInt32("UserId", CheckEmail.UserId);
                    HttpContext.Session.SetString("FirstName", CheckEmail.FirstName);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ViewBag.errors = "Incorrect Password";
                    return View("Register");
                }
            }
            else
            {
                ViewBag.errors = "Email not registered";
                return View("Register");
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}