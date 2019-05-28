using System.Threading.Tasks;
using Orleans.Runtime;

namespace Orleans.TestKit
{
    /// <summary>
    ///     <para>
    ///         Represents a grain extension that provides methods to get persistent state from a storage provider.
    ///     </para>
    ///     <para>
    ///         These methods <i>do not</i> attempt to automatically determine the configuration of persistent state
    ///         (e.g. type of persistent state, name of storage provider). It is the responsibility of the test author to
    ///         call the these methods with the appropriate configuration.
    ///     </para>
    ///     <para>
    ///         Use the <see cref="GetGrainStateAsync{TGrainState}(string)"/> method to get the current persistent state
    ///         for a grain that extends <see cref="Grain{TGrainState}"/>.
    ///     </para>
    ///     <para>
    ///         Use the <see cref="GetGrainFacetStateAsync{TGrainState}(string, string)"/> method to get the current
    ///         persistent state for a grain that uses facets.
    ///     </para>
    /// </summary>
    public interface ITestKitStorageExtension : IGrainExtension
    {
        /// <summary>Asynchronously gets the current persistent state for a grain that uses facets.</summary>
        /// <typeparam name="TGrainState">The type of the persistent state value.</typeparam>
        /// <param name="stateName">The name of the facet.</param>
        /// <param name="storageName">
        ///     The name of the storage provider. Use <c>null</c> (the default value) to use the default storage
        ///     provider.
        /// </param>
        /// <returns>
        ///     A task that represents that asynchronous operation. The task result returns a value tuple containing the
        ///     current persistent state value and ETag.
        /// </returns>
        Task<(TGrainState state, string etag)> GetGrainFacetStateAsync<TGrainState>(string stateName, string storageName = null)
            where TGrainState : new();

        /// <summary>
        ///     Asynchronously gets the current persistent state for a grain that extends
        ///     <see cref="Grain{TGrainState}"/>.
        /// </summary>
        /// <typeparam name="TGrainState">The type of the persistent state value.</typeparam>
        /// <param name="storageName">
        ///     The name of the storage provider. Use <c>null</c> (the default value) to use the default storage
        ///     provider.
        /// </param>
        /// <returns>
        ///     A task that represents that asynchronous operation. The task result returns a value tuple containing the
        ///     current persistent state value and ETag.
        /// </returns>
        Task<(TGrainState state, string etag)> GetGrainStateAsync<TGrainState>(string storageName = null)
            where TGrainState : new();
    }
}
