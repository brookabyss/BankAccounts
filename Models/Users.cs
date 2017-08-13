using System;
using System.Collections.Generic;
namespace BankAccounts.Models
{
    public class Users: BaseEntity
    {
        public int UsersId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

        public List<JointAccounts> JointAccounts {get;set;}

        public Users()
        {
            JointAccounts = new List<JointAccounts> ();
        }

       
    }
}