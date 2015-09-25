namespace GW2NET.Parser
{
    /// <summary>Represents a guild emblem.</summary>
    public class EmblemBase
    {
        /// <summary>Gets or sets the name of the emblem.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the image data.</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "The return type is intended here.")]
        public byte[] ImageData { get; set; }
    }
}