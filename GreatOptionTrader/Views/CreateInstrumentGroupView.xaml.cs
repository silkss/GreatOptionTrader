using GreatOptionTrader.Services.Repositories;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для CreateInstrumentGroupView.xaml
/// </summary>
public partial class CreateInstrumentGroupView : Window {
    private readonly InstrumentGroupRepo repository;

    public CreateInstrumentGroupView (InstrumentGroupRepo repository) {
        InitializeComponent();
        this.repository = repository;
    }

    private void btnCreateClicked (object sender, RoutedEventArgs e) {
        if (string.IsNullOrEmpty(tbName.Text)) {
            MessageBox.Show(
                "Не указано имя!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var name = tbName.Text.Replace('\n', ' ');

        repository.CreateByName(name);
        DialogResult = true;
    }
}
