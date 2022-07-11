namespace Electronic_Bank.Models
{
    public class Wallet
    {
        public int WalletID { get; set; }
        public double WalletAmount { get; set; }
        public int ClientID { get; set; }
        public int CurrencyID { get; set; }

        public virtual ICollection<Transaction> WalletTransactions { get; set; }
        public virtual Client Clients { get; set; }
        public virtual Currency Currencys { get; set; }


    }
}
