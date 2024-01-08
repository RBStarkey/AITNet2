using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AitNet2.Models
{
    public class RawResult<T>
    {
        public int Status { get; set; }
        public T Result { get; set; }
    }
}
