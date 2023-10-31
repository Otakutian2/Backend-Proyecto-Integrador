using AspNetCore.Reporting;
using proyecto_backend.Dto;
using proyecto_backend.Interfaces;
using proyecto_backend.Models;
using proyecto_backend.Utils;

namespace proyecto_backend.Services
{
    public class ReportService : IReport
    {
        public async Task<byte[]> ReportReceipt(Receipt receipt)
        {
            LocalReport report = ReportUtils.GetReport("ReceiptReport");

            List<PurchaseInformation> purchaseInformation = new();
            purchaseInformation.Add(new PurchaseInformation { PaymentMethod = receipt.ReceiptDetailsCollection.First().PaymentMethod.Name, CreatedAt = receipt.CreatedAt });

            List<OrderDetail> orderDetails = new();

            receipt.Command.CommandDetailsCollection.ForEach(dc =>
            {
                orderDetails.Add(new OrderDetail
                {
                    Quantity = dc.DishQuantity,
                    Amount = dc.OrderPrice,
                    Price = dc.DishPrice,
                    Description = dc.Dish.Name
                });
            });

            report.AddDataSource("PurchaseInformation", purchaseInformation);
            report.AddDataSource("OrderDetail", orderDetails);

            Dictionary<string, string> parameters = new();
            parameters.Add("NameEstablishment", receipt.Cash.Establishment.Name);
            parameters.Add("AddressEstablishment", receipt.Cash.Establishment.Address);
            parameters.Add("PhoneEstablishment", receipt.Cash.Establishment.Phone);
            parameters.Add("RucEstablishment", receipt.Cash.Establishment.Ruc);
            parameters.Add("ReceiptId", receipt.Id.ToString());
            parameters.Add("NameCustomer", receipt.Customer.FirstName + " " + receipt.Customer.LastName);
            parameters.Add("DniCustomer", receipt.Customer.Dni);
            parameters.Add("NameWaiter", receipt.Command.Employee.FirstName + " " + receipt.Command.Employee.LastName);
            parameters.Add("NameCashier", receipt.Employee.FirstName + " " + receipt.Employee.LastName);
            parameters.Add("NumberCash", receipt.CashId.ToString());
            parameters.Add("SubTotal", receipt.Command.TotalOrderPrice.ToString());
            parameters.Add("Igv", receipt.Igv.ToString());
            parameters.Add("TotalDiscount", receipt.Discount.ToString());
            parameters.Add("AdditionalAmount", receipt.AdditionalAmount.ToString());
            parameters.Add("Total", receipt.TotalPrice.ToString());

            var result = report.Execute(RenderType.Pdf, 1, parameters);

            return await Task.FromResult(result.MainStream.ToArray());
        }
    }
}
