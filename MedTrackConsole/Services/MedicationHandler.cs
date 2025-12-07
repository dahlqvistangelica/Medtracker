using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using Mylibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Services
{
    internal class MedicationHandler
    {
        private readonly IHandlerRepo _repository;

       
        private readonly IFileStorage _storeData;
        internal MedicationHandler(IHandlerRepo repository, IFileStorage storeData)
        {
            _repository = repository;
            _storeData = storeData;
        }
        #region Public methods
        public void CreateAndAddMedication()
        {
            Medication medication = CreateMedication();
            AddMedication(medication);
        }

        public bool NoMedications()
        {
            if (_repository.medications.Count <= 0)
                return true;
            return false;
        }

        public void SelectAndRemoveMedication()
        {
            if (NoMedications())
            {
                //TODO: No medications to show
            }
            else
            {
                if (RemoveMedication())
                {
                    //TODO: Medication successfully removed
                }
                else
                {
                    //ToDO:Error
                }
            }
        }
        public void ShowMedicationsToUser()
        {
            ShowMedications();
        }
        public void EditMedication()
        {
            Medication medicationToEdit = GetMedication("Enter name to search for:", "Invalid input, try again.", "Enter ID for medication to edit:");
            EditMedicationChoice(medicationToEdit);
        }

        #endregion
        #region Private methods
        private void AddMedication(Medication med)
        {
            _repository.medications.Add(med);
            _storeData.SaveToFile(_repository);
        }
        private void ShowMedications()
        {
            if (_repository.medications.Count <= 0)
            {
                Console.WriteLine("No medications added.");
            }
            else
            {
                Console.WriteLine("Current medications:");
                foreach (var m in _repository.medications)
                {
                    Console.WriteLine(m);
                }
                Console.WriteLine();
                if (_repository.usedMedications.Count > 0)
                {
                    ShowEmptyMedications();
                }
            }
        }
        private void ShowEmptyMedications()
        {
            if (_repository.usedMedications.Count <= 0)
            { Console.WriteLine("No empty medications."); }
            else
            {
                Console.WriteLine("Medications to refill:");
                foreach (var m in _repository.usedMedications)
                {
                    Console.WriteLine(m);
                }
                Console.WriteLine();
            }
        }
        private Medication GetMedication(string FirstPromt, string errorMsg, string SecondPromt)
        {
            string medicationNameToFind = InputHandler.StringInput(FirstPromt, errorMsg);
            var foundMedications = _repository.medications.Where(m => m.Name.ToLower().Contains(medicationNameToFind.ToLower()))
                                                          .ToList();
            foreach (var m in foundMedications)
            {
                Console.WriteLine($"ID: {m.MedID} " + m);
            }
            int medIDToGet = InputHandler.IntInput(SecondPromt, errorMsg);
            Medication medicationToRemove = _repository.medications.FirstOrDefault(m => m.MedID.Equals(medIDToGet));
            return medicationToRemove;
        }
        private bool RemoveMedication()
        {
            Medication medicationToRemove = GetMedication("Enter medicationname to search for:", "Invalid input, try again.", "Enter ID for medication to remove:");
            bool isSuccess = _repository.medications.Remove(medicationToRemove);
            _storeData.SaveToFile(_repository);
            return isSuccess;
        }
        private void EditMedicationChoice(Medication medication)
        {
            int userChoice = 0;
            int exitnumber = 8;
            while (userChoice != exitnumber)
            {
                Console.WriteLine($"What do you want to change with {medication.Name}?");
                Console.WriteLine("[1] - Name");
                Console.WriteLine("[2] - Collection date");
                Console.WriteLine("[3] - Strength");
                Console.WriteLine("[4] - Dosage");
                Console.WriteLine("[5] - Collected amount");
                Console.WriteLine("[6] - Medication type");
                Console.WriteLine("[7] - Prescription status");
                Console.WriteLine("[8] - Return to main menu.");
                userChoice = InputHandler.IntInput("Enter choice:", "Invalid choice, try again.");
                switch (userChoice)
                {
                    case 1:
                        Console.Clear();
                        EditMedicationName(medication);
                        Console.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        EditCollectionDate(medication);
                        Console.Clear();
                        break;
                    case 3:
                        Console.Clear();
                        EditStrength(medication);
                        Console.Clear();
                        break;
                    case 4:
                        EditDosage(medication);
                        break;
                    case 5:
                        Console.Clear();
                        EditCollectedAmount(medication);
                        Console.Clear();
                        break;
                    case 6:
                        Console.Clear();
                        EditMedicationType(medication);
                        Console.Clear();
                        break;
                    case 7:
                        Console.Clear();
                        EditPerscriptionStatus(medication);
                        Console.Clear();
                        break;
                    case 8:
                        Console.WriteLine("Returning to mainmenu");
                        _storeData.SaveToFile(_repository);
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again");
                        break;
                }
            }
        }
        private void EditMedicationName(Medication medication)
        {
            string oldName = medication.Name;
            string newName = InputHandler.StringInput($"Enter new name for {oldName}:", "Invalid name, try again");
            medication.Name = newName;
            if (medication.Name.Equals(newName))
            {
                Console.WriteLine($"Name successfully changed from {oldName} to {medication.Name}");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditCollectionDate(Medication medication)
        {
            DateTime oldCollectionDate = new DateTime();
            oldCollectionDate = medication.PrescriptionCollected;
            DateTime newCollectionDate = DateTimeHandler.UserChoiceDate("Enter collection date (yyyy-mm-dd):", "Invalid date, please enter correct dateformat.");
            medication.PrescriptionCollected = newCollectionDate;
            if (medication.PrescriptionCollected.Equals(newCollectionDate))
            {
                Console.WriteLine($"Collection date successfully changed from {oldCollectionDate} to {newCollectionDate}");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditStrength(Medication medication)
        {
            decimal oldStrenght = medication.Strength;
            decimal newStrenght = InputHandler.DecimalInput($"Enter strenght for {medication.Name}:", "Invalid dosage, try again");
            medication.Strength = newStrenght;
            if (medication.Strength.Equals(newStrenght))
            {
                Console.WriteLine($"Strenght successfully changed from {oldStrenght} to {medication.Strength}");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditDosage(Medication medication)
        {
            decimal oldDosage = medication.Dosage;
            decimal newDosage = InputHandler.DecimalInput($"Enter dosage for {medication.Name}:", "Invalid dosage, try again");
            medication.Dosage = newDosage;
            if (medication.Strength.Equals(newDosage))
            {
                Console.WriteLine($"Dosage successfully changed from {oldDosage} to {medication.Dosage}");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditCollectedAmount(Medication medication)
        {
            decimal oldAmount = medication.AmountCollected;
            decimal newAmount = InputHandler.DecimalInput($"Enter amount collected for {medication.Name}:", "Invalid amount, try again");
            medication.AmountCollected = newAmount;
            if (medication.Strength.Equals(newAmount))
            {
                Console.WriteLine($"Amount successfully changed from {oldAmount} to {medication.AmountCollected}");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditMedicationType(Medication medication)
        {
            MedicationType oldMedType = medication.medicationType;
            Console.WriteLine($"Current medicationtype on {medication.Name} is:{medication.medicationType}");
            int medicationInt = InputHandler.IntInput("Medicationtypes:\n1.Fluid\n2.Pill\n3.Injection\n4.Ointment\n5.Other\nChoose type of medication: ", "You must enter a number between 1-5"); //Fixa metod med min och maxvärde.
            MedicationType newMedType = Medication.GetMedicationType(medicationInt);
            medication.medicationType = newMedType;
            if (medication.medicationType.Equals(newMedType))
            {
                Console.WriteLine("Successfully changed medicationtype");
            }
            else
            {
                Console.WriteLine("Something went wrong. Try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private void EditPerscriptionStatus(Medication medication)
        {
            bool oldperscription = medication.AllCollected;
            bool changePerscription = InputHandler.BoolInput($"Do you want to change perscription status for {medication.Name}?", "Invalid input, enter yes/y or no/n", "yes", 'y', "no", 'n');
            if (changePerscription)
            {
                if (oldperscription)
                {
                    medication.AllCollected = false;
                }
                else
                {
                    medication.AllCollected = true;
                }
            }
            if (oldperscription != medication.AllCollected)
            {
                Console.WriteLine("Perscription status changed.");
            }
            else if (!changePerscription)
            {
                Console.WriteLine("Perscription status unchanged.");
            }
            else
            {
                Console.WriteLine("Something went wrong, try again.");
            }
            _storeData.SaveToFile(_repository);
            Console.ReadKey();
        }
        private Medication CreateMedication()
        {
            Medication create = new Medication();
            create.Name = InputHandler.StringInput("Enter medication name:", "You must enter a valid name.");
            create.PrescriptionCollected = DateTimeHandler.UserCreateDateTime("Did you collect the medication today?", "You must enter yes/y or no/n", "yes", 'y', "no", 'n', "Enter collectionday in (yyyy-mm-dd)", "You must enter a date in yyyy-mm-dd format.");
            int medicationType = InputHandler.IntInput("Medicationtypes:\n1.Fluid\n2.Pill\n3.Injection\n4.Ointment\n5.Other\nChoose type of medication: ", "You must enter a number between 1-5"); //Fixa metod med min och maxvärde.
            create.medicationType = Medication.GetMedicationType(medicationType);
            if (create.medicationType == MedicationType.Ointment || create.medicationType == MedicationType.Other)
            {
                create.UserMedicationScope = InputHandler.StringInput("Enter calculated medication scope:", "You must enter a valid scope with numbers in days.");
            }
            create.AmountCollected = InputHandler.IntInput("Enter amount collected:", "You must enter a valid amount", 0);
            create.Dosage = InputHandler.IntInput("Enter daily dosage:", "You must enter a valid dosage.", 0);
            create.AllCollected = InputHandler.BoolInput("Did you collect the last on the prescription:", "You must answer with yes/y or no/n", "yes", 'y', "no", 'n');
            return create;
        }
        #endregion

    }
}
