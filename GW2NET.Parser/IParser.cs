namespace GW2NET.Parser
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Provides the interface to parse one arbitrary data into another.</summary>
    /// <typeparam name="TInput">The type of data to parse.</typeparam>
    /// <typeparam name="TOutput">The type of data to return.</typeparam>
    public interface IParser<in TInput, TOutput>
    {
        /// <summary>Parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>The parsed data of type: <see cref="TOutput"/>.
        /// </returns>
        TOutput Parse(TInput data);

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        Task<TOutput> ParseAsync(TInput data);

        /// <summary>Asynchronously parses arbitrary data into another.</summary>
        /// <param name="data">The data to parse.</param>
        /// <param name="cancellationToken">An <see cref="CancellationToken"/>, which propagates notification that this operations should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> containing the parsed data of type: <see cref="TOutput"/>.</returns>
        Task<TOutput> ParseAsync(TInput data, CancellationToken cancellationToken);
    }
}