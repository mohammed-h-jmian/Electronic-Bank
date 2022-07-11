using Electronic_Bank.Models;

namespace Electronic_Bank.ViewModel
{
	public class ClientWallets
	{
		public virtual Client GetClient { get; set; }
		public  Currency GetCurrency { get; set; }
		public virtual Wallet GetWallet { get; set; }
	}
}
