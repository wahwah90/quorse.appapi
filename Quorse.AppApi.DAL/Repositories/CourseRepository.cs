using Quorse.AppApi.DAL.Interfaces;
using Quorse.AppApi.DAL.Repositories.Base;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.EntityFramework.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Quorse.AppApi.Helper.ClassDetailTypeConverter;

namespace Quorse.AppApi.DAL.Repositories
{
    public class CourseRepository:BaseFuncRepository,ICourseRepository
    {
        private QuorseDbEntities db = new QuorseDbEntities();
        public IQueryable<course> GetAll()
        {
            return db.courses;
        }

        public async Task<course> GetByKeyAsync(int id)
        {
            course course = await db.courses.FindAsync(id);
            return course;
        }

        public async Task<int> SaveAsync(course course)
        {
            if (course.id == 0)
            {
                db.courses.Add(course);
            }
            else
            {
                course dbEntry = await db.courses.FindAsync(course.id);
                if (dbEntry != null)
                {
                    dbEntry.id = course.id;
                    dbEntry.title = course.title;
                }
            }
            return await db.SaveChangesAsync();
        }
        public async Task<course> DeleteAsync(int courseID)
        {
            course dbEntry = db.courses.Find(courseID);
            if (dbEntry != null)
            {
                db.courses.Remove(dbEntry);
            }
            await db.SaveChangesAsync();
            return dbEntry;
        }

