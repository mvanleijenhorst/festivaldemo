using System;
using System.Collections.Generic;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets.Messages
{
    public static class CloseFestivalCommandSerializer 
    {
        public static CloseFestivalCommand Deserialize()
        {
            return new CloseFestivalCommand();
        }

        public static byte[] Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.CloseFestival));
            return bytes.ToArray();
        }
    }
}
