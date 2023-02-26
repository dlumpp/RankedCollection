using System.Linq;
using EnhancedCollections;
using FluentAssertions;
using Xunit;

namespace RankedList.UnitTest;

public class DefragTests
{
    [Fact]
    public void RankBelowOneBecomesOne()
    {
        var sandwiches = new RankedCollection<string>()
        {
            "Rueben",
            "Cubano",
            "Club"
        };

        sandwiches[2].Rank = -1;

        sandwiches.Select(i => i.Value).Should().Equal(
            "Club",
            "Rueben",
            "Cubano"
            );
    }
}
