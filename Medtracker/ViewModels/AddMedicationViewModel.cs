using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using System.Collections.ObjectModel;

namespace Medtracker.ViewModels;

public partial class AddMedicationViewModel : ObservableObject
{
    private readonly IHandlerRepo _repository;
    private readonly IFileStorage _fileStorage;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string strength;

    [ObservableProperty]
    private string dosage;

    [ObservableProperty]
    private string amountCollected;

    [ObservableProperty]
    private DateTime prescriptionCollected = DateTime.Today;

    [ObservableProperty]
    private MedicationType selectedMedicationType;

    public List<MedicationType> AvailableMedicationTypes { get; } = Enum.GetValues(typeof(MedicationType)).Cast<MedicationType>().ToList();

    public AddMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
    {
        _repository = repo;
        _fileStorage = fileStorage;
    }

    // VIKTIGT: Detta genererar 'SaveMedicationCommand' som XAML-koden letar efter
    [RelayCommand]
    private async Task SaveMedication()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Fel", "Du måste ange ett namn", "OK");
            return;
        }

        // Konvertera strängar till decimaler/int säkert
        if (!decimal.TryParse(Strength, out decimal strengthVal)) strengthVal = 0;
        if (!decimal.TryParse(Dosage, out decimal dosageVal)) dosageVal = 0;
        if (!decimal.TryParse(AmountCollected, out decimal amountVal)) amountVal = 0;

        var newMed = new Medication
        {
            Name = Name,
            Strength = strengthVal,
            Dosage = dosageVal,
            AmountCollected = amountVal,
            PrescriptionCollected = PrescriptionCollected,
            medicationType = SelectedMedicationType
        };

        _repository.medications.Add(newMed);
        _fileStorage.SaveToFile(_repository);

        await Shell.Current.DisplayAlert("Klart", $"{Name} har sparats!", "OK");
        await Shell.Current.GoToAsync(".."); // Gå tillbaka
    }
    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync(".."); // ".." betyder "gå upp en nivå" (tillbaka)
    }
}