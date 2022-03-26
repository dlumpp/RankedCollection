using System.Collections;

namespace EnhancedCollections;

public class RankedCollection<T> : ICollection<T> where T : notnull
{
    //at least have to implement IEnumerable to override w rank order
    readonly List<RankedItem<T>> items = new();

    public int Count => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public IRankedItem<T> this[int i] => items[i];

    public void Add(T item)
    {
        var rankedItem = new RankedItem<T>(item, items.Count + 1);
        rankedItem.RankChanged += RankedItem_RankChanged;
        items.Add(rankedItem);
    }

    public void Remove(T item)
    {
        var i = items.Find(ri => ri.Item.Equals(item));
        if(i is RankedItem<T> ri)
        {
            ri.RankChanged -= RankedItem_RankChanged;
            items.Remove(i);
            //TODO shift remaining ranks
        }
    }

    bool ICollection<T>.Remove(T item)
    {
        throw new NotImplementedException();
    }

    private int RankedItem_RankChanged(IRankedItem<T> item, int desiredRank)
    {
        var displaced = items.Find(ri => ri.Rank == desiredRank);
        if (displaced is null)
            return item.Rank;

        displaced.SetRank(item.Rank);
        return desiredRank;
    }

    public List<T> ToList()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(T item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }


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

/*
 *  shiftUp(item: IRankedContestant): void {
        this.shiftContestant(item, -1);
    }

    shiftDown(item: IRankedContestant): void {
        this.shiftContestant(item, 1);
    }

    private shiftContestant(item: IRankedContestant, offset: number): void {
        let newRank: number = item.rank + offset;
        let ubound: number = this.findMaxRank();
        if (newRank >= 0 && newRank <= ubound) {

            let displaced = this.rankings.find(c => c.rank == newRank);
            displaced.rank = item.rank;
            item.rank = newRank;

            this.rankings.$save(displaced);
            this.rankings.$save(item);
        }
    }

    private findMaxRank(): number {
        const ranks: number[] = this.rankings.map(r => <number>r.rank);
        return Math.max(...ranks);
    }
 */
