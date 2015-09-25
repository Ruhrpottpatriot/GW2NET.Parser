using System;
using System.Linq;
using System.Text;

namespace ParsingEngineTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;

    using GW2NET.Parser;

    using Xunit;

    public class EmblemParserTests
    {
        private readonly Uri wikiUri = new Uri("http://wiki.guildwars2.com/wiki/Gallery_of_guild_emblems");

        [Fact]
        public async void AsyncAndTokenTest()
        {
            HttpMessageHandler fakeHandler = FakeHttpMessageHandler.GetHttpMessageHandler(this.HtmlSourceMock(), HttpStatusCode.OK);

            HttpClient httpClient = new HttpClient(fakeHandler);

            Gw2WikiEmblemParser parser = new Gw2WikiEmblemParser(httpClient);

            IEnumerable<EmblemBase> foo = await parser.ParseAsync(this.wikiUri, CancellationToken.None);

            Assert.Equal(2, foo.Count());
        }

        [Fact]
        public async void ReltiveUriThrowsTest()
        {
            var parser = new Gw2WikiEmblemParser(new HttpClient());

            var foo = parser.ParseAsync(new Uri("/wiki/Gallery_of_guild_emblems", UriKind.Relative), CancellationToken.None);

            await Assert.ThrowsAsync<ArgumentException>("pageUri", () => foo);
        }

        [Fact]
        public async void CancellationTokenTest()
        {
            var parser = new Gw2WikiEmblemParser(new HttpClient());

            var foo = await parser.ParseAsync(this.wikiUri, new CancellationToken(true));

            Assert.Equal(null, foo);
        }

        private string HtmlSourceMock()
        {
            return "<div id=\"mw-content-text\" lang=\"en\" dir=\"ltr\" class=\"mw-content-ltr\">"
                   + "<ul class=\"gallery mw-gallery-traditional\" style=\"max-width: 815px;_width: 815px;\">"
                   + "<li class=\"gallerybox\" style=\"width: 155px\">" + "<div style=\"width: 155px\">"
                   + "<div class=\"thumb\" style=\"width: 150px;\">" + "<div style=\"margin:15px auto;\">"
                   + "<a href=\"/wiki/File:Guild_emblem_background_01.png\" class=\"image\">"
                   + "<img alt=\"Guild emblem background 01.png\" src=\"/images/thumb/c/c3/Guild_emblem_background_01.png/120px-Guild_emblem_background_01.png\" width=\"120\" height=\"120\" />"
                   + "</a>" + "</div>" + "</div>" + "<div class=\"gallerytext\">" + "</div>" + "</div>" + "</li>"
                   + "</ul>" + "</div>";
        }
    }
}
