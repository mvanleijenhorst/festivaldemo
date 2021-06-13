using System;
using System.Collections.Generic;
using FestivalDemo.WebServer.DomainServices.Commands.Messages;

namespace FestivalDemo.WebServer.Infrastructure.WebSockets.Messages
{
    public static class BuildingInfoCommandSerializer
    {
        public static BuildingInfoCommand Deserialize(byte[] bytes)
        {
            var position = sizeof(int);
            var buildingId = BitConverter.ToInt32(bytes, position);

            position += sizeof(int);
            var buildingType = BitConverter.ToInt32(bytes, position);

            position += sizeof(int);
            var logitude = BitConverter.ToSingle(bytes, position);

            position += sizeof(float);
            var latitude = BitConverter.ToSingle(bytes, position);

            return new BuildingInfoCommand(buildingId, buildingType, logitude, latitude);
        }

        public static byte[] Serialize(BuildingInfoCommand command)
        {
            var bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((int)WebSocketCommandType.BuildingInfo));
            bytes.AddRange(BitConverter.GetBytes(command.BuildingId));
            bytes.AddRange(BitConverter.GetBytes(command.BuildingType));
            bytes.AddRange(BitConverter.GetBytes(command.Longitude));
            bytes.AddRange(BitConverter.GetBytes(command.Latitude));
            return bytes.ToArray();
        }
    }
}
