using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XendBuySellEscrow.Models
{
    public class PaymentDetails
    {
        public string amount { get; set; }
        public string currency { get; set; }
        public string externalId { get; set; }
        public Payer payer { get; set; }
        public string payerMessage { get; set; }
        public string payeeNote { get; set; }
    }

    public class Payer
    {
        public string partyIdType { get; set; }
        public string partyId { get; set; }
    }

    public class NotificationMessage
    {
        public string notificationMessage { get; set; }
    }


    public class TransferModel
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public int financialTransactionId { get; set; }
        public int externalId { get; set; }
        public Payee payee { get; set; }
        public string status { get; set; }
    }

    public class Payee
    {
        public string partyIdType { get; set; }
        public float partyId { get; set; }
    }


    public class WithdrawalMethod
    {
        public string payeeNote { get; set; }
        public string externalId { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public Payer payer { get; set; }
        public string payerMessage { get; set; }
    }

}
