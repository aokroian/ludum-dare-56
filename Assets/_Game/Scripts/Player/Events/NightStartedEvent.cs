namespace Player.Events
{
    public class NightStartedEvent
    {
        public int Night { get; private set; }
        
        public NightStartedEvent(int night)
        {
            Night = night;
        }
    }
}