using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    namespace Common.DTOs
    {
        public class GlobalDataObject
        {
            public List<object> Bookings { get; set; } = new();
            public List<object> Posts { get; set; } = new();
            public List<object> Certificates { get; set; } = new();
            public List<object> Guarantees { get; set; } = new();
        }
    }

}
