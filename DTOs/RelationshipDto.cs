// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent.DAL
{
    using System;

    public class RelationshipDto
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public int AContentTypeId { get; set; }

        public int BContentTypeId { get; set; }

        public bool? ARequired { get; set; }

        public int? AMinLimit { get; set; }

        public int? AMaxLimit { get; set; }

        public string AHelpText { get; set; }

        public bool? BRequired { get; set; }

        public int? BMinLimit { get; set; }

        public int? BMaxLimit { get; set; }

        public string BHelpText { get; set; }

        public int? ALayoutRow { get; set; }

        public int? ALayoutColumn { get; set; }

        public int? BLayoutRow { get; set; }

        public int? BLayoutColumn { get; set; }

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

        public ContentTypeDto AContentType { get; set; }

        public ContentTypeDto BContentType { get; set; }
    }
}
