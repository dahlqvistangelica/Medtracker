using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Medtracker.ViewModels
{
    public partial class MainPageViewModel: ObservableObject
    {
        private readonly IHandlerRepo _repository;
        [ObservableProperty]
        private ObservableCollection<Medication> urgentMedications = new();
        public MainPageViewModel(IHandlerRepo repo)
        {
            _repository = repo;
            LoadData();
        }
        public void LoadData()
        {
            _repository.SortMedications();

            var urgentList = _repository.medications.Where(m => m.DaysLeft.TotalDays <= 7 || m.DaysLeft.TotalDays == 0)
                                                    .OrderBy(m => m.DaysLeft.TotalDays)
                                                    .ToList();
            urgentMedications.Clear();
            foreach(var med in urgentList)
            {
                urgentMedications.Add(med);
            }

        }
    }
}
