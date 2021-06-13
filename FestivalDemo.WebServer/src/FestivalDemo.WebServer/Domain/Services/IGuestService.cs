namespace FestivalDemo.WebServer.Domain.Services
{
    public interface IGuestService
    {
        void MoveGuest(int guestId, float longitude, float latitude);
    }
}
