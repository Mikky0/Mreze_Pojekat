using Common.Interfaces.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Services;

namespace Server
{
    public class Program
    {
        public static IServerMessageService messageService = new ServerMessageService();
        public static IParkingService parkingService = new ParkingService();
        public static IConnectionHandler connHandlerService = new ConnectionHandler();
        public static IServerMaintanceService server = new ServerMaintenanceService();

        public static void Main()
        {
            messageService = new ServerMessageService();
            parkingService = new ParkingService();
            connHandlerService = new ConnectionHandler();
            server = new ServerMaintenanceService();

            server.Start();
            server.Stop();
        }
    }
}
