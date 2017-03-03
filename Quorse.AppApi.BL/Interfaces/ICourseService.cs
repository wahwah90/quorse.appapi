using Quorse.AppApi.BL.Interfaces.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.BL.Interfaces
{
    public interface ICourseService:IBaseService
    {
        IQueryable<course> Courses();
        Task<course> CoursesAsync(int id);
        Task<int> UpdateCourseAsync(course course);
        Task<course> DeleteCourseAsync(int key);
        Task<int> AddCourseAsync(course course);
        IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range);
        IQueryable<RecommendedViewModel> GetLatestRecommended(int start, int size, int range);
        IQueryable<RecommendedViewModel> GetPopularRecommended(int start, int size, int range);
    }
}
