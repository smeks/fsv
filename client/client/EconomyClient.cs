using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Economy.Client.DTO;
using Economy.Domain.Exceptions;
using Economy.Domain.IManager;
using Economy.Domain.Models;
using Newtonsoft.Json;

namespace Economy.Client
{
    public class EconomyClient : IEconomyClient
    {
        private static HttpClient _client;
        private static IMapper _mapper;
        private static User _currentUser;
        public EconomyClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("http://localhost:56791/");
                // Setting content type.  
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if(_currentUser != null)
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentUser.Token);

                _mapper = new MapperConfiguration(config => config.AddProfile(new EconomyClientMappingProfile())).CreateMapper();
            }
        }

        public async Task<Player> GetPlayer()
        {
            var response = await _client.GetAsync($"api/player");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var player = await response.Content.ReadAsStringAsync();
                    var dc = JsonConvert.DeserializeObject<PlayerDC>(player);
                    return _mapper.Map<Player>(dc);
                case HttpStatusCode.NotFound:
                    throw new ClientNotFoundException(response.Content.ToString());
                default:
                    throw new ClientBadRequestException(response.Content.ToString());
            }
        }

        public async Task<PlayerFlight> GetPlayerFlight(double lat, double lon, string model)
        {
            var modelEndcoded = WebUtility.UrlEncode(model);
            var response = await _client.GetAsync($"api/playerflight?lat={lat}&lon={lon}&model={modelEndcoded}");

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var player = await response.Content.ReadAsStringAsync();
                    var dc = JsonConvert.DeserializeObject<PlayerFlightDC>(player);
                    return _mapper.Map<PlayerFlight>(dc);
                case HttpStatusCode.NotFound:
                    throw new ClientNotFoundException(response.Content.ToString());
                default:
                    throw new ClientBadRequestException(response.Content.ToString());
            }
        }

        public async Task<Player> StartFlight(double lat, double lon, string model)
        {
            var startFlight = new StartFlightDC()
            {
                Longitude = lon,
                Latitude = lat,
                Model = model
            };
            var request = new StringContent(JsonConvert.SerializeObject(startFlight), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/playerflight/start", request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var player = await response.Content.ReadAsStringAsync();
                    var dc = JsonConvert.DeserializeObject<PlayerDC>(player);
                    return _mapper.Map<Player>(dc);
                case HttpStatusCode.NotFound:
                    throw new ClientNotFoundException(response.Content.ToString());
                default:
                    throw new ClientBadRequestException(response.Content.ToString());
            }
        }

        public async Task<Player> EndFlight(double lat, double lon, string model)
        {
            var endFlight = new EndFlightDC()
            {
                Longitude = lon,
                Latitude = lat,
                Model = model
            };
            var request = new StringContent(JsonConvert.SerializeObject(endFlight), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"api/playerflight/end", request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var player = await response.Content.ReadAsStringAsync();
                    var dc = JsonConvert.DeserializeObject<PlayerDC>(player);
                    return _mapper.Map<Player>(dc);
                case HttpStatusCode.NotFound:
                    throw new ClientNotFoundException(response.Content.ToString());
                default:
                    throw new ClientBadRequestException(response.Content.ToString());
            }
        }

        public async Task<User> Authenticate(string userName, string password)
        {
            var userAuth = new UserDC()
            {
                Username = userName,
                Password = password
            };
            var request = new StringContent(JsonConvert.SerializeObject(userAuth), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"api/Users/authenticate", request);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var player = await response.Content.ReadAsStringAsync();
                    var dc = JsonConvert.DeserializeObject<UserDC>(player);
                    _currentUser = _mapper.Map<User>(dc);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _currentUser.Token);
                    return _currentUser;
                case HttpStatusCode.NotFound:
                    throw new ClientNotFoundException(response.Content.ToString());
                case HttpStatusCode.Unauthorized:
                    throw new ClientUnauthorizedException(response.Content.ToString());
                default:
                    throw new ClientBadRequestException(response.Content.ToString());
            }
        }
    }
}
