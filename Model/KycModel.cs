namespace APIGateWay.Model;

public class KycModel
{
    public string BRCode { get; set; }
    public string BankAccountNumber { get; set; }
    public string BankCode { get; set; }
    public string BankName { get; set; }
    public string BirthCountry { get; set; }
    public string BranchCode { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PostalCode { get; set; }
    public string ProposalNumber { get; set; }
    public string FormUrl { get; set; }   // <-- New property added here

}