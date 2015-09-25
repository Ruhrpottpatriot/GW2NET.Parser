namespace GW2NET.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>Contains methods to retrieve the guild emblems from the official Guild Wars 2 Wiki.</summary>
    public sealed class Gw2WikiEmblemParser : IParser<Uri, IEnumerable<EmblemBase>>, IDisposable
    {
        /// <summary>Holds a reference to the http client.</summary>
        private HttpClient httpClient;

        /// <summary>Initializes a new instance of the <see cref="Gw2WikiEmblemParser"/> class.</summary>
        /// <param name="httpClient">The http client used to make the web calls.
        /// </param>
        public Gw2WikiEmblemParser(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        
        /// <summary>Synchronously parses a webpage for guild emblems.</summary>
        /// <param name="pageUri">The <see cref="Uri"/> of the webpage.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="EmblemBase"/> with the downloaded emblems.</returns>
        /// <exception cref="ArgumentException">Thrown when the uri to the gallery is not an absolute Uri.</exception>
        /// <exception cref="HttpRequestException">Thrown when the download of one of the emblems failed.</exception>
        public IEnumerable<EmblemBase> Parse(Uri pageUri)
        {
            return this.ParseAsync(pageUri).Result;
        }

        /// <summary>Asynchronously parses a webpage for guild emblems.</summary>
        /// <param name="pageUri">The <see cref="Uri"/> of the webpage.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing a <see cref="IEnumerable{T}"/> of <see cref="EmblemBase"/> with the downloaded emblems.</returns>
        /// <exception cref="ArgumentException">Thrown when the uri to the gallery is not an absolute Uri.</exception>
        /// <exception cref="HttpRequestException">Thrown when the download of one of the emblems failed.</exception>
        public Task<IEnumerable<EmblemBase>> ParseAsync(Uri pageUri)
        {
            return this.ParseAsync(pageUri, CancellationToken.None);
        }

        /// <summary>Asynchronously parses a webpage for guild emblems.</summary>
        /// <param name="pageUri">The <see cref="Uri"/> of the webpage.</param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}" /> containing a <see cref="IEnumerable{T}"/> of <see cref="EmblemBase"/> with the downloaded emblems.</returns>
        /// <exception cref="ArgumentException">Thrown when the uri to the gallery is not an absolute Uri.</exception>
        /// <exception cref="HttpRequestException">Thrown when the download of one of the emblems failed.</exception>
        public async Task<IEnumerable<EmblemBase>> ParseAsync(Uri pageUri, CancellationToken cancellationToken)
        {
            if (!pageUri.IsAbsoluteUri)
            {
                throw new ArgumentException("URI to gallery has to be an absolute URI", "pageUri");
            }

            var baseUri = new Uri(string.Format("{0}://{1}", pageUri.Scheme, pageUri.Host));

            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            IEnumerable<string> emblemPageLinks = this.ExtractLinks(await this.GetPageSourceAsnc(pageUri, cancellationToken), new Regex("^/wiki/File:"));

            List<EmblemBase> links = new List<EmblemBase>();
            foreach (string link in emblemPageLinks)
            {
                ConfiguredTaskAwaitable<string> imageLink = this.GetPageSourceAsnc(new Uri(baseUri, link), cancellationToken).ConfigureAwait(false);

                string parsedLinks = this.ExtractLinks(await imageLink, new Regex(".*(/images/)(?!archive|thumb).*(guild_emblem_background|guild_emblem).*", RegexOptions.IgnoreCase)).FirstOrDefault();

                links.Add(await this.GetImageFromUrlAsync(new Uri(baseUri, parsedLinks), cancellationToken));
            }

            return links;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        /// <param name="disposing">The disposing.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (this.httpClient != null)
                {
                    this.httpClient.Dispose();
                    this.httpClient = null;
                }
            }
        }

        /// <summary>Asynchronously downloads an emblem from the wiki page.</summary>
        /// <param name="imageUri">An <see cref="Uri"/> pointing towards the image.</param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the emblem downloaded from the webserver.</returns>
        private async Task<EmblemBase> GetImageFromUrlAsync(Uri imageUri, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            EmblemBase emblem;
            var fileName = Path.GetFileName(imageUri.LocalPath);

            if (fileName.Contains("guild_emblem_background"))
            {
                emblem = new EmblemBackground();
            }
            else
            {
                emblem = new EmblemForeground();
            }

            emblem.Name = fileName;

            using (HttpResponseMessage message = await this.httpClient.GetAsync(imageUri, cancellationToken))
            {
                message.EnsureSuccessStatusCode();

                using (HttpContent content = message.Content)
                {
                    emblem.ImageData = await content.ReadAsByteArrayAsync();
                }
            }

            return emblem;
        }

        /// <summary>Asynchronously gets the html source of a webpage.</summary>
        /// <param name="siteUri">An <see cref="Uri"/> pointing to the webpage. </param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the html source of a webpage as <see cref="string"/>. </returns>
        private async Task<string> GetPageSourceAsnc(Uri siteUri, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return string.Empty;
            }

            using (HttpResponseMessage response = await this.httpClient.GetAsync(siteUri, cancellationToken))
            {
                // response.EnsureSuccessStatusCode();

                using (HttpContent content = response.Content)
                {
                    return await content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>Extracts the links from the passed source code.</summary>
        /// <param name="source">The page source (or fragment thereof) to parse.</param>
        /// <param name="selector">A <see cref="Regex"/> to select the links from the page source.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the extracted links as <see cref="string"/>.</returns>
        private IEnumerable<string> ExtractLinks(string source, Regex selector)
        {
            XDocument siteData = XDocument.Parse(source);

            return siteData.Descendants()
                .Where(n => n.Name == "a" && n.Attributes().Any(a => a.Name == "href"))
                .Where(link => link.Attributes()
                    .Any(attr => selector.IsMatch(attr.Value)))
                .SelectMany(link => link.Attributes("href")
                    .Select(a => a.Value));
        }
    }
}
