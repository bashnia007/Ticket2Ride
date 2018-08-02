using Ticket2Ride.Enums;

namespace Ticket2Ride
{
    public class BuildSet
    {
        public CardColor Color { get; set; }
        public int WagonsCost { get; set; }
        public int TunnelCost { get; set; }
        public int JokersToBuild { get; set; }

        public int WagonsToBuild => WagonsCost + TunnelCost - JokersToBuild;
    }
}
