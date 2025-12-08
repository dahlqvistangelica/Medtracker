using System;
using System.Collections.Generic;
using System.Text;
using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using Medtracker.ViewModels;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MedTrackConsole.Services;

namespace Medtracker.ViewModels
{
    public class AddMedicationViewModel: ObservableObject
    {
        private readonly IHandlerRepo _repository;
        private readonly IFileStorage _fileStorage;
        public List<MedicationType> AvailableMedicationTypes { get; }
        //Property to hold new medication (binding)
        public Medication NewMedication { get; set; } = new Medication();
        //Field to hold showstatus
        private bool _isScopeInputVisible;
        public bool IsScopeInputVisible
        {
            get => _isScopeInputVisible;
            set => SetProperty(ref _isScopeInputVisible, value);
        }
        //Property and field for medicationtype
        private MedicationType _selectedMedicationType;
        public MedicationType SelectedMedicationType
        {
            get => _selectedMedicationType;
            set
            {
                if(SetProperty(ref _selectedMedicationType, value))
                {
                    NewMedication.medicationType = value;

                    UpdateScopeVisibility(value);
                }
            }
        }

        /// <summary>
        /// Updates the visibility of the scope input based on the specified medication type.
        /// </summary>
        /// <param name="medicationType">The medication type used to determine whether the scope input should be visible.</param>
        private void UpdateScopeVisibility(MedicationType medicationType)
        {
            bool isCalculatedSupplyType = (SelectedMedicationType == MedicationType.Injection ||
                                            SelectedMedicationType == MedicationType.Pills ||
                                            SelectedMedicationType == MedicationType.Fluid);
            IsScopeInputVisible = !isCalculatedSupplyType;
        }

        //Command for save button.
        public ICommand SaveCommand { get; }
        public AddMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
        {
            _repository = repo;
            _fileStorage = fileStorage;
            AvailableMedicationTypes = Enum.GetValues<MedicationType>().ToList();
            _selectedMedicationType = NewMedication.medicationType;
            UpdateScopeVisibility(NewMedication.medicationType);
            //Initiate commando that run SaveMedication method.
            SaveCommand = new Command(async () => await SaveMedication());
        }
        /// <summary>
        /// Validates and saves the current medication entry to persistent storage asynchronously. Displays error
        /// messages for invalid input and navigates back to the previous page upon successful save.
        /// </summary>
        /// <remarks>Validation includes checking that the medication name is not empty, dosage and
        /// strength are positive values, and the prescription collection date is not in the future. If any validation
        /// fails, an error alert is displayed and the save operation is aborted. Upon successful save, the method
        /// navigates back to the main page.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        private async Task SaveMedication()
        {
            if(string.IsNullOrWhiteSpace(NewMedication.Name))
            {
                await Shell.Current.DisplayAlert("Error", "Medication must have a name", "Ok");
                return;
            }
            if(NewMedication.Dosage < 0)
            {
                await Shell.Current.DisplayAlert("Error", "Dosage must be more than 0", "Ok");
            }
            if(NewMedication.Strength <0)
            {
                await Shell.Current.DisplayAlert("Error", "Strenght must be more than 0", "Ok");
            }
            if(NewMedication.PrescriptionCollected > DateTime.Today)
            {
                await Shell.Current.DisplayAlert("Error", "Collection date can't be in the future", "OK");
                return;
            }
            _repository.medications.Add(NewMedication);
            _fileStorage.SaveToFile(_repository);
            //Call IFileStorage to save to file.
            //Return to mainpage after saving.
            await Shell.Current.GoToAsync("..");
        }
    }
}
