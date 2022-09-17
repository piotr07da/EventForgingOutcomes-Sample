using EventForging;

namespace EFO.DeliveryAcceptance.Domain
{
    public class ComponentUnloadingBase : IEventForged
    {
        public ComponentUnloadingBase()
        {
            Events = Events.CreateFor(this);
        }

        public Events Events { get; }

        public void DoSomeAction()
        {
            if (true)
            {
                Events.Apply(new object());
            }
        }
    }
}
