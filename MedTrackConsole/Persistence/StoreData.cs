using MedTrackConsole.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MedTrackConsole.Persistence
{
    public class StoreData : IFileStorage
    {
        private readonly string _path;

        public StoreData(string path)
        {
            _path = path;
        }
        public T? ReadFromFile<T>() where T : class
        {
            if (!File.Exists(_path))
            { return null; }
            try
            {
                if (new FileInfo(_path).Length == 0)
                {
                    return null;
                }
                return JsonSerializer.Deserialize<T>(File.ReadAllText(_path));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Could not read data from file.");
                
            }
        }

        public void SaveToFile<T>(T saveInstance)
        {
            var jString = JsonSerializer.Serialize<T>(saveInstance, JsonSerializerOptions.Default);
            File.WriteAllText(_path, jString);
        }
    }
}
