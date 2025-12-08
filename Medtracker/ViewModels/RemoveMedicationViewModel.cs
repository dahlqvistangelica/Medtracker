using CommunityToolkit.Mvvm.ComponentModel;
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
        private ObservableCollection<Medication> _foundMedications = new();

        public ObservableCollection<Medication> MedicationsList
        {
            get => _foundMedications;
            set => _foundMedications = value;
        }
        public RemoveMedicationViewModel(IHandlerRepo repo)
        {
            _repo = repo;
            LoadMedications();
        }
        public string SearchMedication { get; set; } = "";
        public void LoadMedications()
        {
            MedicationsList.Clear();
            var allMeds = _repo.medications.Where(m => m.Name.Contains(SearchMedication))
            .OrderBy(m => m.DaysLeft.TotalDays)
            .ToList();

            foreach (var med in allMeds)
            {
                MedicationsList.Add(med);
            }
        }
    }
}
