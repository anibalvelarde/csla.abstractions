namespace Csla.Abstractions.Core.Contracts
{
    /// <summary>
    /// Basic interface for Command business object types.
    /// </summary>
    public interface ICommandBaseCore : ICommandBase
    {
        /// <summary>
        /// Conducts the execution and logic of what a command business object must do.
        /// </summary>
        /// <returns>Returns 'true' if command is successful, 'false' otherwise.</returns>
        bool Execute();
        /// <summary>
        /// "remembers" the result of the last command operation.
        /// </summary>
        bool Result { get; }
    }
}
