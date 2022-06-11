using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService101.Services
{
    public class PrimeFactorService : PrimeFactor.PrimeFactorBase
    {
        private readonly ILogger<PrimeFactorService> _logger;

        public PrimeFactorService(ILogger<PrimeFactorService> logger)
        {
            _logger = logger;
        }

        public override Task Factorize(PrimeFactorRequest request,
            IServerStreamWriter<PrimeFactorResponse> responseStream, ServerCallContext context)
        {
            var number = request.Number;
            var limit = Math.Sqrt(number);
            var primeFactors = new List<int>();

            for (var div = 2; div <= limit && number > 1; div++)
            {
                while (number % div == 0)
                {
                    primeFactors.Add(div);
                    number /= div;
                }
            }

            primeFactors.ForEach(async x =>
                {
                    await responseStream.WriteAsync(new PrimeFactorResponse
                    {
                        Prime = x
                    });
                }
            );
            return Task.CompletedTask;
        }
    }
}
