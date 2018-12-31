using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArgipApiWpfConsume.Services
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }

    public class TokenPayloadData
    {
        public string iss { get; set; }
        public string[] aud { get; set; }
        public long exp { get; set; }
        public long nbf { get; set; }
        public string[] scope { get; set; }
    }

    public class AccessTokenService
    {
        private HttpClient _client;
        private string token = null;
        private long expiration = 0;

        private TokenPayloadData DecodeJWT(string jwt)
        {
            try
            {
                string strdata = JWT.JsonWebToken.Decode(jwt, string.Empty, false);
                return JsonConvert.DeserializeObject<TokenPayloadData>(strdata);
            }
            catch (JWT.SignatureVerificationException)
            {
                return null;
            }
        }

        private long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public async Task<string> GetAccessTokenAsync(string clientId, string clientSecret, string audience, string tokenEndpoint)
        {

            if (expiration <= UnixTimeNow())
            {
                _client = new HttpClient()
                {
                    BaseAddress = new Uri(tokenEndpoint)
                };

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                CancellationToken cancellationToken = default(CancellationToken);

                var fields = new Dictionary<string, string>
            {
              { "grant_type", "client_credentials" },
              { "audience", audience },
              { "client_id", clientId },
              { "client_secret", clientSecret }
            };


                var response = await _client.PostAsync(string.Empty, new FormUrlEncodedContent(fields), cancellationToken).ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    TokenResponse _response = JsonConvert.DeserializeObject<TokenResponse>(content);
                    TokenPayloadData _tokenpayloaddata = DecodeJWT(_response.access_token);

                    token = _response.access_token;
                    expiration = _tokenpayloaddata.exp;
                }
            }

            return token;
        }
    }
}
