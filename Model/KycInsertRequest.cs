namespace APIGateWay.Model;

public class KycInsertRequest
{
    public KycModel Kyc { get; set; }
    public string AccountId { get; set; }
    public DateTime RecordDate { get; set; }
}