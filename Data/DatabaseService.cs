using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearnWebApp.Models;

namespace SeniorLearnWebApp.Data;

public class DatabaseService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public DatabaseService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task ResetDatabaseAsync()
    {
        // WARNING: Deletes and recreates DB
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        // Optional: Add additional seeding here
        /*if (!await _context.Members.AnyAsync())
        {
            _context.Members.AddRange(
                new Member
                {
                    FirstName = "John",
                    LastName = "Pork",
                    Phone = "000",
                    Email = "jsmith@example.com",
                    DateOfBirth = new DateTime(2020, 1, 1)
                },
                new Member {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Phone = "000",
                    Email = "jdoe@example.com",
                    DateOfBirth = new DateTime(2020, 1, 1)
                }
            );
            await _context.SaveChangesAsync();
        }

        if (!await _context.MemberRoles.AnyAsync())
        {
            _context.MemberRoles.AddRange(
                new MemberRole
                {
                    MemberId = 1,
                    Role = MemberRole.MemberRoleType.Professional,
                    StartDate = new DateTime(2021, 1, 1),
                    EndDate = new DateTime(2021, 2, 1),
                },
                new MemberRole
                {
                    MemberId = 2,
                    Role = MemberRole.MemberRoleType.Standard,
                    StartDate = new DateTime(2021, 1, 1),
                    EndDate = new DateTime(2021, 2, 1),
                });
            await _context.SaveChangesAsync();
        }

        if (!await _context.DeliveryPatterns.AnyAsync())
        {
         _context.DeliveryPatterns.AddRange(
             new DeliveryPattern
             {
                 Type = DeliveryPattern.DeliveryPatternType.Recurring,
                 Frequency = "Weekly",
                 StartDate = new DateTime(),
                 EndDate = new DateTime(+100),
                 Occurrences = 1
             });
         await _context.SaveChangesAsync();
        }

        if (!await _context.Lessons.AnyAsync())
        {
            _context.Lessons.AddRange(
                new Lesson
                {
                    Title = "Lesson 1",
                    Status = Lesson.LessonStatus.Scheduled,
                    InstructorId = 1,
                    Location = "Online",
                    DeliveryMode = Lesson.LessonDeliveryMode.Online,
                    Capacity = 10,
                    Description = "Description 1",
                    Start = new DateTime(),
                    DurationMinutes = 60
                },
                new Lesson
                {
                    Title = "Lesson 2",
                    Status = Lesson.LessonStatus.Scheduled,
                    InstructorId = 1,
                    Location = "Online",
                    DeliveryMode = Lesson.LessonDeliveryMode.Online,
                    Capacity = 10,
                    Description = "Description 2",
                    Start = new DateTime(),
                    DurationMinutes = 60,
                    DeliveryPatternId = 1
                });
            await _context.SaveChangesAsync();
        }*/
    }

    public async Task SeedDatabaseAsync(String firstname, String lastname, String email, String password,
        String roletype)
    {
        var user = new Member
        {
            FirstName = firstname,
            LastName = lastname,
            Email = email,
            Phone = "000",
            DateOfBirth = new DateTime(2000, 01, 01)
        };

        _context.Members.Add(user);
        await _context.SaveChangesAsync();

        if (roletype == "Professional")
        {
            var role = new MemberRole
            {
                MemberId = user.MemberId,
                Role = MemberRole.MemberRoleType.Professional,
                StartDate = DateTime.UtcNow
            };
            _context.MemberRoles.Add(role);
            await _context.SaveChangesAsync();
            var member = new ApplicationUser
            {
                UserName = email,
                Email = email,
                MemberId = user.MemberId
            };

            await _userManager.CreateAsync(member, password);
            await _userManager.AddToRoleAsync(member, "Professional");

            var lesson = new Lesson
            {
                Title = "Lesson 1",
                Status = Lesson.LessonStatus.Scheduled,
                InstructorId = user.MemberId,
                Location = "Online",
                DeliveryMode = Lesson.LessonDeliveryMode.Online,
                Capacity = 10,
                Description = "Lesson 1 Description",
                Start = new DateTime(),
                DurationMinutes = 60,
            };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
        }
        else if (roletype == "Honorary")
        {
            var role = new MemberRole
            {
                MemberId = user.MemberId,
                Role = MemberRole.MemberRoleType.Honorary,
                StartDate = DateTime.UtcNow
            };
            _context.MemberRoles.Add(role);
            await _context.SaveChangesAsync();
            var member = new ApplicationUser
            {
                UserName = email,
                Email = email,
                MemberId = user.MemberId
            };

            await _userManager.CreateAsync(member, password);
            await _userManager.AddToRoleAsync(member, "Honorary");
        }
        else
        {
            {
                var role = new MemberRole
                {
                    MemberId = user.MemberId,
                    Role = MemberRole.MemberRoleType.Standard,
                    StartDate = DateTime.UtcNow
                };
                _context.MemberRoles.Add(role);
                await _context.SaveChangesAsync();
                var member = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    MemberId = user.MemberId
                };

                await _userManager.CreateAsync(member, password);
                await _userManager.AddToRoleAsync(member, "Standard");
            }
        }
    }
}