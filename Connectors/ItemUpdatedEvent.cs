namespace Connectors;

public delegate void ItemUpdatedEvent<TItem> (int id, TItem item);