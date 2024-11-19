using FabSandbox.Data.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabSandbox.Data.Models
{
    public class DeviceMessage
    {
        public string Device { get; set; }

        public string Type { get; set; }

        public object Value { get; set; }
    }
}
