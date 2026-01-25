using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Common.Dtos
{
    // Version không có data (cho các response chỉ cần Success/Message)
    public abstract record GenericResponseRecord(
        bool Success,
        string Message
    );

    // Version có data
    public abstract record GenericResponseRecord<T>(
        bool Success,
        string Message,
        T? Data = default
    );
}
