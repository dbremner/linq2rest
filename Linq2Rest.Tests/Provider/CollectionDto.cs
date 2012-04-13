using System;
using System.Collections.Generic;

namespace Linq2Rest.Tests.Provider {
    public class CollectionDto
    {
        public int ID { get; set; }

        public string Content { get; set; }

        public double Value { get; set; }

        public DateTime Date { get; set; }

        public Choice Choice { get; set; }

        public ICollection<ChildDto> Children { get; set; }
    }
}