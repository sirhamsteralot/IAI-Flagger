using Flagger.Models;
using Flagger.Server.Logic;
using System.Collections.Concurrent;

namespace Flagger_Server.Logic
{
    public interface ILobbyCollection
    {
        public ConcurrentDictionary<Guid, Lobby> Lobbies { get; }

        public LobbyLoginResponse Login(Guid guid, LobbyLoginRequest loginRequest);
        public bool CreateLobby(Guid guid, CreateLobbyRequest createRequest);
        public bool TryGetLobby(Guid guid, out Lobby lobby);

    }
}
