using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using Medtracker.Pages;
using System.Linq;

namespace Medtracker.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IHandlerRepo _repository;

        // Egenskaper för statistiken på startsidan
        [ObservableProperty]
        private int _totalMedicationsCount;

        [ObservableProperty]
        private int _lowStockCount;

        // Konstruktor som tar emot repot (precis som du hade innan)
        public MainPageViewModel(IHandlerRepo repo)
        {
            _repository = repo;
            // Vi laddar data direkt, men också bra att anropa LoadData() när sidan visas
            LoadData();
        }

        public void LoadData()
        {
            // Sortera listan (din gamla logik)
            _repository.SortMedications();

            // 1. Uppdatera totalen
            TotalMedicationsCount = _repository.medications.Count;

            // 2. Räkna ut hur många som har lågt lager (t.ex. 14 dagar eller mindre)
            // Du hade <= 7 dagar innan, men React-designen använder ofta 14 för "Varning". 
            // Du kan ändra siffran här om du vill.
            LowStockCount = _repository.medications.Count(m => m.DaysLeft.TotalDays <= 14);
        }

        // --- HÄR ÄR DE NYA KOMMANDONA FÖR KNAPPARNA ---

        [RelayCommand]
        private async Task GoToShowMedications()
        {
            await Shell.Current.GoToAsync(nameof(ShowMedicationPage));
        }

        [RelayCommand]
        private async Task GoToAddMedication()
        {
            await Shell.Current.GoToAsync(nameof(AddMedicationPage));
        }

        [RelayCommand]
        private async Task GoToRemoveMedication()
        {
            await Shell.Current.GoToAsync(nameof(RemoveMedicationPage));
        }
    }
}