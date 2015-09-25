namespace ParsingEngineTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using GW2NET.Parser;

    using Xunit;

    public class LinkParserTests
    {
        [Fact]
        public void ParseTest()
        {
            Assert.Equal<IEnumerable<LinkElement>>(this.CreateElements(), new LinkHeaderParser().Parse(this.CreateHeaders()));
        }

        private IEnumerable<LinkElement> CreateElements()
        {
            return new List<LinkElement>
            {
                new LinkElement
                {
                    Uri = new Uri("/v2/items", UriKind.Relative),
                    Relation = RelationType.Previous,
                    PageIndex = 11,
                    PageSize = 20
                },
                new LinkElement
                {
                    Uri = new Uri("/v2/items", UriKind.Relative),
                    Relation = RelationType.Next,
                    PageIndex = 13,
                    PageSize = 20
                },
                new LinkElement
                {
                    Uri = new Uri("/v2/items", UriKind.Relative),
                    Relation = RelationType.Self,
                    PageIndex = 12,
                    PageSize = 20
                },
                new LinkElement
                {
                    Uri = new Uri("/v2/items", UriKind.Relative),
                    Relation = RelationType.First,
                    PageIndex = 0,
                    PageSize = 20
                },
                new LinkElement
                {
                    Uri = new Uri("/v2/items", UriKind.Relative),
                    Relation = RelationType.Last,
                    PageIndex = 2258,
                    PageSize = 20
                },
            };
        }

        private WebHeaderCollection CreateHeaders()
        {
            return new WebHeaderCollection
            {
                "Access-Control-Expose-Headers: \"Link, X-Page-Total, X-Page-Size, X-Result-Total, X-Result-Count\"",
                "Cache-Control: \"public, max-age=300\"",
                "Content-Encoding: gzipContent-Language: de",
                "Content-Length: 1401",
                "Content-Type: application/json; charset=utf-8",
                "Date: Sat, 19 Sep 2015 17:10:41 GMT",
                "Expires: Sat, 19 Sep 2015 17:15:42 +0000",
                "Link: </v2/items?page=11&page_size=20>; rel=previous, </v2/items?page=13&page_size=20>; rel=next, </v2/items?page=12&page_size=20>; rel=self, </v2/items?page=0&page_size=20>; rel=first, </v2/items?page=2258&page_size=20>; rel=last",
                "Server: Microsoft-IIS/7.5",
                "Vary: Accept-Encoding",
                "X-Content-Type-Options: nosniff",
                "X-Page-Size: 20",
                "X-Page-Total: 2259",
                "X-Result-Count: 20",
                "X-Result-Total: 45174",
                "access-control-allow-origin: *"
            };
        }
    }
}

