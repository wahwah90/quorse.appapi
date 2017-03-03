using Quorse.AppApi.DAL.Interfaces.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quorse.AppApi.DAL.Interfaces
{
    public interface ICourseRepository:IBaseRepository<course,int>
    {
        IQueryable<course> GetAll();
        Task<course> GetByKeyAsync(int id);
        Task<int> SaveAsync(course course); 
        Task<course> DeleteAsync(int key);
        IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range);
        IQueryable<RecommendedViewModel> GetLatestRecommended(int start, int size, int range);
        IQueryable<RecommendedViewModel> GetPopularRecommended(int start, int size, int range);
    }
}
