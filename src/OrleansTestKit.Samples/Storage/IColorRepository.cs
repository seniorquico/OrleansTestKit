using System;
using System.Threading.Tasks;

namespace Orleans.TestKit.Samples.Storage
{
    /// <summary>Represents a custom, CRUD-based storage API.</summary>
    public interface IColorRepository
    {
        /// <summary>Asynchronously creates a storage record with the specified values.</summary>
        /// <param name="state">The values of the new storage record.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(ColorGrainState state);

        /// <summary>Asynchronously deletes the storage record with the specified unique ID.</summary>
        /// <param name="id">The unique ID of the storage record to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>Asynchronously gets the storage record with the specified unique ID.</summary>
        /// <param name="id">The unique ID of the storage record to get.</param>
        /// <returns>
        ///     A task that represents the asynchronous operation. The task result returns the storage record or
        ///     <c>null</c> if no storage record exists with <paramref name="id"/>.
        /// </returns>
        Task<ColorGrainState> GetByIdAsync(Guid id);

        /// <summary>Asynchronously updates the storage record with the specified unique ID and values.</summary>
        /// <param name="id">The unique ID of the storage record to update.</param>
        /// <param name="state">The new values of the storage record.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateAsync(Guid id, ColorGrainState state);
    }
}
