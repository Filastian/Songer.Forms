using Songer.Core.Client;

namespace Songer.Core.Services
{
    public class LogOutService : ILogOutService
    {
        public LogOutService(
            ICasheService casheService,
            IRestClient client,
            IAuthenticateService authenticateService,
            IPlayerService playerService)
        {
            _casheService = casheService;
            _client = client;
            _authenticateService = authenticateService;
            _playerService = playerService;
        }

        private readonly ICasheService _casheService;
        private readonly IRestClient _client;
        private readonly IAuthenticateService _authenticateService;
        private readonly IPlayerService _playerService;

        public void LogOut()
        {
            _casheService.ClearCashe();
            _client.Reset();
            _authenticateService.LogOut();
            _playerService.Reset();
        }
    }
}
