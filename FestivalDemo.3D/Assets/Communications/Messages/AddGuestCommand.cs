namespace Communications.Messages
{
    public class AddGuestCommand
    {
        public AddGuestCommand(int guestId, bool isFollower)
        {
            GuestId = guestId;
            IsFollower = isFollower;
        }

        public int GuestId { get; }
        public bool IsFollower { get; }
    }
}
