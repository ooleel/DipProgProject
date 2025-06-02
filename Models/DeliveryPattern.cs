namespace SeniorLearnWebApp.Models;

public class DeliveryPattern {
    public enum DeliveryPatternType {
        Standalone = 0, Recurring = 1, Course = 2
    }
    
    public int DeliveryPatternId {get; set;} //PK
    public DeliveryPatternType Type {get; set;} 
    public string Frequency {get; set;} = null!;
    //Why non-nullable? Freq might be nullable, as custom dates = no regular freq
    public DateTime StartDate {get; set;} 
    public DateTime EndDate {get; set;}
    public int Occurrences {get; set;}
	
    public ICollection<Lesson> Lessons {get; set;}
}