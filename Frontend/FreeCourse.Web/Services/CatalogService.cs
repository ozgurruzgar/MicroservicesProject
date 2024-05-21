using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Createsync(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("courses/create",courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int courseId)
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
            return responseSuccessful.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"courses/getbyuserid/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return responseSuccessful.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"courses/getbyid/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccessful = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return responseSuccessful.Data;
        }

        public async Task<bool> UpdateAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("courses/update", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
