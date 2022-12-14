namespace CosmosDbPoC;

public class CosmosDbOptions
{
    public const string CosmosDb = "CosmosDb";
    
    public string Endpoint { get; init; }
    public string PrimaryKey { get; init; }
    public string Database { get; set; }
    public string Container { get; set; }
    public string LeaseContainer { get; set; }
    public string PartitionKey { get; set; }
}