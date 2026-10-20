
namespace GreatOptionTrader;

internal class ViewModelsHolder {
    private readonly GOTContext _context;

    public ViewModels.MainMenuViewModel MainMenu { get; }
    
    public ViewModelsHolder () {
        _context = new GOTContext("great_option_trader");
        MainMenu = new ViewModels.MainMenuViewModel(_context.Connector);
    }
}
