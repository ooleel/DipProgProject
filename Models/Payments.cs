namespace SeniorLearnWebApp.Models;

public class Payment {
    public enum PaymentMethod {
        Cash = 0, Eft = 1, Card = 2, Cheque = 3
    }
    
    public int PaymentId {get; set;} //PK
    public int MemberId {get; set;} //FK
    public PaymentMethod Method {get; set;}
    public decimal Amount {get; set;}
    public DateTime? PaymentDate {get; set;} //nullable 
	
    public Member Member {get; set;} //navigation property
}