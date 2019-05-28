using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace Orleans.TestKit.Samples.Streams
{
    public sealed class ProducerGrain : Grain, IProducerGrain
    {
        private readonly ILogger<IProducerGrain> logger;

        private IAsyncStream<int> stream;

        public ProducerGrain(ILogger<IProducerGrain> logger) =>
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task GoToSleepAsync()
        {
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }

        public override Task OnActivateAsync()
        {
            this.logger.LogInformation("Producer {Id} activated", this.GetPrimaryKey());

            var streamProvider = this.GetStreamProvider("SMS");
            this.stream = streamProvider.GetStream<int>(Guid.Empty, "Values");

            return Task.CompletedTask;
        }

        public override Task OnDeactivateAsync()
        {
            this.logger.LogInformation("Producer {Id} deactivated", this.GetPrimaryKey());

            return Task.CompletedTask;
        }

        public Task PublishValueAsync(int value)
        {
            this.logger.LogInformation("Producer {Id} published {Value}", this.GetPrimaryKey(), value);

            return this.stream.OnNextAsync(value);
        }
    }
}
