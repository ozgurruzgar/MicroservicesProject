namespace FreeCourse.Shared.Messages
{
    public class PublishCourseNameChangedEvent
    {
        public string CourseId { get; set; }
        public string UpdatedName { get; set; }
    }
}
