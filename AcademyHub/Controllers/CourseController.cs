using AcademyHub.Identity;
using AcademyHub.Models;
using AcademyHub.Repositories.Interface;
using AcademyHub.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademyHub.Controllers
{
    public class CourseController:Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public CourseController(ICourseRepository courseRepository, IMapper mapper ,UserManager<ApplicationUser> userManager)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var courses =
                await _courseRepository.GetAllAsync();

            var courseViewModels =
                _mapper.Map<List<CourseListItemViewModel>>(courses);

            var instructorOptions = await GetInstructorOptionsAsync();

            var indexViewModel = new CourseIndexViewModel
            {
                Courses = courseViewModels,

                NewCourse = new CreateCourseViewModel
                {
                    Instructors = instructorOptions
                }

            };

            return View(indexViewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if(course == null) 
              return  NotFound();
            var courseViewModel = _mapper.Map<CourseDetailsViewModel>(course);
            return View(courseViewModel);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
          [Bind(Prefix = "NewCourse")] CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var instructorExists =
                    await _courseRepository.InstructorExistsAsync(model.InstructorId);

                if (!instructorExists)
                {
                    ModelState.AddModelError(
                        "NewCourse.InstructorId",
                        "المحاضر المختار غير موجود."
                    );
                }
            }
          
            if (!ModelState.IsValid)
            {
                var courses = await _courseRepository.GetAllAsync();
                var courseViewModels =  _mapper.Map<List<CourseListItemViewModel>>(courses);
                model.Instructors = await GetInstructorOptionsAsync();
                var indexViewModel = new CourseIndexViewModel
                {
                    Courses = courseViewModels,
                    NewCourse = model                    
                     

                };

                return View("Index",indexViewModel);
            }
            var userId = _userManager.GetUserId(User);
            if(string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }
            var course = _mapper.Map<Course>(model);
            course.CreatedByUserId = userId;
            await _courseRepository.AddAsync(course);
          await  _courseRepository.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course created successfully.";
            return RedirectToAction("Index");

        }
        [Authorize]
        public async Task<IActionResult> Edit(int id )
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }

            if (course.CreatedByUserId != userId)
            {
                return Forbid();
            }

            var editViewModel = _mapper.Map<EditCourseViewModel>(course);
            editViewModel.Instructors = await GetInstructorOptionsAsync();

            return View(editViewModel);



        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, EditCourseViewModel model)
        {
            if (id != model.CourseId)
            {
                return BadRequest();
            }
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }
            if(course.CreatedByUserId != userId)
            {
                return Forbid();
            }
     
            if (ModelState.IsValid)
            {
                var instructorExists =
                    await _courseRepository.InstructorExistsAsync(model.InstructorId);

                if (!instructorExists)
                {
                    ModelState.AddModelError(
                        "InstructorId",
                        "المحاضر المختار غير موجود."
                    );
                }
            }
            if (!ModelState.IsValid)
            {
                model.Instructors = await GetInstructorOptionsAsync();
                return View(model);
            }
            course.Title = model.Title;
            course.InstructorId = model.InstructorId;
            await _courseRepository.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course updated successfully.";
            return RedirectToAction("Index");


        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }
            if (course.CreatedByUserId != userId)
            {
                return Forbid();
            }
            var deleteViewModel = _mapper.Map<CourseDetailsViewModel>(course);
            return View(deleteViewModel);
        }
        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Challenge();
            }
            if(course.CreatedByUserId != userId)
            {
                return Forbid();
            }
            if(course.Enrollments != null && course.Enrollments.Any())
            {
                ModelState.AddModelError(string.Empty, "لا يمكن حذف الدورة لأنها تحتوي على تسجيلات.");
                var deleteViewModel = _mapper.Map<CourseDetailsViewModel>(course);
                return View("Delete", deleteViewModel);
            } 
                _courseRepository.Remove(course);
                await _courseRepository.SaveChangesAsync();
            TempData["SuccessMessage"] = "Course deleted successfully.";
            return RedirectToAction("Index");

        }
        private async Task<List<SelectListItem>> GetInstructorOptionsAsync()
        {
            // اجلب المحاضرين.
            var instructors = await _courseRepository.GetAllInstructorsAsync();
           
            var instructorOptions = instructors
                .Select(instructor => new SelectListItem
                {
                    Text = instructor.Name,
                    Value = instructor.Id
                })
                .ToList();
            return instructorOptions;
        }


    }
}
