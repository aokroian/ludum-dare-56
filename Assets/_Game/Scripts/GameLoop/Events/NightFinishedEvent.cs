namespace GameLoop.Events
{
    public class NightFinishedEvent
    {
        public int Night { get; private set; }
        
        public NightFinishedEvent(int night)
        {
            Night = night;
        }
    }
}