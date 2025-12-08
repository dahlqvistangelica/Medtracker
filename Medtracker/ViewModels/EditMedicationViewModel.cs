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
        /// <summary>
        /// Updates the visibility of the scope input based on the selected medication type.
        /// </summary>
        /// <remarks>Scope input is hidden for calculated supply types such as Injection, Pills, or Fluid.
        /// For other medication types, the scope input is visible.</remarks>
        /// <param name="medicationType">The medication type to evaluate when determining scope input visibility.</param>
        private void UpdateScopeVisibility(MedicationType medicationType)
        {
            bool isCalculatedSupplyType = (SelectedMedicationType == MedicationType.Injection ||
                                            SelectedMedicationType == MedicationType.Pills ||
                                            SelectedMedicationType == MedicationType.Fluid);
            IsScopeInputVisible = !isCalculatedSupplyType;
        }

        //Command for save button.
        public ICommand SaveChangesCommand { get; }
        /// <summary>
        /// Initializes a new instance of the EditMedicationViewModel class with the specified repository and file
        /// storage service.
        /// </summary>
        /// <param name="repo">The repository used to access and manage medication data.</param>
        /// <param name="fileStorage">The file storage service used for handling file operations related to medications.</param>
        public EditMedicationViewModel(IHandlerRepo repo, IFileStorage fileStorage)
        {
            _repository = repo;
            _fileStorage = fileStorage;
            AvailableMedicationTypes = Enum.GetValues<MedicationType>().ToList();
            //Initiate commando that run SaveMedication method.
            SaveChangesCommand = new AsyncRelayCommand(SaveChanges);
        }
        /// <summary>
        /// Loads the medication with the specified identifier and updates the current selection.
        /// </summary>
        /// <remarks>If the specified medication is found, the current medication and its type are
        /// updated, and related UI elements are refreshed. If not found, an error message is shown to the
        /// user.</remarks>
        /// <param name="medicationId">The unique identifier of the medication to load. Must correspond to an existing medication; otherwise, an
        /// error alert is displayed.</param>
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
        /// <summary>
        /// Validates the current medication entry and saves changes to persistent storage asynchronously. Displays
        /// appropriate alerts for validation errors or successful updates.
        /// </summary>
        /// <remarks>If validation fails, an error alert is displayed and the save operation is not
        /// performed. Upon successful save, a confirmation alert is shown and navigation returns to the previous
        /// page.</remarks>
        /// <returns>A task that represents the asynchronous save operation.</returns>
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

