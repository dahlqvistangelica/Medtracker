using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Interfaces
{
    public interface IFileStorage
    {
        /// <summary>
        /// Creates instans of type T
        /// </summary>
        /// <param name="dataInstance"></param>
        void SaveToFile<T>(T dataInstance);

        /// <summary>
        /// Load and returns instance of type T
        /// </summary>
        /// <returns></returns>
        T? ReadFromFile<T>() where T : class;
    }
}
