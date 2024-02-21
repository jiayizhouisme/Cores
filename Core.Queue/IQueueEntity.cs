using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Queue
{
    public interface IQueueEntity
    {
        public string name { get; }
        public object body { get; }
    }
}
