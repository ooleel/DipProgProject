namespace SeniorLearnWebApp.Models;
public class Member {
    public int MemberId {get; set;} //PK
    public string FirstName {get; set;} = null!; //= non-nullable
    public string LastName {get; set;} = null!;
    public string Email {get; set;} = null!;
    public string Phone {get; set;} = null!; //!! not Int
    public DateTime DateOfBirth {get; set;}
    
    public ApplicationUser? User { get; set; } // Navigation property (optional)

    //1-* relationships
    public ICollection<Payment> Payments {get; set;}
    public ICollection<MemberRole> MemberRoles {get; set;}
    public ICollection<Enrolment> Enrolments {get; set;}

    public ICollection<Lesson> LessonsTaught {get; set;} //navigation
    //when mb = Instructor 
    //when all lessons are taught by this particular member
}