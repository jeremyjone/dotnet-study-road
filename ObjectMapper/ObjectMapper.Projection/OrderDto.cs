using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Projection
{
    public class OrderDto
    {
        public string CustomerName { get; set; }
        public decimal Total { get; set; }
    }
}
