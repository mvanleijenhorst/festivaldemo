namespace Communications.Messages
{
    public class BuildingInfoCommand
    {
        public BuildingInfoCommand(int buildingId, int buildingType, float longitude, float latitude)
        {
            BuildingId = buildingId;
            BuildingType = buildingType;
            Longitude = longitude;
            Latitude = latitude;
        }

        public int BuildingId { get; }
        public int BuildingType { get; }
        public float Longitude { get; }
        public float Latitude { get; }
    }
}
