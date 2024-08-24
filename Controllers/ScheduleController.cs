using Faculty_M.Models;
using Faculty_M.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Faculty_M.Controllers
{
    public class ScheduleController : Controller
    {

        private readonly FacultyDbContext facultyDbContext;
        public ScheduleController(FacultyDbContext Context)
        {
            facultyDbContext = Context;
        }
        public IActionResult Index()
        {
            ICollection<Schedule> schedules = facultyDbContext.Schedules.ToList();
            return View(schedules);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                var validDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                if (!validDays.Contains(schedule.DayOfWeek, StringComparer.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("DayOfWeek", "Invalid day of the week.");
                    return View(schedule);
                }

                var exists = facultyDbContext.Schedules.Any(s => s.SchId == schedule.SchId);
                if (exists)
                {
                    ModelState.AddModelError("SchId", "Schedule ID already exists.");
                    return View(schedule);
                }

                if (schedule.StartTime >= schedule.EndTime)
                {
                    ModelState.AddModelError("EndTime", "End time must be later than start time.");
                    return View(schedule);
                }


                if (schedule.CrsId.HasValue)
                {
                    var crsExists = facultyDbContext.Courses.Any(d => d.CrsId == schedule.CrsId);
                    if (!crsExists)
                    {
                        ModelState.AddModelError("CrsId", "The selected Course ID does not exist.");
                        return View(schedule);
                    }
                }


                if (schedule.InsId.HasValue)
                {
                    var insExists = facultyDbContext.Instructors.Any(i => i.InsId == schedule.InsId);
                    if (!insExists)
                    {
                        ModelState.AddModelError("InsId", "The selected Instructor ID does not exist.");
                        return View(schedule);
                    }
                }

                facultyDbContext.Add(schedule);
                await facultyDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var schedule = facultyDbContext.Schedules.Where(s => s.SchId == id).FirstOrDefault();
            return View(schedule);
        }
        [HttpPost]
        public IActionResult Edit(Schedule schedule)
        {
            var validDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            if (!validDays.Contains(schedule.DayOfWeek, StringComparer.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("DayOfWeek", "Invalid day of the week.");
                return View(schedule);
            }

            if (schedule.StartTime >= schedule.EndTime)
            {
                ModelState.AddModelError("EndTime", "End time must be later than start time.");
                return View(schedule);
            }

            if (!ModelState.IsValid)
            {
                return View(schedule);
            }


            if (schedule.CrsId.HasValue)
            {
                var crsExists = facultyDbContext.Courses.Any(d => d.CrsId == schedule.CrsId);
                if (!crsExists)
                {
                    ModelState.AddModelError("CrsId", "The selected Course ID does not exist.");
                    return View(schedule);
                }
            }


            if (schedule.InsId.HasValue)
            {
                var insExists = facultyDbContext.Instructors.Any(i => i.InsId == schedule.InsId);
                if (!insExists)
                {
                    ModelState.AddModelError("InsId", "The selected Instructor ID does not exist.");
                    return View(schedule);
                }
            }

            var currentSchedule = facultyDbContext.Schedules.FirstOrDefault(s => s.SchId == schedule.SchId);
            if (currentSchedule == null)
            {
                return NotFound();
            }

            currentSchedule.DayOfWeek = schedule.DayOfWeek;
            currentSchedule.StartTime = schedule.StartTime;
            currentSchedule.EndTime = schedule.EndTime;
            currentSchedule.Location = schedule.Location;
            currentSchedule.CrsId = schedule.CrsId;
            currentSchedule.InsId = schedule.InsId;

            facultyDbContext.SaveChanges();
            return RedirectToAction("Index");
        }



        public IActionResult Details(int id)
        {
            var schedule = facultyDbContext.Schedules
                .Include(s => s.Ins)
                .Include(s => s.Crs)
                .FirstOrDefault(s => s.SchId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            var scheduleVM = new ScheduleVM
            {
                ScheduleId = schedule.SchId,
                InsName = schedule.Ins != null ? schedule.Ins.InsName : "",
                CrsName = schedule.Crs != null ? schedule.Crs.CrsName : "",
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Location = schedule.Location
            };

            return View(scheduleVM);
        }

        public IActionResult Delete(int id)
        {
            var currentschedule = facultyDbContext.Schedules
                .Where(s => s.SchId == id)
                .FirstOrDefault();
            facultyDbContext.Schedules.Remove(currentschedule);
            facultyDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}