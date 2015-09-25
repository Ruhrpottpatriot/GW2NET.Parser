
namespace GW2NET.Parser
{
    /// <summary>Represents a rich text string.</summary>
    /// <remarks>This object represents a text string, that has some parts adorned with additional style information.
    /// These information have been cut out from the string and been stored inside the <see cref="Markup"/> property.
    /// The index of each information is equal to the index in the Markup property array.</remarks>
    public class RichTextString
    {
        /// <summary>Initializes a new instance of the <see cref="RichTextString"/> class.</summary>
        /// <param name="numberOfFlavorTexts">The total number of flavor text objects inside the string.</param>
        /// <param name="text">The complete text.</param>
        public RichTextString(int numberOfFlavorTexts, string text)
        {
            this.Markup = new FlavorString[numberOfFlavorTexts];
            this.Text = text;
        }

        /// <summary>Gets or sets the rich text.</summary>
        public string Text { get; set; }

        /// <summary>Gets or sets the array containing markup information.</summary>
        public FlavorString[] Markup { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string fullText = this.Text;

            for (int i = 0; i < this.Markup.Length; i++)
            {
                fullText = fullText.Replace("{" + i + "}", this.Markup[i].ToString());
            }

            return fullText;
        }
    }
}