using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XendBuySellEscrow.Models
{
    public class PaymentDetails
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string ExternalId { get; set; }
        public Payer Payer { get; set; }
        public string PayerMessage { get; set; }
        public string PayeeNote { get; set; }
    }

    public class Payer
    {
        public string PartyIdType { get; set; }
        public string PartyId { get; set; }
    }


}
