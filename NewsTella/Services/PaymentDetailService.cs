using NewsTella.Data;
using NewsTella.Models.Database;

namespace NewsTella.Services
{
	public class PaymentDetailService:IPaymentDetailService
	{
		private readonly AppDbContext _db;

		public PaymentDetailService(AppDbContext db)
		{
			_db = db;
		}
		public List<PaymentDetail> GetPaymentDetails()
		{
			var obj = _db.PaymentDetails.ToList();
			return obj;
		}
		public PaymentDetail GetPaymentDetailById(int id)
		{
			var paymentDetail = _db.PaymentDetails.FirstOrDefault(p => p.Id == id);
			return paymentDetail;
		}

		public void AddPaymentDetail(PaymentDetail paymentDetail)
		{
			_db.PaymentDetails.Add(paymentDetail);
			_db.SaveChanges();
		}

		public void RemovePaymentDetail(PaymentDetail paymentDetail)
		{
			_db.PaymentDetails.Remove(paymentDetail);
			_db.SaveChanges();
		}

		public void UpdatePaymentDetail(PaymentDetail paymentDetail)
		{
			_db.PaymentDetails.Update(paymentDetail);
			_db.SaveChanges();
		}
	}
}
