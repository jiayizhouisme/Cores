using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utill.UniqueCode
{
    public interface IUniqueCodeGenerater<T>
    {
        Task<T> Generate(string key);
    }
}
