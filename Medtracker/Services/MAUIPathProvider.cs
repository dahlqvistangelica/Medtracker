using System;
using System.Collections.Generic;
using System.Text;
using Medtracker.Interfaces;
using System.IO;
using Microsoft.Maui.Storage;

namespace Medtracker.Services
{
    internal class MAUIPathProvider : IPathProvider
    {
        private const string _appfolder = "MedTracker";
        private const string _fileName = "medTrackerMeds.json";

        public string GetDatabasePath()
        {
            string appDataPath = FileSystem.AppDataDirectory;
            string directoryPath = Path.Combine(appDataPath, _appfolder);

            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return Path.Combine(directoryPath, _fileName);
        }


    }
}
