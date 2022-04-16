namespace EnhancedCollections;

public class RankedItem
{
    int _rank;
    public int Rank
    {
        get => _rank;
        set
        {
            if (value != _rank)
            {
                //TODO: normalize entered rank if out of bounds
                int? newRank = RankChanged?.Invoke(this, value);
                if (newRank.HasValue)
                    _rank = newRank.Value;
            }
        }
    }

    public void Promote() => Rank--;
    public void Demote() => Rank++;

    internal event RankChangedEvent? RankChanged;
    internal delegate int RankChangedEvent(RankedItem item, int desiredRank);

    /// <summary>
    /// Use to cascade rank shifts through list without firing infinite events
    /// </summary>
    /// <param name="i"></param>
    internal void SetRank(int i) => _rank = i;
}

public class RankedItem<T> : RankedItem
{
    public T Item { get; }

    internal RankedItem(T item)
    {
        Item = item;
    }

    public static implicit operator T(RankedItem<T> rankedItem) => rankedItem.Item;
    public static implicit operator RankedItem<T>(T item) => new RankedItem<T>(item);
}
