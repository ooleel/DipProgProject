namespace SeniorLearnWebApp.Models;

public class Lesson {
	public enum LessonStatus {
		Draft = 0, Scheduled = 1, Closed = 2, Complete = 3, Cancelled = 4
	}

	public enum LessonDeliveryMode {
		Online = 0, OnPremises = 1
	}
	
    public int LessonId {get; set;} //PK
    public string Title {get; set;} = null!;
    public LessonStatus Status {get; set;}
	
    public int InstructorId {get; set;} //FK
    public Member Instructor {get; set;} //nav
	
    public string Location {get; set;} = null!;
    public LessonDeliveryMode DeliveryMode {get; set;}
    public int Capacity {get; set;}
    public string? Description {get; set;}
    public DateTime Start {get; set;}
    public int DurationMinutes {get; set;}
	
    public int? DeliveryPatternId {get; set;} //FK
    public DeliveryPattern? DeliveryPattern {get; set;} 
    //nullable because not every lesson needs to be part of a pattern, so both the FK and nav prop (the relationship) are nullable
	
    public ICollection<Enrolment> Enrolments {get; set;} 
    public ICollection<Timetable> Timetables {get; set;}
}