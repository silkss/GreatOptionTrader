using Hedger.Models;
using Hedger.ViewModels;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace Hedger.Views;
/// <summary>
/// Логика взаимодействия для CreateHedgeContainerView.xaml
/// </summary>
public partial class EditHedgeContainerView : Window {
    public EditHedgeContainerView () {
        InitializeComponent();
    }

    private void update (object sender, RoutedEventArgs e) {
        dgSellHedgeLevels.GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateSource();
        dgBuyHedgeLevels.GetBindingExpression(DataGrid.ItemsSourceProperty).UpdateSource();
        DialogResult = true;
    }
}
