namespace GreatOptionTrader.EventArguments;
public class CommissionUpdateEventArgs {
    public required decimal Commission { get; init; }
    public required int PermId { get; init; }
}
