using System;
using System.Collections.Generic;
namespace BankAccounts.Models
{
    public class Accounts: BaseEntity
    {
        public int AccountsId {get;set;}
        public float CurrentBalance {get;set;}
        public float Transaction {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        
        public List<JointAccounts>  JointAccounts {get;set;}


        public Accounts()
        {
            JointAccounts = new List<JointAccounts> ();
        }
       
    }
}