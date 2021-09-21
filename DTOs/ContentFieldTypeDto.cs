// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace StructuredContent
{
    public class ContentFieldTypeDto
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public int Ordinal { get; set; }

        public int DefaultDataType { get; set; }

        public string DefaultDataLength { get; set; }

        public object DefaultOptions { get; set; }

        public string Icon { get; set; }
    }
}
