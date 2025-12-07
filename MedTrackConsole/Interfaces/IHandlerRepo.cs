using MedTrackConsole.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Interfaces
{
    public interface IHandlerRepo
    {
        List<Medication> medications { get; }
        List<Medication> usedMedications { get; }
        void SortMedications();

    }
}
