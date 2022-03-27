using System.Collections;

namespace EnhancedCollections;

public class RankedCollection<T> : ICollection<T> where T : notnull
{
    readonly List<RankedItem<T>> items = new();

    public int Count => items.Count();

    public bool IsReadOnly => false;

    public IRankedItem<T> this[int i] => items[i];

    public void Add(T item)
    {
        var rankedItem = new RankedItem<T>(item, items.Count + 1);
        rankedItem.RankChanged += RankedItem_RankChanged;
        items.Add(rankedItem);
    }

    public bool Remove(T item)
    {
        var i = items.Find(ri => ri.Item.Equals(item));
        if(i is RankedItem<T> ri)
        {
            ri.RankChanged -= RankedItem_RankChanged;
            items.Remove(i);
            //TODO shift remaining ranks
            return true;
        }
        return false;
    }

    private int RankedItem_RankChanged(IRankedItem<T> item, int desiredRank)
    {
        var displaced = items.Find(ri => ri.Rank == desiredRank);
        if (displaced is null)
            return item.Rank;

        displaced.SetRank(item.Rank);
        return desiredRank;
    }

    public void Clear() => items.Clear();

    public bool Contains(T item) => items.Find(ri => ri.Item.Equals(item)) is not null;

    public void CopyTo(T[] array, int arrayIndex) =>
        items.Select(ri => ri.Item).ToList().CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

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
