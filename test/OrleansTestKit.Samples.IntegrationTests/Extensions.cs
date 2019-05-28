//using Microsoft.Extensions.DependencyInjection;
//using Orleans;
//using Orleans.Hosting;
//using Orleans.Timers;

//namespace Orleans.TestKit
//{
//    public static class Extensions
//    {
//        private const string StorageGrainExtension = "Orleans.TestKit.StorageGrainExtension";

//        public static ISiloHostBuilder AddTestKitGrainCallFilter(this ISiloHostBuilder builder) =>
//            builder.AddOutgoingGrainCallFilter<CallFilter>();

//        public static ISiloHostBuilder AddTestKitStorage(this ISiloHostBuilder builder, string name) =>
//            builder
//                .AddMemoryGrainStorage(name)
//                .AddTestKitStorageExtension();

//        public static ISiloHostBuilder AddTestKitStorageAsDefault(this ISiloHostBuilder builder) =>
//                    builder
//                .AddMemoryGrainStorageAsDefault()
//                .AddTestKitStorageExtension();

//        public static ISiloHostBuilder UseTestKitReminderService(this ISiloHostBuilder builder) =>
//            builder
//                .AddGrainExtension<ITestKitReminderExtension, TestKitReminderExtension>()
//                .ConfigureServices(services =>
//                {
//                    services.AddSingleton<IReminderRegistry, TestKitReminderRegistry>();
//                });

//        private static ISiloHostBuilder AddTestKitStorageExtension(this ISiloHostBuilder builder)
//        {
//            var registered = false;
//            if (builder.Properties.TryGetValue(typeof(TestKitStorageExtension), out var property))
//            {
//                var value = property as bool?;
//                registered = value.HasValue && value.Value;
//            }

//            if (registered)
//            {
//                return builder;
//            }

//            builder.AddGrainExtension<ITestKitStorageExtension, TestKitStorageExtension>();
//            builder.Properties[typeof(TestKitStorageExtension)] = true;
//            return builder;
//        }
//    }
//}
