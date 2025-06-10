namespace SeniorLearnWebApp.Models;

public class MemberRole {
    public enum MemberRoleType {
        Standard = 1, Professional = 2, Honorary = 3
    }
    
    public int MemberRoleId {get; set;} //PK
    public int MemberId {get; set;} //FK
    public MemberRoleType Role {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime? EndDate {get; set;} //nullable 
	
    public Member Member {get; set;} 
    
}