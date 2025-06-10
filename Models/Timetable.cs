namespace SeniorLearnWebApp.Models;

public class Timetable {
    public int TimetableId {get; set;} //PK
    public int LessonId {get; set;} //FK
    public DateTime DateTime {get; set;} 
	
    public Lesson Lesson {get; set;}
}