using EnhancedCollections;
using FluentAssertions;
using Xunit;

namespace RankedList.UnitTest;

public class PromoteDemoteTests
{
    static RankedCollection<string> CreateTestSubject() => new()
    {
        "Kanye",
        "Jay-Z",
        "Notorious B.I.G."
    };

    [Fact]
    public void ItemsAddInAscendingOrder()
    {
        var sut = CreateTestSubject();

        RankedItem<string> ye = sut[0];
        ye.Rank.Should().Be(1);
        ye.Value.Should().Be("Kanye");

        RankedItem<string> hov = sut[1];
        hov.Rank.Should().Be(2);
        hov.Value.Should().Be("Jay-Z");

        RankedItem<string> biggie = sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Value.Should().Be("Notorious B.I.G.");
    }

    [Fact]
    public void PromoteOnce()
    {
        var sut = CreateTestSubject();
        RankedItem<string> hov = sut[1];
        hov.Rank.Should().Be(2);
        hov.Promote();
        hov.Rank.Should().Be(1);
    }

    [Fact]
    public void PromoteTwiceInBounds()
    {
        var sut = CreateTestSubject();
        RankedItem<string> biggie = sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Promote();
        biggie.Promote();
        biggie.Rank.Should().Be(1);
    }

    [Fact]
    public void PromoteOnceOutOfBounds()
    {
        var sut = CreateTestSubject();
        RankedItem<string> ye = sut[0];
        ye.Rank.Should().Be(1);
        ye.Promote();
        ye.Rank.Should().Be(1);
    }

    [Fact]
    public void DemoteOnce()
    {
        var sut = CreateTestSubject();
        RankedItem<string> hov = sut[1];
        hov.Rank.Should().Be(2);
        hov.Demote();
        hov.Rank.Should().Be(3);
    }

    [Fact]
    public void DemoteTwiceInBounds()
    {
        var sut = CreateTestSubject();
        RankedItem<string> ye = sut[0];
        ye.Rank.Should().Be(1);
        ye.Demote();
        ye.Demote();
        ye.Rank.Should().Be(3);
    }

    [Fact]
    public void DemoteOnceOutOfBounds()
    {
        var sut = CreateTestSubject();
        RankedItem<string> biggie = sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Demote();
        biggie.Rank.Should().Be(3);
    }
}
