using Medtracker.ViewModels;
namespace Medtracker;

public partial class MainPage : ContentPage
{

	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

		viewModel.LoadData();
	}
	private async void OnAddMedicationClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(AddMedicationPage));
	}
	private async void OnShowMedicationClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(ShowMedicationPage));
	}
	private async void OnRemoveMedicationClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(RemoveMedicationPage));
	}

	
}
