namespace FestivalDemo.WebServer.Domain.Models.Positions
{
    public record Coordinate(Longitude Longitude, Latitude Latitude)
    {
        public static Coordinate PointZero => 
            new Coordinate(Longitude.Zero, Latitude.Zero);

        public static Coordinate Create(float longitude, float latitude) => 
            new Coordinate(new Longitude(longitude), new Latitude(latitude));
        
        public static Coordinate operator +(Coordinate coordinateA, Coordinate coordinateB)
        {
            return new Coordinate(coordinateA.Longitude + coordinateB.Longitude, coordinateA.Latitude + coordinateB.Latitude);
        }
    }
}
