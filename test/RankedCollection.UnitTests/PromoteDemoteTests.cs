using EnhancedCollections;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace RankedList.UnitTest;

public class PromoteDemoteTests
{
    readonly RankedCollection<string> _sut;

    public PromoteDemoteTests()
    {
        _sut = CreateTestSubject();
    }

    static RankedCollection<string> CreateTestSubject() => new()
    {
        "Kanye",
        "Jay-Z",
        "Notorious B.I.G."
    };

    [Fact]
    public void ItemsAddInAscendingOrder()
    {
        RankedItem<string> ye = _sut[0];
        ye.Rank.Should().Be(1);
        ye.Value.Should().Be("Kanye");

        RankedItem<string> hov = _sut[1];
        hov.Rank.Should().Be(2);
        hov.Value.Should().Be("Jay-Z");

        RankedItem<string> biggie = _sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Value.Should().Be("Notorious B.I.G.");
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void PromoteOnce()
    {
        RankedItem<string> hov = _sut[1];
        hov.Rank.Should().Be(2);
        hov.Promote();
        hov.Rank.Should().Be(1);
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void PromoteTwiceInBounds()
    {
        RankedItem<string> biggie = _sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Promote();
        biggie.Promote();
        biggie.Rank.Should().Be(1);
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void PromoteOnceOutOfBounds()
    {
        RankedItem<string> ye = _sut[0];
        ye.Rank.Should().Be(1);
        ye.Promote();
        ye.Rank.Should().Be(1);
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void DemoteOnce()
    {
        RankedItem<string> hov = _sut[1];
        hov.Rank.Should().Be(2);
        hov.Demote();
        hov.Rank.Should().Be(3);
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void DemoteTwiceInBounds()
    {
        RankedItem<string> ye = _sut[0];
        ye.Rank.Should().Be(1);
        ye.Demote();
        ye.Demote();
        ye.Rank.Should().Be(3);
        AssertProperty.RanksAscendByOne(_sut);
    }

    [Fact]
    public void DemoteOnceOutOfBounds()
    {
        RankedItem<string> biggie = _sut[2];
        biggie.Rank.Should().Be(3);
        biggie.Demote();
        biggie.Rank.Should().Be(3);
        AssertProperty.RanksAscendByOne(_sut);
    }
}
