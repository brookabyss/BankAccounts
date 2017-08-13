namespace BankAccounts.Models
{
    public class JointAccounts: BaseEntity
    {
        public int JointAccountsId {get;set;}

        public int UsersId {get;set;}
        public Users Users {get;set;}

        public int AccountsId {get;set;}
        public Accounts Accounts {get;set;}
        
    }
}