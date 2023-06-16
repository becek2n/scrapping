using Newtonsoft.Json;
using Refit;
using Scrap.UI.Models.DTO;
using Scrap.UI.Models.Wordpress;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Scrap.UI.Models.DataAccessObject.Wordpress
{
    [Headers("Authorization: Basic")]
    public interface IPost {
        [Post("/posts")]
        Task<string> Create(WordpressPostRequestDTO request);
    }
    public class PostDAO
    {
        private readonly IPost _apiPost;
        public PostDAO()
        {
            var username = ConfigurationManager.AppSettings["Username"].ToString();
            var password = ConfigurationManager.AppSettings["Password"].ToString();
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            var baseAddress = ConfigurationManager.AppSettings["BaseURL"].ToString();

            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
            };

            _apiPost = RestService.For<IPost>(baseAddress, refitSettings);

        }
        public async Task<Post> Create(WordpressPostRequestDTO request)
        {
            Post model = new Post();

            try
            {
                var response = await _apiPost.Create(request);
                model = JsonConvert.DeserializeObject<Post>(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return model;
        }
    }
}