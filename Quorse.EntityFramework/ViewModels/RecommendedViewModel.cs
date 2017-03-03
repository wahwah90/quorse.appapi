using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.EntityFramework.ViewModels
{
    public class RecommendedViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public decimal? CoursePrice { get; set; }  
        public string CourseEntityName { get; set; }     
        public int? CourseRating { get; set; }
        public string CourseImage { get; set; }
        public bool CourseHasPublicClass { get; set; }
        public bool CourseHasLVCClass { get; set; }
        public bool CourseHasELearningClass { get; set; }
        public DateTime? CourseClassTTEndDate { get; set; }
        public ClassSimpleModel ClassSimpleModel { get; set; }
    }
    public class ClassSimpleModel
    {
        public decimal? CourseHighestClassPrice { get; set; }
        public decimal? CourseLowestClassPrice { get; set; }
        public decimal? CourseDiscountRate { get;set; }
    }
}
