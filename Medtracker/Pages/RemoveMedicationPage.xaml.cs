using Medtracker.ViewModels;

namespace Medtracker.Pages;

public partial class RemoveMedicationPage : ContentPage
{
	public RemoveMedicationPage(RemoveMedicationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}