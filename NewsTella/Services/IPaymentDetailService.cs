using NewsTella.Models.Database;

namespace NewsTella.Services
{
	public interface IPaymentDetailService
	{
		List<PaymentDetail> GetPaymentDetails();

		PaymentDetail GetPaymentDetailById(int id);

		void AddPaymentDetail(PaymentDetail paymentDetail);

		void RemovePaymentDetail(PaymentDetail paymentDetail);

		void UpdatePaymentDetail(PaymentDetail paymentDetail);
	}
}
