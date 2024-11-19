using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSandbox.Data.Models
{
    public class MessageLog
    {
        public string DeviceKey { get; set; }
        public DeviceMessage Message { get; set; }
    }
}
