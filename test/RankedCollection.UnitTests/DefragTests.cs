using System.Linq;
using EnhancedCollections;
using FluentAssertions;
using Xunit;

namespace RankedList.UnitTest;

public class DefragTests
{
    readonly RankedCollection<string> _sut = new()
        {
            "Rueben",
            "Cubano",
            "Club"
        };

    [Theory]
    [InlineData("Rueben")]
    [InlineData("Cubano")]
    [InlineData("Club")]
    public void RemovedRanksGetPlugged(string itemToRemove)
    {
        _sut.Remove(itemToRemove).Should().BeTrue();
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void RankBelowOneBecomesOne()
    {
        _sut[2].Rank = -1;

        _sut.Select(i => i.Value).Should().Equal(
            "Club",
            "Rueben",
            "Cubano"
            );
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void RankBeyondCountBecomesBottom()
    {
        _sut[0].Rank = _sut.Count * 2;

        _sut.Select(i => i.Value).Should().Equal(
            "Cubano",
            "Club",
            "Rueben"
            );
        AssertProperty.RanksAscendByOne(_sut);
    }
}
