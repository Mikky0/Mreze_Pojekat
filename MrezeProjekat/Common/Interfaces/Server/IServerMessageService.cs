using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces.Server
{
    public interface IServerMessageService
    {
        void SendParkingStatus(TcpClient client);
        void ProcessClientMessage(TcpClient client);
        void SendMessage(TcpClient client, string message);
    }
}
