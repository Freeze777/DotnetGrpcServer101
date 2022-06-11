using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService101.Services
{
    public class CalculatorService : Calculator.CalculatorBase
    {
        private readonly ILogger<CalculatorService> _logger;

        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;
        }

        public override Task<SumResult> Sum(SumRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SumResult
            {
                Result = request.X + request.Y
            });
        }
    }
}
