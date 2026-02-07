using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces.Server
{
    public interface IParkingService
    {
        void ProcessZauzece(Zauzece zauzece, TcpClient client);
        void ProcessOslobadjanje(string message, TcpClient client);
    }
}
