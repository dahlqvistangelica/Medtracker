using Medtracker.ViewModels;
using Microsoft.Maui.Controls;

namespace Medtracker.Pages;
// Uses QueryProperty to get Medication-object from prevoius page.
[QueryProperty(nameof(MedicationId), "MedicationId")]
public partial class EditMedicationPage : ContentPage
{
	private readonly EditMedicationViewModel _viewModel;
    // For QueryProperty usage. Binds automatic to a setter method in ViewModel. 
    public string MedicationId { get; set; }
	public EditMedicationPage(EditMedicationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}
	//When the page is shown, update viewmodel with the provided id.
    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(int.TryParse(MedicationId, out int id))
		{ _viewModel.LoadMedication(id); }
    }
}