using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces.Server
{
    public interface IServerMaintanceService
    {
        void Start();
        void Stop();
        void CleanupResources();
    }
}
