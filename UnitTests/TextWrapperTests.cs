using System.Linq;
using TextWrapper;
using Xunit;
using Xunit.Extensions;

namespace UnitTests
{
    public class TextWrapperTests
    {
        public class TheWrapMethod
        {
            [Theory]
            [InlineData(@"This is not indented.
    This is indented
    So is this.
But this is not")]
            [InlineData("")]
            [InlineData("Blah blah blah")]
            [InlineData(@"Blah blah

blah blah")]
            public void DoesNotChangeTextThatFits(string text)
            {
                Assert.Equal(text, text.Wrap());
            }

            [Theory]
            [InlineData("This is just a bunch of text. This is more text. This is",
                @"This is just a
bunch of text.
This is more 
text. This is")]
            public void InsertsWrappingForTooLongLines(string text, string expected)
            {
                Assert.Equal(expected, text.Wrap(14));
            }

            [Theory]
            [InlineData("Lopado­temacho­selacho­galeo­kranio­leipsano­drim­hypo­trimmato­silphio­parao­melito­katakechy­meno­kichl­epi­kossypho­phatto­perister­alektryon­opte­kephallio­kigklo­peleio­lagoio­siraio­baphe­tragano­pterygon",
                @"Lopadotemachoselachogaleokranioleipsanodrimhypotrimmatosilphioparaomelitokatakec
hymenokichlepikossyphophattoperisteralektryonoptekephalliokigklopeleiolagoiosira
iobaphetraganopterygon")]
            public void ProperlyWrapsLongWords(string text, string expected)
            {
                Assert.Equal(text, expected);
            }
        }

        public class TheLinesWithinLengthMethod
        {
            [Theory]
            [InlineData("This is way too long", 80)]
            [InlineData("This is way too long", 20)]
            [InlineData("    This is way too long", 24)]
            [InlineData("", 20)]
            public void ReturnsLinesThatAreSmallerThanOrEqualToLength(string text, int length)
            {
                var lines = text.LinesWithinLength(length).ToArray();
                Assert.Equal(text, lines[0]);
            }

            [Theory]
            [InlineData(@"This is way too long", 12, "This is way ", "too long")]
            [InlineData(@"This is way too long", 11, "This is way", "too long")]
            public void ReturnsLinesFormattedToFit(string text, int maxLength, string firstLine, string secondLine)
            {
                var lines = text.LinesWithinLength(maxLength).ToArray();
                Assert.Equal(firstLine, lines[0]);
                Assert.Equal(secondLine, lines[1]);
            }

            [Fact]
            public void ReturnsWayTooLongLinesFormattedToFit()
            {
                const string text = @"This is way too long";
                var lines = text.LinesWithinLength(8).ToArray();
                Assert.Equal("This is ", lines[0]);
                Assert.Equal("way too ", lines[1]);
                Assert.Equal("long", lines[2]);
            }

            [Fact]
            public void ReturnsWayTooLongLinesWhenLengthHitsMiddleOfStringFormattedToFit()
            {
                const string text = @"This is way too long";
                var lines = text.LinesWithinLength(9).ToArray();
                Assert.Equal("This is ", lines[0]);
                Assert.Equal("way too ", lines[1]);
                Assert.Equal("long", lines[2]);
            }

            [Fact]
            public void KeepsExtraSpaces()
            {
                const string text = @"This is  way too long";
                var lines = text.LinesWithinLength(10).ToArray();
                Assert.Equal("This is  ", lines[0]);
                Assert.Equal("way too ", lines[1]);
                Assert.Equal("long", lines[2]);
            }

            [Fact]
            public void ReturnsEmptyStringForEmptyString()
            {
                const string text = @"";
                var lines = text.LinesWithinLength(10).ToArray();
                Assert.Equal("", lines[0]);
            }
        }
    }
}