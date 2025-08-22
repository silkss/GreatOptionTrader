using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Repositories;
using System;
using System.Windows;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для CreateInstrumentGroupView.xaml
/// </summary>
public partial class CreateInstrumentGroupView : Window {
    private readonly OptionStrategiesContainersRepository repository;

    public CreateInstrumentGroupView (OptionStrategiesContainersRepository repository) {
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
        var container = new OptionStrategiesContainer() {
            Guid = Guid.NewGuid(),
            Name = name,
            Strategies = []
        };
        repository.Create(container);
        DialogResult = true;
    }
}
