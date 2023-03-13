using System.Collections.Generic;
using System.Linq;
using EnhancedCollections;
using FluentAssertions;

public static class AssertProperty
{
    public static void RanksAscendByOne<T>(ICollection<RankedItem<T>> rankedCollection) where T : notnull =>
        rankedCollection.Select(x => x.Rank).Should().Equal(Enumerable.Range(1, rankedCollection.Count));
}
