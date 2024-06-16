using Flagger.Models;
using Flagger.Server.Logic;
using Flagger_Server.Logic;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Flagger.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : ControllerBase
    {
        ILobbyCollection LobbyService { get; set; }

        ILogger<LobbyController> logger;

        public LobbyController(ILogger<LobbyController> logger, ILobbyCollection lobbyService)
        {
            this.logger = logger;

            LobbyService = lobbyService;
        }

        [HttpPost("{id}/flags")]
        public async Task<FlagStatusResponse> GetFlagStatus(Guid id, FlagStatusRequest flagStatusRequest)
        {
            if (LobbyService.TryGetLobby(id, out Lobby lobby))
            {
                if (flagStatusRequest.Flags != null && flagStatusRequest.Flags.Count == lobby.Flags.Count)
                {
                    lobby.Flags = flagStatusRequest.Flags;
                }

                var response = new FlagStatusResponse
                {
                    Flags = lobby.Flags
                };

                return response;
            }

            return new FlagStatusResponse();
        }


        [HttpGet("list")]
        public async Task<IEnumerable<LobbySettings>> Get()
        {
            if (LobbyService.Lobbies.Count == 0)
                return Enumerable.Empty<LobbySettings>();

            var settingsList = new List<LobbySettings>();
            foreach (var lobby in  LobbyService.Lobbies.Values)
            {
                settingsList.Add(lobby.LobbySettings);
            }

            return settingsList;
        }

        [HttpPut("create/{id}")]
        public async Task<IActionResult> PutLobby(Guid id, CreateLobbyRequest createLobbyRequest)
        {
            if (LobbyService.CreateLobby(id, createLobbyRequest))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("{id}/login")]
        public async Task<LobbyLoginResponse> PostLobbyLogin(Guid id, LobbyLoginRequest loginRequest)
        {
            return LobbyService.Login(id, loginRequest);
        }

        [HttpGet("{id}")]
        public async Task<LobbySettings> GetLobby(Guid id)
        {
            if (LobbyService.TryGetLobby(id, out Lobby lobby))
            {
                return lobby.LobbySettings;
            }

            return new LobbySettings();
        }
    }
}
