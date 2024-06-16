using System.Security;
using Flagger;
using Flagger.Models;

namespace Flagger.Server.Logic
{
    public class Lobby
    {
        public string PasswordHash { get; set; }

        public Dictionary<string, User> Users { get; set; } = new();
        public List<FlagModel> Flags { get; set; } = new();

        public LobbySettings LobbySettings { get; set; }

        private const int TimeoutTime = 30_000;
        public Timer LobbyWatchdogTimer { get; set; }
        public Action<Guid> LobbyTimeoutCallback { get; set; }

        public Lobby(string passwordHash, LobbySettings lobbySettings, Action<Guid> lobbyTimeoutCallback)
        {
            PasswordHash = passwordHash;
            LobbySettings = lobbySettings;
            LobbyTimeoutCallback = lobbyTimeoutCallback;

            for (int i = 0; i < lobbySettings.NumberOfFlags; i++)
            {
                var newFlag = new FlagModel();
                newFlag.Number = i;
                Flags.Add(newFlag);
            }

            LobbyWatchdogTimer = new Timer((x) => { ((Lobby)x).LobbyTimeoutCallback?.Invoke(((Lobby)x).LobbySettings.Guid); }, this, TimeoutTime, Timeout.Infinite);
        }

        public void Kick()
        {
            LobbyWatchdogTimer.Change(TimeoutTime, Timeout.Infinite);
        }

        public LobbyLoginResponse Login(LobbyLoginRequest request)
        {
            if (Users.Count >= LobbySettings.MaxUsers)
            {
                return new LobbyLoginResponse { Response = LobbyLoginResponse.ResponseType.Rejected_Full };
            }

            if (request.Password == PasswordHash)
            {
                string token = Guid.NewGuid().ToString();

                var response = new LobbyLoginResponse
                {
                    Response = LobbyLoginResponse.ResponseType.Accepted,
                    SessionToken = token
                };

                Users.Add(token, new User() { SessionToken = token });

                return response;
            } else
            {
                return new LobbyLoginResponse { Response = LobbyLoginResponse.ResponseType.Rejected_InvalidLogin };
            }
        }
    }
}
