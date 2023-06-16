using Newtonsoft.Json;
using Refit;
using Scrap.UI.Models.DTO;
using Scrap.UI.Models.Wordpress;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap.UI.Models.DataAccessObject.Wordpress
{
    [Headers("Authorization: Basic")]
    public interface ICategory
    {
        [Post("/categories")]
        Task<string> Create(WordpressCategoryRequestDTO request);

        [Get("/categories/{id}")]
        Task<string> Get(int id);
    }
    public class CategoryDAO
    {
        private readonly ICategory _api;
        public CategoryDAO()
        {
            var username = ConfigurationManager.AppSettings["Username"].ToString();
            var password = ConfigurationManager.AppSettings["Password"].ToString();
            var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            var baseAddress = ConfigurationManager.AppSettings["BaseURL"].ToString();

            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
            };

            _api = RestService.For<ICategory>(baseAddress, refitSettings);

        }
        public async Task<Category> Create(WordpressCategoryRequestDTO request)
        {
            Category model = new Category();

            try
            {
                var response = await _api.Create(request);
                model = JsonConvert.DeserializeObject<Category>(response);

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