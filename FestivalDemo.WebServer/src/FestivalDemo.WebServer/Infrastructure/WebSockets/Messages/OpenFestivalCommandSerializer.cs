using System;
using System.Collections.Generic;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets.Messages
{
    public static class OpenFestivalCommandSerializer 
    {
        public static OpenFestivalCommand Deserialize()
        {
            return new OpenFestivalCommand();
        }

        public static byte[] Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.OpenFestival));
            return bytes.ToArray();
        }
    }
}
