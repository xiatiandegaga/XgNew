using System.Threading.Tasks;

namespace Cloud.LogisticsFuQing.Models
{
    public interface ILogisticsQuery
    {
        Task<T> GenericNoQuery<T>(LogisticsRequest request) where T : class;

        Task<FuQingNoQueryResponse> NoQuery(LogisticsRequest request);

        Task<FuQingNoQueryResultResponse> NoQueryResult(LogisticsRequest request);
    }
}
