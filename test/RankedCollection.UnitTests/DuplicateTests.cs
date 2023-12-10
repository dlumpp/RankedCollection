using System;
using EnhancedCollections;
using FluentAssertions;
using Xunit;

namespace RankedList.UnitTest;

public class DuplicateTests 
{
    [Fact]
    public void AddingDuplicateValueThrows()
    {
        var sut = new RankedCollection<bool>();
        sut.Add(true);
        Action act = () => sut.Add(true);
        act.Should().Throw<InvalidAddException<bool>>()
            .WithMessage("Collection already contains an equivalent item.")
            .And.InvalidItem.Value.Should().Be(true);
    }
}
