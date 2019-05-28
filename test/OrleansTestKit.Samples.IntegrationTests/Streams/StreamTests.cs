//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using Orleans.Hosting;
//using Orleans.TestingHost;
//using Xunit;

//namespace Orleans.TestKit.Samples.Streams
//{
//    public sealed class StreamTests : IAsyncLifetime
//    {
//        private static readonly StringWriter writer = new StringWriter();

//        private readonly TestCluster cluster;

//        public StreamTests()
//        {
//            var builder = new TestClusterBuilder();
//            builder.AddSiloBuilderConfigurator<SiloBuilderConfigurator>();

//            this.cluster = builder.Build();
//        }

//        public Task DisposeAsync() =>
//            this.cluster.StopAllSilosAsync();

//        public Task InitializeAsync() =>
//            this.cluster.DeployAsync();

//        [Fact]
//        public async Task Test()
//        {
//            Console.SetOut(writer);

//            var producerOneId = Guid.Parse("00000000-0000-0000-0000-000000000001");
//            var producerOne = this.cluster.GrainFactory.GetGrain<IProducerGrain>(producerOneId);

//            var producerTwoId = Guid.Parse("00000000-0000-0000-0000-000000000002");
//            var producerTwo = this.cluster.GrainFactory.GetGrain<IProducerGrain>(producerTwoId);

//            var consumerOneId = Guid.Parse("10000000-0000-0000-0000-000000000000");
//            var consumerOne = this.cluster.GrainFactory.GetGrain<IConsumerGrain>(consumerOneId);

//            var consumerTwoId = Guid.Parse("20000000-0000-0000-0000-000000000000");
//            var consumerTwo = this.cluster.GrainFactory.GetGrain<IConsumerGrain>(consumerTwoId);

//            // Create first producer and publish value (no consumers!)
//            await producerOne.PublishValueAsync(1);

//            // Create first consumer; verify initial state
//            Assert.Equal(0, await consumerOne.GetValueAsync());

//            // Publish value from first producer to first consumer
//            await producerOne.PublishValueAsync(2);
//            Assert.Equal(2, await consumerOne.GetValueAsync());

//            // Deactivate first producer
//            await producerOne.GoToSleepAsync();
//            await Task.Delay(250); // just to allow the silo to do its thing...

//            // Create second consumer; verify initial state
//            Assert.Equal(0, await consumerTwo.GetValueAsync());

//            // Manually verify logs; was the first producer re-activated?
//            Console.WriteLine("----- Check if first producer was re-activated (A) -----");

//            // Create second producer and publish value to first and second consumer
//            await producerTwo.PublishValueAsync(3);
//            Assert.Equal(3, await consumerOne.GetValueAsync());
//            Assert.Equal(3, await consumerTwo.GetValueAsync());

//            // Manually verify logs; was the first producer re-activated?
//            Console.WriteLine("----- Check if first producer was re-activated (B) -----");

//            // Deactivate first consumer
//            await consumerOne.GoToSleepAsync();
//            await Task.Delay(250); // just to allow the silo to do its thing...

//            // Re-activate first consumer; verify initial state
//            Assert.Equal(0, await consumerOne.GetValueAsync());

//            // Manually verify logs; was the first producer re-activated?
//            Console.WriteLine("----- Check if first producer was re-activated (C) -----");

//            await writer.FlushAsync();
//            var output = writer.ToString();
//            Assert.NotNull(output);
//        }

//        public sealed class SiloBuilderConfigurator : ISiloBuilderConfigurator
//        {
//            public void Configure(ISiloHostBuilder builder)
//            {
//                Console.SetOut(writer);
//                builder
//                    .AddMemoryGrainStorage("PubSubStore")
//                    .AddSimpleMessageStreamProvider("SMS")
//                    .ConfigureLogging(logBuilder =>
//                    {
//                        logBuilder.AddConsole(options =>
//                        {
//                            options.DisableColors = true;
//                            options.IncludeScopes = false;
//                        });
//                        logBuilder.AddFilter(
//                            (scope, logLevel) =>
//                                scope != null && scope.StartsWith("Orleans.TestKit.Samples.Streams", StringComparison.OrdinalIgnoreCase));
//                        logBuilder.SetMinimumLevel(LogLevel.Information);
//                    });
//            }
//        }
//    }
//}
