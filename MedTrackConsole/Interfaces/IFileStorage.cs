using System;
using System.Collections.Generic;
using System.Text;

namespace MedTrackConsole.Interfaces
{
    public interface IFileStorage
    {
        /// <summary>
        /// Skapar en instans av typen T
        /// </summary>
        /// <param name="dataInstance"></param>
        void SaveToFile<T>(T dataInstance);

        /// <summary>
        /// Laddar och returnar en instans av typen T
        /// </summary>
        /// <returns></returns>
        T? ReadFromFile<T>() where T : class;
    }
}
