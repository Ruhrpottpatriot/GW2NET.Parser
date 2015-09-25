namespace GW2NET.Parser
{
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary> Contains methods to parse a arbitrary string to a <see cref="RichTextString"/>. </summary>
    public class RichTextStringParser : IParser<string, RichTextString>
    {
        /// <summary> Parses a string to a <see cref="RichTextString"/>. </summary>
        /// <param name="textToParse">The text to parse.</param>
        /// <returns>The <see cref="RichTextString"/>.</returns>
        public RichTextString Parse(string textToParse)
        {
            MatchCollection matches = Regex.Matches(textToParse, @"<c=@(.+?)>(.+?)</c>");

            RichTextString richTextString = new RichTextString(matches.Count, textToParse);

            int index = 0;

            foreach (Match match in matches)
            {
                richTextString.Text = richTextString.Text.Replace(match.Value, "{" + index + "}");

                richTextString.Markup[index] = new FlavorString
                {
                    FlavorType = match.Groups[1].Value,
                    Value = match.Groups[2].Value
                };

                index++;
            }

            return richTextString;
        }

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        public Task<RichTextString> ParseAsync(string data)
        {
            return this.ParseAsync(data, CancellationToken.None);
        }

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        public Task<RichTextString> ParseAsync(string data, CancellationToken cancellationToken)
        {
            return cancellationToken.IsCancellationRequested ? null : Task.Run(() => this.Parse(data), cancellationToken);
        }
    }
}
