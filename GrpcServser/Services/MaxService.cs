using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService101.Services
{
    public class MaxService : Max.MaxBase
    {
        private readonly ILogger<MaxService> _logger;

        public MaxService(ILogger<MaxService> logger)
        {
            _logger = logger;
        }

        public override async Task ComputeMax(IAsyncStreamReader<MaxRequest> requestStream,
            IServerStreamWriter<MaxResponse> responseStream, ServerCallContext context)
        {
            var max = int.MinValue;
            while (await requestStream.MoveNext())
            {
                var request = requestStream.Current;
                if (max >= request.Number) continue;
                max = request.Number;
                await responseStream.WriteAsync(new MaxResponse
                {
                    Max = max
                });
            }
        }
    }
}
