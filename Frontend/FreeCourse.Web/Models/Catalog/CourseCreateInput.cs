using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        public string UserId { get; set; }

        [Display(Name = "Kurs Kategorisi")]
        public string CategoryId { get; set; }

        [Display(Name = "Kurs ismi")]
        public string Name { get; set; }

        [Display(Name = "Kurs açıklaması")]
        public string Description { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Kurs fiyatı")]
        public decimal Price { get; set; }

        public FeatureViewModel Feature { get; set; }
        
        [Display(Name = "Kurs Resmi")]
        public IFormFile PhotoFormFile { get; set; }
    }    
}
