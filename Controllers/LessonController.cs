using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearnWebApp.Data;
using SeniorLearnWebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SeniorLearnWebApp.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.MemberId == null)
                return Unauthorized();

            var memberId = user.MemberId.Value;

            // Get member's current role
            var role = await _context.MemberRoles
                .Where(r => r.MemberId == memberId && r.EndDate == null)
                .OrderByDescending(r => r.StartDate)
                .FirstOrDefaultAsync();

            var isProfessional = role != null && role.Role == MemberRole.MemberRoleType.Professional;

            var lessons = await _context.Lessons
                .Include(l => l.DeliveryPattern)
                .Include(l => l.Instructor)
                .ToListAsync();

            ViewBag.IsProfessional = isProfessional;

            return View(lessons);
        }

        [HttpGet]
        [Authorize(Policy = "Professional")]
        public IActionResult Create()
        {
            // Fetch instructors
            // TODO Change this so it gets based on Role
            var instructors = _context.Members
                .Select(m => new SelectListItem
                {
                    Value = m.MemberId.ToString(),
                    Text = m.FirstName + " " + m.LastName
                })
                .ToList();
            ViewBag.Instructors = instructors;

            // Fetch delivery patterns
            var patterns = _context.DeliveryPatterns
                .Select(dp => new SelectListItem
                {
                    Value = dp.DeliveryPatternId.ToString(),
                    Text = dp.Type.ToString()
                })
                .ToList();
            ViewBag.DeliveryPatterns = patterns;

            return View();
        }


        [HttpPost]
        [Authorize(Policy = "Professional")]
        public async Task<IActionResult> Create(IFormCollection form)
        {
            var title = form["Title"];
            var status = Enum.TryParse<Lesson.LessonStatus>(form["Status"], out var parsedStatus) ? parsedStatus : Lesson.LessonStatus.Draft;
            var instructorId = int.TryParse(form["InstructorId"], out var iid) ? iid : 0;
            var location = form["Location"];
            var deliveryMode = Enum.TryParse<Lesson.LessonDeliveryMode>(form["DeliveryMode"], out var parsedMode) ? parsedMode : Lesson.LessonDeliveryMode.OnPremises;
            var capacity = int.TryParse(form["Capacity"], out var parsedCap) ? parsedCap : 0;
            var description = form["Description"];
            var duration = int.TryParse(form["DurationMinutes"], out var parsedDuration) ? parsedDuration : 0;
            var start = DateTime.TryParse(form["Start"], out var parsedStart) ? parsedStart : DateTime.Now;
            var deliveryPatternId = int.TryParse(form["DeliveryPatternId"], out var dpId) ? dpId : (int?)null;

            var lesson = new Lesson
            {
                Title = title,
                Status = status,
                InstructorId = instructorId,
                Location = location,
                DeliveryMode = deliveryMode,
                Capacity = capacity,
                Description = description,
                Start = parsedStart,
                DurationMinutes = duration,
                DeliveryPatternId = deliveryPatternId
            };

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}