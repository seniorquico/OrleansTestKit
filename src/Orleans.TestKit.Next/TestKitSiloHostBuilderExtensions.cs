using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Orleans.TestKit;
using Orleans.TestKit.Grains;
using Orleans.Timers;

namespace Orleans.Hosting
{
    /// <summary>Provides <see cref="ISiloHostBuilder"/> extension methods to configure the TestKit.</summary>
    [DebuggerStepThrough]
    public static class TestKitSiloHostBuilderExtensions
    {
        /// <summary>
        ///     Configures the silo to run a fake reminder service. This will disable the actual reminder functionality
        ///     in the silo. Do not use this with any other `Use___ReminderService` configuration.
        /// </summary>
        /// <param name="builder">The silo builder.</param>
        /// <returns>The silo builder.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <c>null</c>.</exception>
        public static ISiloHostBuilder UseTestKitReminderService(this ISiloHostBuilder? builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Load grain types.
            builder.AddTestKitFrameworkPart();

            // Add reminder registry.
            var properties = builder.GetOrRegisterProperties();
            if (!properties.RegisteredReminderRegistry)
            {
                builder.AddGrainExtension<ITestKitReminderGrainExtension, TestKitReminderGrainExtension>();
                builder.ConfigureServices(services => services.AddSingleton<IReminderRegistry, TestKitReminderRegistry>());
                properties.RegisteredReminderRegistry = true;
            }

            return builder;
        }

        /// <summary>
        ///     Configures the silo to run a fake timer service. This will disable the actual timer functionality in the
        ///     silo.
        /// </summary>
        /// <param name="builder">The silo builder.</param>
        /// <returns>The silo builder.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <c>null</c>.</exception>
        public static ISiloHostBuilder UseTestKitTimerService(this ISiloHostBuilder? builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // Load grain types.
            builder.AddTestKitFrameworkPart();

            // Add timer registry.
            var properties = builder.GetOrRegisterProperties();
            if (!properties.RegisteredTimerRegistry)
            {
                builder.AddGrainExtension<ITestKitTimerGrainExtension, TestKitTimerGrainExtension>();
                builder.ConfigureServices(services => services.AddSingleton<ITimerRegistry, TestKitTimerRegistry>());
                properties.RegisteredTimerRegistry = true;
            }

            return builder;
        }

        /// <summary>Configures the silo to load the TestKit grain types.</summary>
        /// <param name="builder">The silo builder.</param>
        /// <returns>The silo builder.</returns>
        private static ISiloHostBuilder AddTestKitFrameworkPart(this ISiloHostBuilder builder)
        {
            var properties = builder.GetOrRegisterProperties();
            if (!properties.RegisteredFrameworkPart)
            {
                builder.ConfigureApplicationParts(parts => parts.AddFrameworkPart(typeof(ITestKitReminderGrainExtension).Assembly));
                properties.RegisteredFrameworkPart = true;
            }

            return builder;
        }

        /// <summary>Gets the property bag used to track the current configuration.</summary>
        /// <param name="builder">The silo builder.</param>
        /// <returns>The property bag.</returns>
        private static Properties GetOrRegisterProperties(this ISiloHostBuilder builder)
        {
            Properties? properties = null;
            if (builder.Properties.TryGetValue(typeof(Properties), out var value))
            {
                properties = value as Properties;
            }

            if (properties == null)
            {
                builder.Properties[typeof(Properties)] = properties = new Properties();
            }

            return properties;
        }

        /// <summary>A property bag used to track the TestKit configuration.</summary>
        private sealed class Properties
        {
            /// <summary>
            ///     Gets or sets a value indicating whether the TestKit's assembly has been registered as a framework
            ///     part.
            /// </summary>
            public bool RegisteredFrameworkPart { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the TestKit's fake reminder registry has been registered.
            /// </summary>
            public bool RegisteredReminderRegistry { get; set; }

            /// <summary>
            ///     Gets or sets a value indicating whether the TestKit's fake timer registry has been registered.
            /// </summary>
            public bool RegisteredTimerRegistry { get; set; }
        }
    }
}
