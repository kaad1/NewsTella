namespace NewsTella.Models.Database
{
	public class PaymentDetail
	{
		public int Id { get; set; }
		public string CardType { get; set; }
		public string NameOnCard { get; set; }
		public string CardNumber { get; set; }
		public string ExpireDate { get; set; }
		public string CardSecurityCode { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string UserId { get; set; }
		public bool IsDeleted { get; set; } = false;
	}
}

