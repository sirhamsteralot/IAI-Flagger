using Flagger.Models;
using System.Net;
using System.Net.Http.Json;

namespace Flagger.Client
{
    public class FlaggerClient
    {
        public bool IsInLobby { get => !string.IsNullOrEmpty(lobbySessionToken) && !string.IsNullOrEmpty(currentLobbyId); }

        string uriRoot = "";
        IHttpClientFactory httpClientFactory;

        string lobbySessionToken = string.Empty;
        string currentLobbyId = string.Empty;

        public void Init(string uriRoot, IHttpClientFactory client)
        {
            this.uriRoot = uriRoot;
            httpClientFactory = client;
        }

        public async Task<List<LobbySettings>> GetLobbies()
        {
            try
            {
                using (var httpClient = httpClientFactory.Create())
                {
                    httpClient.BaseAddress = new Uri(uriRoot);
                    var response = await httpClient.GetAsync("lobby/list");

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<List<LobbySettings>>();
                    }

                    return null;
                }
            } catch (Exception)
            {
                return null;
            }
        }

        public async Task<LobbySettings> GetLobby(string id)
        {
            try
            {
                using (var httpClient = httpClientFactory.Create())
                {
                    httpClient.BaseAddress = new Uri(uriRoot);

                    var response = await httpClient.GetAsync($"lobby/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<LobbySettings>();
                    }

                    return null;
                }
            } catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> EnterLobby(string password, Guid id)
        {
            try
            {
                using (var httpClient = httpClientFactory.Create())
                {
                    httpClient.BaseAddress = new Uri(uriRoot);

                    LobbyLoginRequest loginRequest = new LobbyLoginRequest
                    {
                        Password = password,
                    };

                    var response = await httpClient.PostAsJsonAsync($"lobby/{id}/login", loginRequest);

                    string stringRead = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(stringRead);

                    if (response.IsSuccessStatusCode)
                    {
                        LobbyLoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LobbyLoginResponse>();

                        if (loginResponse?.Response != LobbyLoginResponse.ResponseType.Accepted)
                        {
                            return false;
                        }

                        currentLobbyId = id.ToString();
                        lobbySessionToken = loginResponse.SessionToken;
                        return true;
                    }

                    return false;
                }
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<FlagModel>> SyncFlagStatus(List<FlagModel> flags)
        {
            try
            {
                using (var httpClient = httpClientFactory.Create())
                {
                    httpClient.BaseAddress = new Uri(uriRoot);

                    if (!IsInLobby)
                        return null;

                    FlagStatusRequest flagStatusRequest = new FlagStatusRequest
                    {
                        SessionToken = lobbySessionToken,
                        Flags = flags
                    };

                    var response = await httpClient.PostAsJsonAsync($"lobby/{currentLobbyId}/flags", flagStatusRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        FlagStatusResponse flagStatusResponse = await response.Content.ReadFromJsonAsync<FlagStatusResponse>();

                        return flagStatusResponse.Flags;
                    }

                    return null;
                }
            } catch (Exception)
            {
                return null;
            }

        }

        public async Task<bool> CreateLobby(LobbySettings settings, string password, string lobbyId)
        {
            try
            {
                using (var httpClient = httpClientFactory.Create())
                {
                    httpClient.BaseAddress = new Uri(uriRoot);

                    CreateLobbyRequest createLobbyRequest = new CreateLobbyRequest
                    {
                        LobbySettings = settings,
                        Password = password
                    };

                    var response = await httpClient.PutAsJsonAsync($"lobby/create/{lobbyId}", createLobbyRequest);

                    return response.IsSuccessStatusCode;
                }
            } catch (Exception) {
                return false;
            }

        }
    }
}
