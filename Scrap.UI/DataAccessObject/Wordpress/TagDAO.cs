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
    public interface ITag {
        [Post("/tags")]
        Task<string> Create(WordpressTagRequestDTO request);

        [Get("/tags/{id}")]
        Task<string> Get(int id);
    }
    public class TagDAO
    {
        private readonly ITag _api;
        public TagDAO()
        {
            var username = ConfigurationManager.AppSettings["Username"].ToString();
            var password = ConfigurationManager.AppSettings["Password"].ToString();
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            var baseAddress = ConfigurationManager.AppSettings["BaseURL"].ToString();

            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
            };

            _api = RestService.For<ITag>(baseAddress, refitSettings);

        }
        public async Task<Tag> Create(WordpressTagRequestDTO request)
        {
            Tag model = new Tag();

            try
            {
                var response = await _api.Create(request);
                model = JsonConvert.DeserializeObject<Tag>(response);

            }
            catch (ApiException ex)
            {
                if (ex.Content.Contains("already exists")) {
                    //parsing response exists data
                    var data = JsonConvert.DeserializeObject<ExceptionResponseDTO>(ex.Content);
                    model.id = data.data.term_id;
                    return model;
                }
                throw new Exception(ex.Message);
            }

            return model;
        }
    }
}