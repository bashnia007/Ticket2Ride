namespace Ticket2Ride.Enums
{
    public enum ActionType
    {
        /// <summary>
        /// Построить перегон
        /// </summary>
        BuildConnection,
        /// <summary>
        /// Построить станцию
        /// </summary>
        BuildStation,
        /// <summary>
        /// Взять карты
        /// </summary>
        GetCards,
        /// <summary>
        /// Взять маршруты
        /// </summary>
        GetRoutes
    }
}
