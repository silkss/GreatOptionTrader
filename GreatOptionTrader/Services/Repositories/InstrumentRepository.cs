using GreatOptionTrader.Models;

namespace GreatOptionTrader.Services.Repositories;
public class InstrumentRepository {
    public void Add(Instrument instrument) {
        using (var db = new GOTContext()) {
            if (db.Instruments == null) {
                return;
            }
            try {
                db.Instruments.Add(instrument);
                db.SaveChanges();
            } 
            catch (System.InvalidOperationException) {
                //TODO: Оповестить об ошибки!
            }
        }
    }
}
