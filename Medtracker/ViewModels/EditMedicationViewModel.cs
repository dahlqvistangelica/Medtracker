using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Medtracker.ViewModels
{
    public partial class EditMedicationViewModel : ObservableObject
    {

        private readonly IHandlerRepo _repository;
        private readonly IFileStorage _fileStorage;
        [ObservableProperty]
        private Medication? _currentMedication;
        //Field to hold showstatus
        [ObservableProperty]
        private bool _isScopeInputVisible;
        public List<MedicationType> AvailableMedicationTypes { get; }

        //Property and field for medicationtype
        private MedicationType _selectedMedicationType;
        public MedicationType SelectedMedicationType
        {
            get => _selectedMedicationType;
            set
            {
                if (SetProperty(ref _selectedMedicationType, value))
                {
                    if (CurrentMedication != null)
                    {
                        CurrentMedication.medicationType = value;

                        UpdateScopeVisibility(value);
                    }
                }
            }
        }
        
        //Method to calculate showstatus.
        private void UpdateScopeVisibility(MedicationType medicationType)
        {
            bool isCalculatedSupplyType = (SelectedMedicationType == MedicationType.Injection ||
                                            SelectedMedicationType == MedicationType.Pills ||
                                            SelectedMedicationType == MedicationType.Fluid);
            IsScopeInputVisible = !isCalculatedSupplyType;
        }

        //Command for save button.
        public ICommand SaveChangesCommand { get; }
        public EditMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
        {
            _repository = repo;
            _fileStorage = fileStorage;
            AvailableMedicationTypes = Enum.GetValues<MedicationType>().ToList();
            //Initiate commando that run SaveMedication method.
            SaveChangesCommand = new AsyncRelayCommand(SaveChanges);
        }
        public void LoadMedication(int medicationId)
        {
            CurrentMedication = _repository.medications.FirstOrDefault(m => m.MedID == medicationId);

            if(CurrentMedication != null)
            {
                _selectedMedicationType = CurrentMedication.medicationType;
                UpdateScopeVisibility(CurrentMedication.medicationType);
                OnPropertyChanged(nameof(SelectedMedicationType));
            }
            else
            {
                Shell.Current.DisplayAlert("Error", "Medication not found", "Ok");
            }
        }
        private async Task SaveChanges()
        {
            if (CurrentMedication == null) return;

            if (string.IsNullOrWhiteSpace(CurrentMedication.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Medication must have a name", "Ok");
                return;
            }
            if (CurrentMedication.Dosage < 0)
            {
                await Shell.Current.DisplayAlert("Error", "Dosage must be more than 0", "Ok");
            }
            if (CurrentMedication.Strength < 0)
            {
                await Shell.Current.DisplayAlert("Error", "Strenght must be more than 0", "Ok");
            }
            if (CurrentMedication.PrescriptionCollected > DateTime.Today)
            {
                await Shell.Current.DisplayAlert("Error", "Collection date can't be in the future", "OK");
                return;
            }
            _fileStorage.SaveToFile(_repository);
            await Shell.Current.DisplayAlert("Success", $"{CurrentMedication.Name} has been updated.", "OK");
            //Call IFileStorage to save to file.
            //Return to mainpage after saving.
            await Shell.Current.GoToAsync("..");
        }
    }
}

