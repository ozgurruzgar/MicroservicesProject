using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<CourseViewModel > GetAllCourseByUserIdAsync(int userId);
        Task<CourseViewModel > GetByCourseIdAsync(int id);
        Task<bool> DeleteAsync(int courseId);
        Task<bool> Createsync(CourseCreateInput courseCreateInput);
        Task<bool> UpdateAsync(CourseUpdateInput courseUpdateInput);
    }
}
