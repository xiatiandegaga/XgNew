using Cloud.Utilities.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiClientCore;

namespace Cloud.WebApiClient
{
    public class JsonConfigInit
    {
        public static Action<HttpApiOptions> InitJsonSerializeOptions() => options =>
            {
                JsonUtility.InitJsonOptions(options.JsonSerializeOptions);
                JsonUtility.InitJsonOptions(options.JsonDeserializeOptions);
            };

    }
}
