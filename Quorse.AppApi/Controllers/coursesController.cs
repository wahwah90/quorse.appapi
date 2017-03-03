using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Quorse.EntityFramework.EntityFramework.Schema;
using Quorse.AppApi.BL.Interfaces;
using Quorse.AppApi.BL.Services;
using Quorse.EntityFramework.ViewModels;
using System.Web.Http.Cors;

namespace Quorse.AppApi.Controllers
{
    public class coursesController : ApiController
    {
       private readonly ICourseService _CourseService;
        public coursesController(ICourseService courseService)
        {
            _CourseService = courseService;
        }
        public IQueryable<course> Getcourses()
        {
            var courses = _CourseService.Courses();
            return _CourseService.Courses();
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Getcourse(int id)
        {
            course course = await _CourseService.CoursesAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcourse(course course)
        {
            int courseId = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                courseId = await _CourseService.UpdateCourseAsync(course);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (courseId <= 0)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Postcourse(course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _CourseService.AddCourseAsync(course);

            return CreatedAtRoute("DefaultApi", new { id = course.id }, course);
        }

        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Deletecourse(int id)
        {
            course course = await _CourseService.DeleteCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }
         
            return Ok(course);
        }

        //[HttpGet]
        //[Authorize(Roles = "Administrators")]
        public IQueryable<RecommendedViewModel> GetTimeTickers()
        {
            return _CourseService.GetTimeTickerRecommended(1, 5, 14);
        }

        public IQueryable<RecommendedViewModel> GetLatest()
        {
            return _CourseService.GetLatestRecommended(1, 5, 14);
        }

        public IQueryable<RecommendedViewModel> GetPopular()
        {
            return _CourseService.GetPopularRecommended(1, 5, 14);
        }
    }
}