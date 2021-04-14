using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectMapper.Projection
{
    public class GenericSource<T>
    {
        public T Value { get; set; }
    }

    public class GenericDestination<T>
    {
        public T Value { get; set; }
    }
}
