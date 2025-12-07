using MedTrackConsole.Interfaces;
using MedTrackConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Services
{
    public class DataManager : IHandlerRepo
    {
        public List<Medication> medications { get; set; }
        public List<Medication> usedMedications { get; set; }

        public DataManager()
        {
            medications = new List<Medication>();
            usedMedications = new List<Medication>();
        }
        public void FilterAndSplitLists()
        {
            usedMedications = medications.Where(m => m.DaysLeft.Days == 0)
                .ToList();
            medications.RemoveAll(m => m.DaysLeft.Days == 0);
        }
        public void SortMedications()
        {
            medications.Sort((m1, m2) => m1.Name.CompareTo(m2.Name));
            usedMedications.Sort((m1, m2) => m1.Name.CompareTo(m2.Name));
        }
    }
}
