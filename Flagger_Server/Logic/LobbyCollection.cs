using Flagger.Models;
using Flagger_Server.Logic;
using System.Collections;
using System.Collections.Concurrent;

namespace Flagger.Server.Logic
{
    public class LobbyCollection : ILobbyCollection
    {
        public ConcurrentDictionary<Guid, Lobby> Lobbies => _lobbies;

        private ConcurrentDictionary<Guid, Lobby> _lobbies = new();

        public LobbyLoginResponse Login(Guid guid, LobbyLoginRequest loginRequest)
        {
            if (_lobbies.TryGetValue(guid, out Lobby lobby))
            {
                lobby.Kick();
                return lobby.Login(loginRequest);
            }

            return new LobbyLoginResponse { Response = LobbyLoginResponse.ResponseType.Rejected_Other};
        }

        public bool CreateLobby(Guid guid, CreateLobbyRequest createRequest)
        {
            var lobby = new Lobby(createRequest.Password, createRequest.LobbySettings, LobbyTimeout);
            lobby.Kick();

            return _lobbies.TryAdd(guid, lobby);
        }

        public void LobbyTimeout(Guid guid)
        {
            if (!_lobbies.TryRemove(guid, out _))
            {
                Console.WriteLine("Failed to remove lobby!");
            }
        }

        public bool TryGetLobby(Guid guid, out Lobby lobby)
        {
            if (_lobbies.TryGetValue(guid, out lobby))
            {
                lobby.Kick();
                return true;
            }
            return false;
        }
    }
}
