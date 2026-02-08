using Common.DTO;
using Common.Interfaces.Server;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class ParkingService : IParkingService
    {
        static IServerMessageService messageService = Program.messageService;

        public void ProcessOslobadjanje(string message, TcpClient client)
        {

        }



        public void ProcessZauzece(Zauzece zauzece, TcpClient client)
        {

        }
    }
}
