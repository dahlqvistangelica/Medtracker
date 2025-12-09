using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using System.Collections.ObjectModel;

namespace Medtracker.ViewModels;

public partial class RemoveMedicationViewModel : ObservableObject
{
    private readonly IHandlerRepo _repository;
    private readonly IFileStorage _fileStorage;

    public ObservableCollection<Medication> MedicationsList { get; set; } = new();

    public RemoveMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
    {
        _repository = repo;
        _fileStorage = fileStorage;
        LoadMedications();
    }

    public void LoadMedications()
    {
        MedicationsList.Clear();
        foreach (var med in _repository.medications)
        {
            MedicationsList.Add(med);
        }
    }

    // VIKTIGT: Detta genererar 'DeleteMedicationCommand'
    [RelayCommand]
    private async Task DeleteMedication(int medId)
    {
        var medToRemove = _repository.medications.FirstOrDefault(m => m.MedID == medId);
        if (medToRemove != null)
        {
            bool confirm = await Shell.Current.DisplayAlert("Ta bort", $"Är du säker på att du vill ta bort {medToRemove.Name}?", "Ja", "Nej");
            if (confirm)
            {
                _repository.medications.Remove(medToRemove);
                _fileStorage.SaveToFile(_repository);
                LoadMedications(); // Uppdatera listan direkt
            }
        }
    }
    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync(".."); // ".." betyder "gå upp en nivå" (tillbaka)
    }
}