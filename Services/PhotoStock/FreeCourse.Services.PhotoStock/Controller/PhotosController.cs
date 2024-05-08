using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", formFile.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await formFile.CopyToAsync(stream, cancellationToken);

                var returnPath = "Photos/" + formFile.FileName;

                PhotoDto photo = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photo, 200));
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty.", 400));
        }

        [HttpPut]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos", photoUrl);
            if (!System.IO.File.Exists(path))
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found.", 404));
            else
            {
                System.IO.File.Delete(path);
                return CreateActionResultInstance(Response<NoContent>.Success(204));
            }
        }
    }
}
