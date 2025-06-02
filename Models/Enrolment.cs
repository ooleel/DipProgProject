namespace SeniorLearnWebApp.Models;

public class Enrolment {
    public int MemberId {get; set;} //PK FK
    public int LessonId {get; set;} //PK FK
    public bool HasAttended {get; set;} 
	
    public Member Member {get; set;}
    public Lesson Lesson {get; set;}
}