        public IQueryable<RecommendedViewModel> GetTimeTickerRecommended(int start, int size, int range)
        {

            var dateRange = DateTime.Now.AddDays(range); //show time ticker detail within date range
            var usdExchangeRate = db.currencies.OrderByDescending(c => c.CurrencyID).FirstOrDefault().CurrencyRate;
            var timeTickerCourses = db.courses.Include(c => c.classDetails)
                .Where(c => c.classDetails.Any(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange))
                .Select(c => new RecommendedViewModel
                {
                    CourseId = c.id,
                    CourseTitle = (c.title.Length>60)?c.title.Substring(0,60)+"...":c.title,
                    CourseImage = "https://quorse.s3.amazonaws.com/pub/course/course/"+c.bgLarge,
                    CourseEntityName = db.entities
                                            .Where(e => e.guid == c.entityguid)
                                            .FirstOrDefault().name,
                    CoursePrice = (c.isusd == true) ? c.courseprice * usdExchangeRate : c.courseprice,
                    CourseRating = c.ratingtotal,
                    CourseClassTTEndDate = c.classDetails
                        .Where(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange)
                        .FirstOrDefault()
                        .ttenddate,
                    ClassSimpleModel = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                               )
                        .OrderBy(d => d.ttprice)
                        .Select(d => new ClassSimpleModel
                        {
                            CourseLowestClassPrice = (d.isusd == true) ? d.ttprice * usdExchangeRate : d.ttprice, //since this is time ticker,so ttprice must be lowest
                            CourseHighestClassPrice = (d.isusd == true) ? d.price * usdExchangeRate : d.price,
                            CourseDiscountRate = (d.isusd == true) ? (((d.price * usdExchangeRate)-(d.ttprice * usdExchangeRate))/ (d.price * usdExchangeRate))*100:((d.price-d.ttprice)/d.price)*100
                        }).FirstOrDefault()
                        ,
                    CourseHasELearningClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                            && d.type == (int)ClassDetailType.ELEARNING)
                        .Count() > 0,
                    CourseHasLVCClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.type == (int)ClassDetailType.LIVEVIRTUAL)
                        .Count() > 0,
                    CourseHasPublicClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.type == (int)ClassDetailType.PUBLIC)
                        .Count() > 0
                })
                .OrderBy(c => c.CourseId)
                .Skip(start)
                .Take(size);

            return timeTickerCourses;
        }
        public IQueryable<RecommendedViewModel> GetLatestRecommended(int start, int size, int range)
        {

            var dateRange = DateTime.Now.AddDays(range); //show time ticker detail within date range
            var usdExchangeRate = db.currencies.OrderByDescending(c => c.CurrencyID).FirstOrDefault().CurrencyRate;
            var timeTickerCourses = db.courses.Include(c => c.classDetails)
                .OrderByDescending(c => c.id)
                .Select(c => new RecommendedViewModel
                {
                    CourseId = c.id,
                    CourseTitle = c.title,
                    CourseImage = c.bgLarge,
                    CourseEntityName = db.entities
                                            .Where(e => e.guid == c.entityguid)
                                            .FirstOrDefault().name,
                    CoursePrice = (c.isusd == true) ? c.courseprice * usdExchangeRate : c.courseprice,
                    CourseRating = c.ratingtotal,
                    CourseClassTTEndDate = c.classDetails
                        .Where(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange)
                        .FirstOrDefault()
                        .ttenddate,
                    ClassSimpleModel = c.classDetails
                        .Where(d => d.status == 1)
                        .OrderBy(d => d.ttprice)
                        .Select(d => new ClassSimpleModel
                        { 
                            CourseLowestClassPrice = (d.isusd == true&&d.istimeticker == true) ? d.ttprice * usdExchangeRate : d.ttprice, //since this is time ticker,so ttprice must be lowest
                            CourseHighestClassPrice = (d.isusd == true) ? d.price * usdExchangeRate : d.price
                        }).FirstOrDefault()
                        ,
                    CourseHasELearningClass = c.classDetails
                        .Where(d => d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                            && d.type == (int)ClassDetailType.ELEARNING)
                        .Count() > 0,
                    CourseHasLVCClass = c.classDetails
                        .Where(d => d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                                    && d.type == (int)ClassDetailType.LIVEVIRTUAL)
                        .Count() > 0,
                    CourseHasPublicClass = c.classDetails
                        .Where(d => d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                                    && d.type == (int)ClassDetailType.PUBLIC)
                        .Count() > 0
                })                          
                .Skip(start)
                .Take(size);

            return timeTickerCourses;
        }
        public IQueryable<RecommendedViewModel> GetPopularRecommended(int start, int size, int range)
        {

            var dateRange = DateTime.Now.AddDays(range); //show time ticker detail within date range
            var usdExchangeRate = db.currencies.OrderByDescending(c => c.CurrencyID).FirstOrDefault().CurrencyRate;
            var timeTickerCourses = db.courses.Include(c => c.classDetails)
                .OrderByDescending(c => c.ratingtotal)
                .Select(c => new RecommendedViewModel
                {
                    CourseId = c.id,
                    CourseTitle = c.title,
                    CourseImage = c.bgLarge,
                    CourseEntityName = db.entities
                                            .Where(e => e.guid == c.entityguid)
                                            .FirstOrDefault().name,
                    CoursePrice = (c.isusd == true) ? c.courseprice * usdExchangeRate : c.courseprice,
                    CourseRating = c.ratingtotal,
                    CourseClassTTEndDate = c.classDetails
                        .Where(d => d.istimeticker == true
                                && d.status == 1
                                && DateTime.Now >= d.ttstartdate
                                && d.ttenddate >= DateTime.Now
                                && d.ttenddate < dateRange)
                        .FirstOrDefault()
                        .ttenddate,
                    ClassSimpleModel = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange)
                        .OrderBy(d => d.ttprice)
                        .Select(d => new ClassSimpleModel
                        {
                            CourseLowestClassPrice = (d.isusd == true) ? d.ttprice * usdExchangeRate : d.ttprice, //since this is time ticker,so ttprice must be lowest
                            CourseHighestClassPrice = (d.isusd == true) ? d.price * usdExchangeRate : d.price
                        }).FirstOrDefault()
                        ,
                    CourseHasELearningClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                            && d.type == (int)ClassDetailType.ELEARNING)
                        .Count() > 0,
                    CourseHasLVCClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                                    && d.type == (int)ClassDetailType.LIVEVIRTUAL)
                        .Count() > 0,
                    CourseHasPublicClass = c.classDetails
                        .Where(d => d.istimeticker == true
                                    && d.status == 1
                                    && DateTime.Now >= d.ttstartdate
                                    && d.ttenddate >= DateTime.Now
                                    && d.ttenddate < dateRange
                                    && d.type == (int)ClassDetailType.PUBLIC)
                        .Count() > 0
                })
                .Skip(start)
                .Take(size);

            return timeTickerCourses;
        }
    }
}
