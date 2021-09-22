// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    /// <summary>
    /// A DTO representing a Relationship.
    /// </summary>
    public class RelationshipDto
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets AContentTypeId.
        /// </summary>
        public int AContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets BContentTypeId.
        /// </summary>
        public int BContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets ARequired.
        /// </summary>
        public bool? ARequired { get; set; }

        /// <summary>
        /// Gets or sets AMinLimit.
        /// </summary>
        public int? AMinLimit { get; set; }

        /// <summary>
        /// Gets or sets AMaxLimit.
        /// </summary>
        public int? AMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets AHelpText.
        /// </summary>
        public string AHelpText { get; set; }

        /// <summary>
        /// Gets or sets BRequired.
        /// </summary>
        public bool? BRequired { get; set; }

        /// <summary>
        /// Gets or sets AMinLimit.
        /// </summary>
        public int? BMinLimit { get; set; }

        /// <summary>
        /// Gets or sets BMaxLimit.
        /// </summary>
        public int? BMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets BHelpText.
        /// </summary>
        public string BHelpText { get; set; }

        /// <summary>
        /// Gets or sets ALayoutRow.
        /// </summary>
        public int? ALayoutRow { get; set; }

        /// <summary>
        /// Gets or sets ALayoutColumn.
        /// </summary>
        public int? ALayoutColumn { get; set; }

        /// <summary>
        /// Gets or sets BLayoutRow.
        /// </summary>
        public int? BLayoutRow { get; set; }

        /// <summary>
        /// Gets or sets BLayoutColumn.
        /// </summary>
        public int? BLayoutColumn { get; set; }

        /// <summary>
        /// Gets RelationshipName.
        /// </summary>
        public string RelationshipName
        {
            get
            {
                switch (this.Key)
                {
                    case "o2m":
                        return "One To Many";

                    case "m2o":
                        return "Many To One";

                    case "m2m":
                        return "Many To Many";

                    default:
                        return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets or sets AContentType.
        /// </summary>
        public ContentTypeDto AContentType { get; set; }

        /// <summary>
        /// Gets or sets AContentType.
        /// </summary>
        public ContentTypeDto BContentType { get; set; }

        /// <summary>
        /// Gets AColumnName.
        /// </summary>
        public string AColumnName
        {
            get
            {
                if (this.AContentType != null)
                {
                    switch (this.Key)
                    {
                        case "o2m":
                            return this.AContentType.Singular.ToLower() + "Id";

                        case "m2m":
                            return this.AContentType.Plural.ToLower();

                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets BColumnName.
        /// </summary>
        public string BColumnName
        {
            get
            {
                if (this.BContentType != null)
                {
                    switch (this.Key)
                    {
                        case "o2m":
                            return this.AContentType.Plural.ToLower();

                        case "m2m":
                            return this.BContentType.Plural.ToLower();

                        default:
                            return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
