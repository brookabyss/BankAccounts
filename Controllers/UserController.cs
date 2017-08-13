using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BankAccounts.Models;
using System.Linq;
using MySQL.Data.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class UserController : Controller
    {
        private AccountContext _context;
        public UserController(AccountContext context){
            _context=context;
        }

        // private Users GetUsers(){
        //     Users currentUser= new Users();
        //     int? Id= HttpContext.Session.GetInt32("UserId");
        //     currentUser = _context.Users
        //                                 .Include(a=>a.JointAccounts)
        //                                 .ThenInclude(c=>c.Accounts).SingleOrDefault(user=>user.UsersId==(int)Id);
        //     System.Console.WriteLine($"The joint account {currentUser.JointAccounts.Count}");
        //     return currentUser;
        // }
        private List<Users> GetPopulatedUser(){
            int? Id= HttpContext.Session.GetInt32("UserId");
            List<Users> PopulatedUserAccounts=new List<Users> ();
           
            PopulatedUserAccounts = _context.Users
                                    .Include(a=>a.JointAccounts)
                                    .ThenInclude(c=>c.Accounts)
                                    .Where(user=>user.UsersId==(int)Id).ToList();
            
            return PopulatedUserAccounts;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Errors=new List<string>();
            return View();
        }

        [HttpGet]
        [Route("user/registration")]
        public IActionResult ToRegister(UsersView newUser){
            ViewBag.Errors=new List<string>();
            return View("Registration");
        }



       // Register function

        [HttpPost]
        [Route("user/register")]
        public IActionResult Register(Users newUser)
        {
            System.Console.WriteLine(ModelState.IsValid);
            if(ModelState.IsValid){
                    System.Console.WriteLine(newUser.FirstName);
                    Users createdUser= new Users{
                        FirstName=newUser.FirstName,
                        LastName=newUser.LastName,
                        Email=newUser.Email,
                        Password= newUser.Password,
                        CreatedAt= DateTime.Now,
                        UpdatedAt=DateTime.Now,
                    };
                    _context.Users.Add(createdUser);
                    _context.SaveChanges();
                    Users ReturnedUser = _context.Users.SingleOrDefault(user => user.Email == createdUser.Email);
                    System.Console.WriteLine($"Email from returned {ReturnedUser.Email}");
                    HttpContext.Session.SetInt32("UserId",(int)ReturnedUser.UsersId);  
                    // Users populatedUser=GetUsers();
                    List<Users> populatedUsers=GetPopulatedUser();
                    
                    // Create an Account
                     if(populatedUsers[0].JointAccounts.Count==0){
                        Accounts newAccount=new Accounts{
                            CurrentBalance=50,
                            Transaction=50,
                            CreatedAt=DateTime.Now,
                            UpdatedAt=DateTime.Now,
                        };
                        _context.Accounts.Add(newAccount);
                        _context.SaveChanges();
                        Accounts createdAccount=_context.Accounts.Last();
                        int? jId=HttpContext.Session.GetInt32("UserId");
                        System.Console.WriteLine($"JIIIIIIIIIDDDDDDDdd    {jId}");
                        if(jId!=null){
                            JointAccounts newJoint=new JointAccounts{
                                UsersId=(int)jId,
                                AccountsId=createdAccount.AccountsId
                            };
                            _context.JointAccounts.Add(newJoint);
                            _context.SaveChanges();

                        };
                        
                        System.Console.WriteLine($"Created account Id{createdAccount.AccountsId}");

                    }
                    return RedirectToAction("Show","Account");
                    
            }
            else{
                    ViewBag.Errors=ModelState.Values;
                    return View("Registration");
            }
            
        }
        [HttpPost]
        [Route("user/login")]
        public IActionResult Login(string Email, string Password)
        {
               Users ReturnedUser= _context.Users.SingleOrDefault(user=>user.Email==Email);
               if(ReturnedUser.Password==Password){
                    HttpContext.Session.SetInt32("UserId",(int)ReturnedUser.UsersId);
                    // Users populatedUser=GetUsers();
                    // System.Console.WriteLine($"The populated user data with the joint accounts{populatedUser.JointAccounts.Count}");
                    // if(populatedUser.JointAccounts.Count==0){
                    //     Accounts newAccount=new Accounts{
                    //         CurrentBalance=50,
                    //         Transaction=50,
                    //         CreatedAt=DateTime.Now,
                    //         UpdatedAt=DateTime.Now,
                    //     };
                    //     _context.Accounts.Add(newAccount);
                    //     _context.SaveChanges();


                    // }
                    return RedirectToAction("Show","Account");
               }
               else{
                   System.Console.WriteLine("Say Wahts&&&&&&&");
                   List<string> errors=new List<string>();
                   errors.Add("Email or password incorrect");
                   ViewBag.Errors=errors;
               }
            return View("Index");
        }









        [HttpGet]
        [Route("user/logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
