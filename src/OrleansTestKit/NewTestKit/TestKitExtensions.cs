//using Microsoft.Extensions.DependencyInjection;
//using Orleans.Hosting;
//using Orleans.Timers;

//namespace Orleans.NewTestKit
//{
//    public static class TestKitExtensions
//    {
//        public static ISiloHostBuilder AddGrainProbe<TGrain>(this ISiloHostBuilder builder, TGrain grain)
//            where TGrain : Grain =>
//            builder
//                .AddTestKitGrainService()
//                .AddOutgoingGrainCallFilter<TestKitCallFilter>();

//        public static ISiloHostBuilder AddTestKitStorage(this ISiloHostBuilder builder, string name) =>
//                    builder
//                .AddTestKitGrainService()
//                .AddMemoryGrainStorage(name);

//        public static ISiloHostBuilder AddTestKitStorageAsDefault(this ISiloHostBuilder builder) =>
//            builder
//                .AddTestKitGrainService()
//                .AddMemoryGrainStorageAsDefault();

//        public static ISiloHostBuilder UseTestKitReminderService(this ISiloHostBuilder builder) =>
//            builder
//                .AddTestKitGrainService()
//                .ConfigureServices(services =>
//                {
//                    services.AddSingleton<IReminderRegistry, TestKitReminderRegistry>();
//                });

//        public static ISiloHostBuilder UseTestKitTimerService(this ISiloHostBuilder builder) =>
//                    builder
//                .AddTestKitGrainService()
//                .ConfigureServices(services =>
//                {
//                    services.AddSingleton<ITimerRegistry, TestKitTimerRegistry>();
//                });

//        private static ISiloHostBuilder AddTestKitGrainService(this ISiloHostBuilder builder)
//        {
//            var properties = builder.GetOrRegisterProperties();
//            if (!properties.GrainServiceRegistered)
//            {
//                builder.AddGrainService<TestKitService>();
//                builder.ConfigureServices(services =>
//                {
//                    services.AddSingleton<ITestKitServiceClient, TestKitServiceClient>();
//                });

//                properties.GrainServiceRegistered = true;
//            }

//            return builder;
//        }

//        private static Properties GetOrRegisterProperties(this ISiloHostBuilder builder)
//        {
//            Properties properties = null;
//            if (builder.Properties.TryGetValue(typeof(Properties), out var value))
//            {
//                properties = value as Properties;
//            }

//            if (properties == null)
//            {
//                builder.Properties[typeof(Properties)] = properties = new Properties();
//            }

//            return properties;
//        }

//        private sealed class Properties
//        {
//            public bool GrainServiceRegistered { get; set; }
//        }
//    }
//}
