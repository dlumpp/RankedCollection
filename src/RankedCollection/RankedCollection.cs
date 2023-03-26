﻿using System.Collections;

namespace EnhancedCollections;

public class RankedCollection<T> : ICollection<RankedItem<T>> where T : notnull
{
    readonly HashSet<RankedItem<T>> items = new();

    public int Count => items.Count();

    public bool IsReadOnly => false;

    public void Add(RankedItem<T> item)
    {
        item.SetRank(items.Count + 1);
        item.RankChanged += RankedItem_RankChanged;
        items.Add(item);
    }

    public bool Remove(RankedItem<T> item)
    {
        var i = items.RemoveWhere(ri => ri.Value.Equals(item.Value));
        if (i > 0)
        {
            item.RankChanged -= RankedItem_RankChanged;
            RankedItem_RankChanged(item, null);
            items.Remove(item);
            return true;
        }
        return false;
    }

    public RankedItem<T>? Find(T item) => items.TryGetValue(item, out var rankedItem) ? rankedItem : null;

    private int? RankedItem_RankChanged(RankedItem changingItem, int? desiredRank)
    {
        if (desiredRank < 1)
            desiredRank = 1;

        if (desiredRank > Count)
            desiredRank = Count;

        bool promoting = desiredRank < changingItem.Rank;
        bool demoting = desiredRank > changingItem.Rank;
        bool removing = desiredRank is null;

        foreach (var ri in items)
        {
            if (ri == changingItem)
            {
                continue;
            }

            var isAbove = ri.Rank < changingItem.Rank;
            var isBelow = ri.Rank > changingItem.Rank;

            if (promoting && isAbove)
            {
                ri.SetRank(ri.Rank + 1); //demote
            }

            if ((demoting || removing) && isBelow)
            {
                ri.SetRank(ri.Rank - 1); //promote
            }
        }
        return desiredRank;
    }

    public void Clear() => items.Clear();

    public bool Contains(RankedItem<T> item) => items.Any(ri => ri.Value.Equals(item.Value));

    public void CopyTo(RankedItem<T>[] array, int arrayIndex) =>
        items.CopyTo(array, arrayIndex);

    public IEnumerator<RankedItem<T>> GetEnumerator() => items.OrderBy(i => i.Rank).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
