﻿using System.Collections;

namespace EnhancedCollections;

public class RankedCollection<T> : ICollection<RankedItem<T>> where T : notnull
{
    readonly List<RankedItem<T>> items = new();

    public int Count => items.Count();

    public bool IsReadOnly => false;

    public RankedItem<T> this[int i] => items[i];

    public void Add(RankedItem<T> item)
    {
        item.SetRank(items.Count + 1);
        item.RankChanged += RankedItem_RankChanged;
        items.Add(item);
    }

    public bool Remove(RankedItem<T> item)
    {
        var i = items.Find(ri => ri.Value.Equals(item.Value));
        if (i is RankedItem<T> ri)
        {
            ri.RankChanged -= RankedItem_RankChanged;
            items.Remove(i);
            //TODO shift remaining ranks
            return true;
        }
        return false;
    }

    private int RankedItem_RankChanged(RankedItem item, int desiredRank)
    {
        if (desiredRank < 1)
            desiredRank = 1;

        if (desiredRank > Count)
            desiredRank = Count;

        bool promoting = desiredRank < item.Rank;
        bool demoting = desiredRank > item.Rank;

        foreach (var ri in items)
        {
            if (ri.Rank == item.Rank)
            {
                continue;
            }
            if (promoting && ri.Rank < item.Rank)
            {
                ri.SetRank(ri.Rank + 1);
            }
            if (demoting && ri.Rank > item.Rank)
            {
                ri.SetRank(ri.Rank - 1);
            }
        }
        return desiredRank;
    }

    public void Clear() => items.Clear();

    public bool Contains(RankedItem<T> item) => items.Find(ri => ri.Value.Equals(item.Value)) is not null;

    public void CopyTo(RankedItem<T>[] array, int arrayIndex) =>
        items.CopyTo(array, arrayIndex);

    public IEnumerator<RankedItem<T>> GetEnumerator() => items.OrderBy(i => i.Rank).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
