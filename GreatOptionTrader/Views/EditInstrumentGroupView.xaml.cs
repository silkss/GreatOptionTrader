using Connectors.IB;
using Core;
using GreatOptionTrader.ViewModels;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для EditInstrumentGroupView.xaml
/// </summary>
public partial class EditInstrumentGroupView : Window {
    private readonly OptionStrategyContainerViewModel container;
    private readonly InteractiveBroker broker;

    public EditInstrumentGroupView (
        OptionStrategyContainerViewModel container, 
        InteractiveBroker broker) {
        InitializeComponent();
        this.container = container;
        this.broker = broker;
        spContainer.DataContext = container;
    }
}
