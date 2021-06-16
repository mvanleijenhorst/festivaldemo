namespace Communications.Messages
{
    public class UpdateGuestCommand
    {
        public UpdateGuestCommand(int guestId, float longitude, float latitude)
        {
            GuestId = guestId;
            Longitude = longitude;
            Latitude = latitude;
        }

        public int GuestId { get; }
        public float Longitude { get; }
        public float Latitude { get; }
    }
}
