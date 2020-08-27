using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GrainInterfaces;

namespace Grains
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }

        Task<string> IHello.SayHello(string greeting)
        {
            logger.LogInformation($"\n Привiтливе повiдомлення: '{greeting}'");
            return Task.FromResult($"\n Кожен день кажу: '{greeting}', отже все в порядку.");
        }
    }
}
