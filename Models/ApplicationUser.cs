using Microsoft.AspNetCore.Identity;

namespace SeniorLearnWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? MemberId { get; set; }
        public Member? Member { get; set; }
    }
}