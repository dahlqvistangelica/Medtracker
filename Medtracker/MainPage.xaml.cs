using Medtracker.ViewModels;

namespace Medtracker;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Detta ser till att siffrorna uppdateras varje gång du visar sidan
        _viewModel.LoadData();
    }
}