using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        ResponseWithContent<List<PaymentLayoutDC>> GetPaymentLayout(string firstDate, string endDate);

        [OperationContract]
        Response UpdatePaymentState(PaymentDC payment);

        [OperationContract]
        Response AddPayment(PaymentDC payment);

        [OperationContract]
        ResponseWithContent<List<PaymentDC>> GetPaymentsFromDateRange(string startDate, string endDate);
    }
}
