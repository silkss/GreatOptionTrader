using Connectors.IB;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Core;
using System.ComponentModel;

namespace GreatOptionTrader.Views;
/// <summary>
/// Логика взаимодействия для RequestOptionView.xaml
/// </summary>
public partial class RequestOptionView : Window, INotifyPropertyChanged {
    private readonly InteractiveBroker broker;
    private readonly int requestId = 1;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? OptionName { get; set; }
    public string? OptionExchange { get; set; }
    public Option? SelectedOption { get; set; }

    public RequestOptionView (InteractiveBroker broker) {
        InitializeComponent();
        this.broker = broker;
        this.broker.OptionRequestedEvent += onOptionRequestEvent;

        tbOptionName.SetBinding(
            TextBox.TextProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(OptionName)),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            });

        tbOptionExchange.SetBinding(
            TextBox.TextProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(OptionExchange)),
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit
            });

        spSelectedOption.SetBinding(
            StackPanel.DataContextProperty,
            new Binding() {
                Source = this,
                Path = new PropertyPath(nameof(SelectedOption)),
                Mode = BindingMode.OneWay,
            });
    }

    private void onOptionRequestEvent (int id, Core.Option item) {
        if (id != requestId) {
            return;
        }
        if (SelectedOption?.Id != item.Id) {
            SelectedOption = item;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedOption)));
        }
    }

    private void btnRequestClicked (object sender, RoutedEventArgs e) {
        tbOptionExchange.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        tbOptionName.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        if (string.IsNullOrEmpty(OptionName)) {
            MessageBox.Show(
                "Не указано имя опциона!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrEmpty(OptionExchange)) {
            MessageBox.Show(
                "Не указана биржа опциона!",
                "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        broker.RequestOption(requestId, OptionName, OptionExchange);
    }

    private void create (object sender, RoutedEventArgs e) {
        if (SelectedOption != null) {
            DialogResult = true;
        }
    }
}
