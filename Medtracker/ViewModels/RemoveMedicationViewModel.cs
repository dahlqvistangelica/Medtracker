using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using MedTrackConsole.Services;
using Medtracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace Medtracker.ViewModels
{
    public class RemoveMedicationViewModel: ObservableObject
    {
        private readonly IHandlerRepo _repo;
        private readonly IFileStorage _filestorage;
        private ObservableCollection<Medication> _foundMedications = new();

        public ObservableCollection<Medication> MedicationsList
        {
            get => _foundMedications;
            set => _foundMedications = value;
        }
        public RemoveMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
        {
            _repo = repo;
            _filestorage = fileStorage;
            LoadMedications();
            RemoveCommand = new AsyncRelayCommand<Medication>(RemoveMedicationAsync);
        }
        private string _searchMedication = "";
        public string SearchMedication
        {
            get => _searchMedication;
            set
            {
                if(SetProperty(ref _searchMedication, value))
                {
                    LoadMedications();
                }
            }
        }
        public ICommand RemoveCommand { get; }
       
        public void LoadMedications()
        {
            MedicationsList.Clear();
            var filter = SearchMedication ?? string.Empty;

            var allMeds = _repo.medications.Where(m => m.Name.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(m => m.DaysLeft.TotalDays)
            .ToList();

            foreach (var med in allMeds)
            {
                MedicationsList.Add(med);
            }
        }
        private async Task RemoveMedicationAsync(Medication? medicationToRemove)
        {
            if(medicationToRemove == null)
            {
                return;
            }
            //Allow user to confirm remove
            bool confirmed = await Shell.Current.DisplayAlert("Confirm remove", $"Are you sure you wan't to remove {medicationToRemove.Name}?", "Yes", "No");
            if(confirmed)
            {
                _repo.medications.Remove(medicationToRemove);
                MedicationsList.Remove(medicationToRemove);
                _filestorage.SaveToFile(_repo);
                await Shell.Current.DisplayAlert("Remobed", $"{medicationToRemove.Name} have been removed.", "OK");
            }

        }
    }
}
