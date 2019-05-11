using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            ViewBag.user = ActiveUser.UserId;
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            ViewBag.user = ActiveUser.UserId;
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
            ViewBag.user = ActiveUser.UserId;
            return View();
        }

        [HttpGet("profit")]
        public IActionResult Profit()
        {
            ViewBag.user = ActiveUser.UserId;
            return View();
        }

        [HttpGet("expense")]
        public IActionResult Expense()
        {
            ViewBag.user = ActiveUser.UserId;
            return View();
        }

        [HttpPost("add-expense")]
        public IActionResult AddExpense(Expense expense)
        {
            if (ActiveUser == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                Expense e = new Expense
                {
                    UserId = ActiveUser.UserId,
                    Title = expense.Title,
                    Description = expense.Description,
                    Cost = expense.Cost
                };
                _context.Expenses.Add(e);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View("Expense");
        }
        
        [HttpPost("add-profit")]
        public IActionResult AddProfit(Profit profit)
        {
            if (ActiveUser == null)
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                Profit p = new Profit
                {
                    UserId = ActiveUser.UserId,
                    Title = profit.Title,
                    Description = profit.Description,
                    Amount = profit.Amount
                };
                _context.Profits.Add(p);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View("Profit");
        }
        
        
        
        
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}