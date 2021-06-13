namespace FestivalDemo.WebServer.Domain.Models.Positions
{
    public record Longitude(float Value)
    {
        public static  Longitude Zero => new Longitude(0);

        public static Longitude operator +(Longitude longitudeA, Longitude longitudeB)
        {
            return new Longitude(longitudeA.Value + longitudeB.Value);
        }
    }
}
