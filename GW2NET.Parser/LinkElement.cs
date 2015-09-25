namespace GW2NET.Parser
{
    using System;

    /// <summary>Represents a link elemet
    /// </summary>
    public class LinkElement : IEquatable<LinkElement>
    {
        /// <summary>Gets or sets the page uri.</summary>
        public Uri Uri { get; set; }

        /// <summary>Gets or sets the page index.</summary>
        public int PageIndex { get; set; }

        /// <summary>Gets or sets the page size.</summary>
        public int PageSize { get; set; }

        /// <summary>Gets or sets the link elements relation.</summary>
        public RelationType Relation { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LinkElement other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Uri.Equals(this.Uri, other.Uri) && this.PageIndex == other.PageIndex && this.PageSize == other.PageSize && this.Relation == other.Relation;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            LinkElement elem = obj as LinkElement;
            return elem != null && this.Equals(elem);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Uri != null ? this.Uri.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ this.PageIndex;
                hashCode = (hashCode * 397) ^ this.PageSize;
                hashCode = (hashCode * 397) ^ (int)this.Relation;
                return hashCode;
            }
        }
    }
}