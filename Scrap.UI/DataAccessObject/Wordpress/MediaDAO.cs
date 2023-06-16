using Newtonsoft.Json;
using Refit;
using Scrap.UI.Models.DTO;
using Scrap.UI.Models.Wordpress;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Scrap.UI.Models.DataAccessObject.Wordpress
{
    [Headers("Authorization: Basic")]
    public interface IMedia {
        [Multipart]
        [Post("/media")]
        Task<string> Create(WordpressMediaRequestDTO request, [AliasAs("file")] byte[] bytes);

        [Get("/media/{id}")]
        Task<string> Get(int id);
    }
    public class MediaDAO
    {
        
        public async Task<Media> Create(WordpressMediaRequestDTO request, byte[] bytes)
        {
            Media model = new Media();
            var clientId = ConfigurationManager.AppSettings["Username"].ToString();
            var clientSecret = ConfigurationManager.AppSettings["Password"].ToString();
            var url = "https://infolooker.id/wp-json/wp/v2/media";

            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();

                
                //Post body content
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                var content = new FormUrlEncodedContent(values);

                var authenticationString = $"{clientId}:{clientSecret}";
                var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);

                form.Add(new StringContent(request.alt_text), "alt_text");
                form.Add(new StringContent(request.title), "title");
                form.Add(new StringContent(request.description), "description");
                form.Add(new StringContent(request.caption), "caption");
                form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", "hello1.jpg");
                HttpResponseMessage response = await httpClient.PostAsync(url, form);

                response.EnsureSuccessStatusCode();
                httpClient.Dispose();
                string resp = response.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<Media>(resp);
            }
            catch (ApiException ex)
            {
                if (ex.Content.Contains("already exists"))
                {
                    //parsing response exists data
                    var data = JsonConvert.DeserializeObject<ExceptionResponseDTO>(ex.Content);
                    model.id = data.data.term_id;
                    return model;
                }
                throw new Exception(ex.Message);
            }
            catch (Exception ex) { 
                throw new Exception(ex.Message);
            }

            return model;
        }
    }
}