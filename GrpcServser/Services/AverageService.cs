using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcService101.Services
{
    public class AverageService : Average.AverageBase
    {
        public override async Task<AverageResponse> ComputeAverage(IAsyncStreamReader<AverageRequest> requestStream,
            ServerCallContext context)
        {
            var numbers = new List<int>();
            while (await requestStream.MoveNext())
            {
                numbers.Add(requestStream.Current.Number);
            }

            return new AverageResponse
            {
                Average = numbers.Average()
            };
        }
    }
}
