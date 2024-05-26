using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);
            if (resultPhotoService != null)
                courseCreateInput.Picture = resultPhotoService.Url;

            var response = await _httpClient.PostAsJsonAsync("courses/create",courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string courseId)
        {
            var response = await _httpClient.PutAsync($"courses/delete/{courseId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return responseSuccessful.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _httpClient.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            responseSuccessful.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return responseSuccessful.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getbyuserid/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccessful.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return responseSuccessful.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"courses/getbyid/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            responseSuccessful.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccessful.Data.Picture);

            return responseSuccessful.Data;
        }

        public async Task<bool> UpdateAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);
            if (resultPhotoService != null)
            {
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;
            }

            var response = await _httpClient.PutAsJsonAsync("courses/update", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
