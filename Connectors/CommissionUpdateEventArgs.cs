namespace Connectors;
public class CommissionUpdateEventArgs {
    public required decimal Commission { get; init; }
    public required int PermId { get; init; }
}
