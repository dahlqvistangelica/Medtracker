using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using Medtracker.Pages;
using Medtracker.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Medtracker.ViewModels;

public class ShowMedicationsViewModel : ObservableObject
{
	private readonly IHandlerRepo _repository;
	private ObservableCollection<Medication> _medicationsList = new();
	public ObservableCollection<Medication> MedicationsList
	{
		get => _medicationsList;
		set => _medicationsList = value;
	}
	public ICommand GoToEditCommand { get; }
	public ShowMedicationsViewModel(IHandlerRepo repo)
	{
		_repository = repo;
		LoadMedications();
		GoToEditCommand = new AsyncRelayCommand<int>(GoToEditPage);
	}
	/// <summary>
	/// Loads all medications from the repository into the MedicationsList collection, ordered by the number of days left
	/// until each medication runs out.
	/// </summary>
	/// <remarks>Existing items in MedicationsList are cleared before loading new medications. This method should be
	/// called to refresh the list when the underlying medication data changes.</remarks>
	public void LoadMedications()
	{
		MedicationsList.Clear();
		var allMeds = _repository.medications
		.OrderBy(m => m.DaysLeft.TotalDays)
		.ToList();

		foreach(var med in allMeds)
		{
			MedicationsList.Add(med);
		}
	}
	private async Task GoToEditPage(int medicationId)
	{
		await Shell.Current.GoToAsync($"{nameof(EditMedicationPage)}?MedicationId={medicationId}");
	}
}