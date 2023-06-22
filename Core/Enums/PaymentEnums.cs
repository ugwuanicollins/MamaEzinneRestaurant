using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
        public enum PaymentStatus
        {
            [Description("Approved")]
            Seen = 1,
            [Description("Pending")]
            pending = 2,
            [Description("Sent")]
            Sent = 3,
            [Description("Declined")]
            Declined = 4,

    }

}
