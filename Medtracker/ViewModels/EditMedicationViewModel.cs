using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;

namespace Medtracker.ViewModels;

public partial class EditMedicationViewModel : ObservableObject
{
    private readonly IHandlerRepo _repository;
    private readonly IFileStorage _fileStorage;

    [ObservableProperty]
    private Medication _currentMedication;

    public EditMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
    {
        _repository = repo;
        _fileStorage = fileStorage;
    }

    // Denna anropas från Code-behind (EditMedicationPage.xaml.cs) när sidan öppnas
    public void LoadMedication(int medicationId)
    {
        var med = _repository.medications.FirstOrDefault(m => m.MedID == medicationId);
        if (med != null)
        {
            CurrentMedication = med;
        }
    }

    // [RelayCommand] skapar automatiskt "SaveChangesCommand" som knappen i XAML letar efter
    [RelayCommand]
    private async Task SaveChanges()
    {
        if (CurrentMedication == null) return;

        if (string.IsNullOrWhiteSpace(CurrentMedication.Name))
        {
            await Shell.Current.DisplayAlert("Fel", "Medicinen måste ha ett namn", "OK");
            return;
        }

        // Här sparar vi ändringarna till filen
        _fileStorage.SaveToFile(_repository);

        await Shell.Current.DisplayAlert("Sparat", $"{CurrentMedication.Name} har uppdaterats.", "OK");

        // Navigera tillbaka till listan
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync(".."); // ".." betyder "gå upp en nivå" (tillbaka)
    }
}