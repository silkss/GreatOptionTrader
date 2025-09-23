using Core.Types;
using System;

namespace Hedger.Models;

public class Container {
    public required Guid Id { get; init; }
    public required Future Basis { get; init; }
    public required string Account { get; set; }
}
