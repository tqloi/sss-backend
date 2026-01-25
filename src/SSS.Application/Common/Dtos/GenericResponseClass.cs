using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Common.Dtos
{
    public abstract class GenericResponseClass<T>
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
