using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace Orleans.TestKit.Samples.Streams
{
    public sealed class ConsumerGrain : Grain, IConsumerGrain
    {
        private readonly ILogger<IConsumerGrain> logger;

        private int value;

        public ConsumerGrain(ILogger<IConsumerGrain> logger) =>
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public Task<int> GetValueAsync() =>
            Task.FromResult(this.value);

        public Task GoToSleepAsync()
        {
            this.DeactivateOnIdle();
            return Task.CompletedTask;
        }

        public override async Task OnActivateAsync()
        {
            this.logger.LogInformation("Consumer {Id} activated", this.GetPrimaryKey());

            var streamProvider = this.GetStreamProvider("SMS");
            var stream = streamProvider.GetStream<int>(Guid.Empty, "Values");

            var subscriptions = await stream.GetAllSubscriptionHandles();
            var hasSubscription = false;
            foreach (var subscription in subscriptions)
            {
                if (hasSubscription)
                {
                    await subscription.UnsubscribeAsync();
                }
                else
                {
                    await subscription.ResumeAsync(this.OnNextValueAsync);
                    hasSubscription = true;
                }
            }

            if (!hasSubscription)
            {
                await stream.SubscribeAsync(this.OnNextValueAsync);
            }
        }

        public override Task OnDeactivateAsync()
        {
            this.logger.LogInformation("Consumer {Id} deactivated", this.GetPrimaryKey());

            return Task.CompletedTask;
        }

        private Task OnNextValueAsync(int value, StreamSequenceToken sequenceToken)
        {
            this.value = value;
            return Task.CompletedTask;
        }
    }
}
