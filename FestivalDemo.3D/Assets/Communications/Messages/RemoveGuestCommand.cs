namespace Communications.Messages
{
    public class RemoveGuestCommand
    {
        public RemoveGuestCommand(int guestId)
        {
            GuestId = guestId;
        }

        public int GuestId { get; }
    }
}
