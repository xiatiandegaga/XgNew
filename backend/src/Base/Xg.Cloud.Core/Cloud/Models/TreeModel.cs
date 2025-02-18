using System.Collections.Generic;

namespace Cloud.Models
{
    public class TreeModel
    {
        public string Title { get; set; }
        public string Value { get; set; }

        public string Key { get; set; }

        public IEnumerable<TreeModel> Children { get; set; }
    }
}
