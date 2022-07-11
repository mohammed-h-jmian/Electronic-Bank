namespace Electronic_Bank.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public int WalletID { get; set; }  
        public double TransactionAmount { get; set; }
        public String TransactionDescription { get; set; }
        public DateTime TransactionDate { get; set; }
        public String TransactionType { get; set; }


        public virtual Wallet Wallets { get; set; }




    }
}
