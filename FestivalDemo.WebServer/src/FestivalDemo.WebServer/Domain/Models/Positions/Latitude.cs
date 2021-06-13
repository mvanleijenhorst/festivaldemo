namespace FestivalDemo.WebServer.Domain.Models.Positions
{
    public record Latitude(float Value)
    {
        public static Latitude Zero => new Latitude(0);

        public static Latitude operator +(Latitude latitudeA, Latitude latitudeB)
        {
            return new Latitude(latitudeA.Value + latitudeB.Value);
        }
    }
}
