
namespace GW2NET.Parser
{
    /// <summary>Represents a flavored string.</summary>
    public class FlavorString
    {
        /// <summary>Gets or sets the type of the flavor text.</summary>
        public string FlavorType { get; set; }

        /// <summary>Gets or sets the value of the flavor text.</summary>
        public string Value { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("<c=@{0}>{1}</c>", this.FlavorType, this.Value);
        }
    }
}