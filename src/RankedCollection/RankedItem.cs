namespace EnhancedCollections;

public interface IRankedItem<T>
{
    T Item { get; }
    int Rank { get; set; }

    void Promote();
    void Demote();
}

internal class RankedItem<T> : IRankedItem<T> where T : notnull
{
    public T Item { get; }

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
                if(newRank.HasValue)
                    _rank = newRank.Value;
            }
        }
    }

    public event RankChangedEvent? RankChanged;
    public delegate int RankChangedEvent(IRankedItem<T> item, int desiredRank);

    public RankedItem(T item, int rank)
    {
        Item = item;
        _rank = rank;
    }

    /// <summary>
    /// Use to cascade rank shifts through list without firing infinite events
    /// </summary>
    /// <param name="i"></param>
    public void SetRank(int i) => _rank = i;

    public void Promote() => Rank--;

    public void Demote() => Rank++;


    public static implicit operator T(RankedItem<T> rankedItem) => rankedItem.Item;
}
