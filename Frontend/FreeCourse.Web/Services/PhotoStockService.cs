using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.PhotoStock;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.PutAsync($"photos?photoUrl={photoUrl}",null);
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
                return null;

            var randomFileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(photo.FileName)}";

            using var memoryStream = new MemoryStream();
            await photo.CopyToAsync(memoryStream);

            var multipartContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(memoryStream.ToArray()), "photo", randomFileName }
            };

            var response = await _httpClient.PostAsync("photos", multipartContent);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return responseSuccess.Data;
        }
    }
}
