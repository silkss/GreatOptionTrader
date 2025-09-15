using Connectors.IB;
using Core;
using GreatOptionTrader.Models;
using GreatOptionTrader.Services.Repositories;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для CreateInstrumentGroupView.xaml
/// </summary>
public partial class CreateStrategyContainerView : Window, INotifyPropertyChanged {
    private readonly InteractiveBroker broker;
    private readonly OptionStrategiesContainersRepository repository;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Option? SelectedOption { get; set; }
    public string? SelectedAccount { get; set; }
    public ContainerSettings Settings { get; }

    public CreateStrategyContainerView (InteractiveBroker broker, OptionStrategiesContainersRepository repository) {
        InitializeComponent();
        this.Settings = new ContainerSettings() { CurrencyTargetPnL = 1000m };
        this.broker = broker;
        this.repository = repository;
        this.cbAccounts.ItemsSource = broker.Accounts;

        cbAccounts.SetBinding(
            ComboBox.SelectedItemProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(SelectedAccount)),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            });

        labSelectedOption.SetBinding(
            Label.ContentProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(SelectedOption)),
                Mode = BindingMode.OneWay,
            });

        tbTargetPnL.SetBinding(
            TextBox.TextProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(Settings.CurrencyTargetPnL)),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.LostFocus
            });
    }

    private void btnCreateClicked (object sender, RoutedEventArgs e) {
        cbAccounts.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();

        if (string.IsNullOrEmpty(tbName.Text)) {
            MessageBox.Show(
                "Не указано имя!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }
        
        if (string.IsNullOrEmpty(SelectedAccount)) {
            MessageBox.Show(
                "Не выбран аккаунт!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        if (SelectedOption == null) {
            MessageBox.Show(
                "Не выбран Master Option!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        var name = tbName.Text.Replace('\n', ' ');
        var container = new OptionStrategiesContainer() {
            Guid = Guid.NewGuid(),
            Name = name,
            Strategies = [],
            Instrument = SelectedOption,
            Orders = [],
            Account = SelectedAccount,
            Settings = Settings
        }; 

        repository.Create(container);
        DialogResult = true;
    }

    private void requestclicked (object sender, RoutedEventArgs e) {
        var dialog = new RequestOptionView(broker);
        if (dialog.ShowDialog() == true) {
            if (SelectedOption?.Id != dialog.SelectedOption?.Id) {
                SelectedOption = dialog.SelectedOption;
                PropertyChanged?.Invoke(this, new(nameof(SelectedOption)));
            }
        }
    }
}
