
namespace Flogging.Core
{
    using System;
    using System.Collections.Generic;

    public class FlogDetail
    {
        public FlogDetail()
        {
            Timestamp = DateTime.Now;
        }

        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        // WHERE
        public string Product { get; set; }
        public string Layer { get; set; }
        public string Location { get; set; }
        public string Hostname { get; set; }

        // WHO
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }

        // EVERYTHING ELSE
        public long? ElapsedMilliseconds { get; set; } // only for Perofmrance Enteries
        public Exception Exception { get; set; }    // the exception for error logging
        public string CorrelationId { get; set; }   // exception shielding from server to client
        public Dictionary<string,object> AdditionalInfo { get; set; } // catch all for anything else

    }
}
