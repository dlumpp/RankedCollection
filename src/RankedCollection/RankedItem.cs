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
                int? newRank = RankChanged?.Invoke(this, value);
                if (newRank.HasValue)
                    _rank = newRank.Value;
            }
        }
    }

    public void Promote() => Rank--;
    public void Demote() => Rank++;

    internal event RankChangedEvent? RankChanged;
    internal delegate int? RankChangedEvent(RankedItem changingItem, int? desiredRank);

    /// <summary>
    /// Use to cascade rank shifts through list without firing infinite events
    /// </summary>
    /// <param name="i"></param>
    internal void SetRank(int i) => _rank = i;
}

public class RankedItem<T> : RankedItem where T : notnull
{
    public T Value { get; }

    internal RankedItem(T value)
    {
        Value = value;
    }

    public static implicit operator T(RankedItem<T> rankedItem) => rankedItem.Value;
    public static implicit operator RankedItem<T>(T value) => new (value);
}
