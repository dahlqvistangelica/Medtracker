using Medtracker.ViewModels;
using Microsoft.Maui.Controls;
namespace Medtracker.Pages;

public partial class ShowMedicationPage : ContentPage
{
	public ShowMedicationPage(ShowMedicationsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(BindingContext is ShowMedicationsViewModel viewModel)
		{
			viewModel.LoadMedications();
		}
    }
}