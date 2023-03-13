using System.Collections.Generic;
using System.Linq;
using EnhancedCollections;
using FluentAssertions;
using Xunit;

namespace RankedList.UnitTest;

public class CollectionImplementationTests
{
    [Fact]
    public void ICollectionImplemented()
    {
        ICollection<RankedItem<char>> sut = new RankedCollection<char>();

        sut.Add('a');
        sut.Add('b');

        sut.Count.Should().Be(2);
        sut.IsReadOnly.Should().BeFalse();

        sut.Remove('b').Should().BeTrue();
        sut.Remove('b').Should().BeFalse();

        sut.Clear();
        sut.Count.Should().Be(0);

        sut.Add('c');
        sut.Contains('c').Should().BeTrue();
        sut.Contains('a').Should().BeFalse();

        var arr = new RankedItem<char>[sut.Count];
        sut.CopyTo(arr, 0);
        arr[0].Value.Should().Be('c');
    }

    [Fact]
    public void EnumeratesInRankAscendingOrder()
    {
        var sut = new RankedCollection<string>() {
            "Turkey",
            "Bacon",
            "Ham"
        };
        sut[1].Promote();
        sut.Select(ri => ri.Value).Should().Equal(
            "Bacon",
            "Turkey",
            "Ham"
            );
    }
}
