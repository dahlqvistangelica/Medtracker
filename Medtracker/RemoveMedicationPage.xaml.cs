using Medtracker.ViewModels;

namespace Medtracker;

public partial class RemoveMedicationPage : ContentPage
{
	public RemoveMedicationPage(RemoveMedicationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}