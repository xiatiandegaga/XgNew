using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cloud.DynamicExpress
{
    public class FilterCollection : Collection<IList<DynamicFilter>>
    {
        public FilterCollection()
          : base()
        { }
    }
}
