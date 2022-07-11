namespace Electronic_Bank.Models
{
    public class Client
    {
        public int ClientID { get; set; }
        public String ClientName { get; set; }
        public String ClientAddress  { get; set; }
        public int ClientPhone  { get; set; }
		public String ClientImg { get; set; }

		public virtual ICollection<Wallet> ClientWallets { get; set; }

       


    }
}
