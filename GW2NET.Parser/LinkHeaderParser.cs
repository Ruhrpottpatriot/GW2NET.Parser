namespace GW2NET.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Contains methods to parse the link header parameter of a y<see cref="WebHeaderCollection"/> into a usable <see cref="IEnumerable{T}"/> of <see cref="LinkElement"/>.</summary>
    public class LinkHeaderParser : IParser<WebHeaderCollection, IEnumerable<LinkElement>>
    {
        /// <summary>Parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>The parsed data of type: <see cref="TOutput"/>.
        /// </returns>
        public IEnumerable<LinkElement> Parse(WebHeaderCollection data)
        {
            var linkHeaderContent = data["Link"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<LinkElement> elements = new List<LinkElement>();

            foreach (string content in linkHeaderContent)
            {
                // Split at the ; char
                var relSplit = content.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // Get the relation
                RelationType relation;
                if (!Enum.TryParse(relSplit[1].Substring(5), true, out relation))
                {
                    relation = RelationType.Unknown;
                }

                // Get the link
                var linkSplit = relSplit[0].Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

                // Get the page size and index
                var pageSplit = linkSplit[1].Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                elements.Add(new LinkElement
                {
                    Relation = relation,
                    Uri = new Uri(linkSplit[0].Trim().TrimStart('<'), UriKind.Relative),
                    PageIndex = int.Parse(pageSplit[0].Substring(5)),
                    PageSize = int.Parse(pageSplit[1].Substring(10).TrimEnd('>'))
                });
            }

            return elements;
        }

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        public Task<IEnumerable<LinkElement>> ParseAsync(WebHeaderCollection data)
        {
            return this.ParseAsync(data, CancellationToken.None);
        }

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        public Task<IEnumerable<LinkElement>> ParseAsync(WebHeaderCollection data, CancellationToken cancellationToken)
        {
            return cancellationToken.IsCancellationRequested ? null : Task.Run(() => this.Parse(data), cancellationToken);
        }
    }
}
