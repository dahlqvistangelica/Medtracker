using CommunityToolkit.Mvvm.ComponentModel;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using Medtracker.ViewModels;
using System.Collections.ObjectModel;
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
	public ShowMedicationsViewModel(IHandlerRepo repo)
	{
		_repository = _repository;
		LoadMedications();
	}
	
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
}