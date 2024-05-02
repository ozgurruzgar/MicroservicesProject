using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomBaseController
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GeAll()
        {
            var result = await _courseService.GetAllAsync();

            return CreateActionResultInstance(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _courseService.GetByIdAsync(id);

            return CreateActionResultInstance(result);
        }

        [HttpGet("getbyuserid/{userid}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var result = await _courseService.GetAllByUserId(userId);

            return CreateActionResultInstance(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var result = await _courseService.CreateAsync(courseCreateDto);

            return CreateActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var result = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(result);
        }

        [HttpPut]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _courseService.DeleteAsync(id);

            return CreateActionResultInstance(result);
        }
    }
}
