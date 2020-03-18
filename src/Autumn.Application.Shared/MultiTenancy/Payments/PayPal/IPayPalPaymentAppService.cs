using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.MultiTenancy.Payments.Dto;
using Autumn.MultiTenancy.Payments.PayPal.Dto;

namespace Autumn.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalPaymentId, string paypalPayerId);

        PayPalConfigurationDto GetConfiguration();

        Task CancelPayment(CancelPaymentDto input);
    }
}
