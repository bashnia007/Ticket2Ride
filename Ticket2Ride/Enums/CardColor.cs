using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket2Ride.Enums
{
    public enum CardColor
    {
        [Description("Серый")]
        Common = -2,
        [Description("Паровоз")]
        Joker = -1,
        [Description("Синий")]
        Blue,
        [Description("Красный")]
        Red,
        [Description("Зеленый")]
        Green,
        [Description("Рыжий")]
        Orange,
        [Description("Черный")]
        Black,
        [Description("Васильковый")]
        Pink,
        [Description("Белый")]
        White,
        [Description("Желтый")]
        Yellow
    }
}
