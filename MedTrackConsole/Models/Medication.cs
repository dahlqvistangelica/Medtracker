using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Models
{
    public enum MedicationType { Fluid, Pills, Injection, Ointment, Other }
    public class Medication
    {
        private static int MedCountForID;
        internal int MedID;
        public string Name { get; set; } = "Unknown";
        public decimal Dosage { get; set; }
        public decimal Strength { get; set; }
        public decimal AmountCollected { get; set; }
        public bool AllCollected = false;
        public MedicationType medicationType { get; set; }

        public int DaysSupply
        {
            get
            {
                if (medicationType == MedicationType.Injection || medicationType == MedicationType.Pills || medicationType == MedicationType.Fluid)
                {
                    if (Dosage > 0)
                    {
                        return (int)(AmountCollected / Dosage);
                    }

                }
                else if (int.TryParse(UserMedicationScope, out int userDays))
                {
                    return userDays;
                }
                return 0;
            }
        }
        public string UserMedicationScope { get; set; } = "0";
        public DateTime PrescriptionCollected { get; set; } = DateTime.Today;
        public DateTime LastDayOfMedication { get => PrescriptionCollected.AddDays(DaysSupply); }
        public TimeSpan DaysLeft
        {
            get
            {
                // Beräkna skillnaden mellan slutdatum och dagens datum
                TimeSpan daysRemaining = LastDayOfMedication.Subtract(DateTime.Today);

                // Returnera TimeSpan.Zero om medicinen redan tagit slut
                return (daysRemaining.TotalDays < 0) ? TimeSpan.Zero : daysRemaining;
            }
        }

        public static MedicationType GetMedicationType(int input)
        {
            return input switch
            {
                1 => MedicationType.Fluid,
                2 => MedicationType.Pills,
                3 => MedicationType.Injection,
                4 => MedicationType.Ointment,
                5 => MedicationType.Other
            };

        }
        public string GetStrengthEnding()
        {
            if (this.medicationType == MedicationType.Pills)
                return "mg";
            if (this.medicationType == MedicationType.Injection || this.medicationType == MedicationType.Fluid)
                return "mg/ml";
            if (this.medicationType == MedicationType.Ointment)
                return "%";
            else
                return "ns";
        }
        public override string ToString()
        {
            return $"Name: {Name} {Strength}{GetStrengthEnding()} Date collected: {PrescriptionCollected:d} Days left:{DaysLeft.Days} Refill prescription: {(AllCollected ? "Yes" : "No")}";
        }
        public Medication()
        {
            MedCountForID++;
            MedID = MedCountForID;
        }
    }
}
