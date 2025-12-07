using Medtracker.ViewModels;
using Microsoft.Maui.Controls;

namespace Medtracker;

public partial class AddMedicationPage : ContentPage
{
	public AddMedicationPage(AddMedicationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}