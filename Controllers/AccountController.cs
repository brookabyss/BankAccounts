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
    public class AccountController : Controller
    {
        private AccountContext _context;
        public AccountController(AccountContext context){
            _context=context;
        }
        
        [HttpGet]
        [Route("accounts/show")]
        public IActionResult Show(){
             ViewBag.Errors=new List<string>();
            // int? Id= HttpContext.Session.GetInt32("UserId");
            // Users currentUser= new Users();
            // currentUser = _context.Users
            //                             .Include(a=>a.JointAccounts)
            //                             .ThenInclude(c=>c.Accounts).SingleOrDefault(user=>user.UsersId==(int)Id);
            // System.Console.WriteLine($"The joint account {currentUser.JointAccounts.Count}");
            // ViewBag.user=currentUser;
                                
            // System.Console.WriteLine($"Success{currentUser.FirstName}");
            List<Users> PopulatedUserAccounts=new List<Users>();
            PopulatedUserAccounts=GetPopulatedUser();
            var accounts= from useraccounts in PopulatedUserAccounts
                            from account in useraccounts.JointAccounts
                            orderby account.Accounts.CreatedAt descending
                            select account;
            
            ViewBag.CurrentBalance=accounts.First();
            ViewBag.user=PopulatedUserAccounts;
            ViewBag.JointAccounts=accounts;

            return View();
        }
        [HttpPost]
        [Route("account/transaction")]
        public IActionResult Transaction(float transact , int AcId,float Balance){
            System.Console.WriteLine($"Line 49, Transaction {transact},{AcId},{Balance}");
            int? Id= HttpContext.Session.GetInt32("UserId");
            ViewBag.Errors=new List<string>();
            if((transact+Balance)>=0){
                // System.Console.WriteLine($"Line 38, ACCCont{transact}{AcId}");
                // Accounts RetrievedAccount = _context.Accounts.SingleOrDefault(account => account.AccountsId == AcId);
                // RetrievedAccount.Transaction=transact;
                // RetrievedAccount.CurrentBalance= Balance + transact;
                // RetrievedAccount.UpdatedAt=DateTime.Now;
                // _context.SaveChanges();
                float total=Balance+(transact);
                Accounts newAccount=new Accounts{
                            CurrentBalance=total,
                            Transaction=(float)transact,
                            CreatedAt=DateTime.Now,
                            UpdatedAt=DateTime.Now,
                        };
                System.Console.WriteLine($"Line 66,Transaction{newAccount.CurrentBalance}");
                _context.Accounts.Add(newAccount);
                _context.SaveChanges();
                Accounts createdAccount=_context.Accounts.Last();
                if(Id!=null){
                            JointAccounts newJoint=new JointAccounts{
                                UsersId=(int)Id,
                                AccountsId=createdAccount.AccountsId
                            };
                            _context.JointAccounts.Add(newJoint);
                            _context.SaveChanges();

                        };
            
                
            }
            else{
                 ViewBag.Errors.Add("Insufficent funds!");
            }
            
            List<Users> PopulatedUserAccounts=new List<Users> ();
           
            PopulatedUserAccounts = GetPopulatedUser();
            
            
            
              var accounts= from useraccounts in PopulatedUserAccounts
                            from account in useraccounts.JointAccounts
                            orderby account.Accounts.CreatedAt descending
                            select account;
            ViewBag.CurrentBalance=accounts.First();
            ViewBag.JointAccounts=accounts;
            ViewBag.user=PopulatedUserAccounts;

            return View("Show");
            

        }

        private List<Users> GetPopulatedUser(){
            int? Id= HttpContext.Session.GetInt32("UserId");
            List<Users> PopulatedUserAccounts=new List<Users> ();
           
            PopulatedUserAccounts = _context.Users
                                    .Include(a=>a.JointAccounts)
                                    .ThenInclude(c=>c.Accounts)
                                    .Where(user=>user.UsersId==(int)Id).ToList();
            
            return PopulatedUserAccounts;
        }

    }
}