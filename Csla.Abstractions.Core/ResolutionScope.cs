namespace Csla.Abstractions
{
    /// <summary>
    /// Controls when a given Dependency in a business object is resolved (either on the server only - the client only - or both)
    /// </summary>
    /// <remarks>
    /// We could have used a bitfield here to eliminate the "ClientAndServer" enum value - but this seems to be more user friendly and there really are not that many enum values to justify it
    /// </remarks>
    public enum ResolutionScope : int
    {
        /// <summary>
        /// Default resolution scope. Dependencies will be set / disposed at Server side.
        /// </summary>
        Server = 0,

        /// <summary>
        /// The dependency is resolved client side only.
        /// </summary>
        Client = 1,

        /// <summary>
        /// The dependency will be resolved both client and server side
        /// </summary>
        ClientAndServer = 2
    }
}
