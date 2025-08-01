using GreatOptionTrader.Models;

namespace GreatOptionTrader.Services.Repositories;
public class OrdersRepository {
    public void Add (Core.Order order) {
        using (var db = new GOTContext()) {
            if (db.Orders == null) {
                return;
            }
            try {
                db.Orders.Add(order);
                db.SaveChanges();
            }
            catch (System.InvalidOperationException) {
                //TODO: Оповестить об ошибки!
            }
        }
    }

    public void Update (Core.Order order) {
        using (var db = new GOTContext()) {
            db.Orders?.Update(order);
            db.SaveChanges();
        }
    }
}
