using GreatOptionTrader.Models;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentRepository {
    public void Add(OptionModel instrument) {
        using (var db = new GOTContext()) {
            if (db.Options == null) {
                return;
            }
            try {
                db.Options.Add(instrument);
                db.SaveChanges();
            } 
            catch (System.InvalidOperationException) {
                //TODO: Оповестить об ошибки!
            }
        }
    }
}
