namespace ParsingEngineTests
{
    using System.Linq;

    using GW2NET.Parser;

    using Xunit;

    public class FlavourTextTest
    {
        [Fact]
        public void GetStringTest()
        {
            var parser = new RichTextStringParser();

            RichTextString parsedText = parser.Parse("This combination of plain text and <c=@flavor>colored text</c> is valid. <c=@warning>Multiple tags are also valid.</c>");

            Assert.Equal("This combination of plain text and {0} is valid. {1}", parsedText.Text);
            Assert.Equal(2, parsedText.Markup.Count());
        }
        
        [Fact]
        public void ToStringTest()
        {
            var parser = new RichTextStringParser();

            RichTextString parsedText = parser.Parse("This combination of plain text and <c=@flavor>colored text</c> is valid. <c=@warning>Multiple tags are also valid.</c>");

            Assert.Equal("This combination of plain text and <c=@flavor>colored text</c> is valid. <c=@warning>Multiple tags are also valid.</c>", parsedText.ToString());
        }

        [Fact]
        public void FlavorStringToStringText()
        {
            var flavorString = new FlavorString { FlavorType = "flavor", Value = "colored text" };

            Assert.Equal("<c=@flavor>colored text</c>", flavorString.ToString());
        }
    }
}
