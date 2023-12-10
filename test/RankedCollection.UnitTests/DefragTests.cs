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
    public void WhenRemovingByValue_ThenRanksGetPlugged(string itemToRemove)
    {
        _sut.Remove(itemToRemove).Should().BeTrue();
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Theory]
    [InlineData("Rueben")]
    [InlineData("Cubano")]
    [InlineData("Club")]
    public void WhenRemovingByItem_ThenRanksGetPlugged(string itemToRemove)
    {
        _sut.Remove(_sut.Find(itemToRemove)!).Should().BeTrue();
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void WhenNoItemFoundToRemove_ThenReturnFalse()
    {
        _sut.Remove("Monte Cristo").Should().BeFalse();
    }

    [Fact]
    public void RankBelowOneBecomesOne()
    {
        _sut.Find("Club")!.Rank = -1;

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
        _sut.Find("Rueben")!.Rank = _sut.Count * 2;

        _sut.Select(i => i.Value).Should().Equal(
            "Cubano",
            "Club",
            "Rueben"
            );
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void ReassignedItemChangesAllRanks()
    {
        _sut.Add("Lobster Roll");
        var newItem = _sut.Find("Lobster Roll");
        newItem!.Rank.Should().Be(_sut.Count);
        newItem!.Rank = 1;

        _sut.Select(i => i.Value).Should().Equal(
            "Lobster Roll",
            "Rueben",
            "Cubano",
            "Club"
            );
        AssertProperty.RanksAscendByOne(_sut);
    }
}
