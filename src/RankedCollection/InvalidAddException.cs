namespace EnhancedCollections
{
    public class InvalidAddException<T> : Exception where T : notnull
    {
        public InvalidAddException(RankedItem<T> item, string message) : base(message)
        {
            InvalidItem = item;
        }

        public RankedItem<T> InvalidItem { get; }
    }
}
