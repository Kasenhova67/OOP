using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace testt
{

    public class LocalFileStorage : IStorageStrategy
    {
        public void Save(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public string Load(string path)
        {
            return File.ReadAllText(path);
        }
    }

  
        public class OneDriveLocalStorage : IStorageStrategy
        {
            public void Save(string path, string content)
            {
                try
                {
                    
                    string oneDrivePath = GetOneDrivePath();

                    if (string.IsNullOrEmpty(oneDrivePath))
                    {
                        Console.WriteLine("OneDrive folder not found on this device");
                        return;
                    }

                string fullPath = Path.Combine(oneDrivePath, path);
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    File.WriteAllText(fullPath, content);

                    Console.WriteLine($"File successfully saved to OneDrive: {fullPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving to OneDrive: {ex.Message}");
                }
            }

            public string Load(string path)
            {
                try
                {
                    string oneDrivePath = GetOneDrivePath();

                    if (string.IsNullOrEmpty(oneDrivePath))
                    {
                        Console.WriteLine("OneDrive folder not found on this device");
                        return string.Empty;
                    }

                    string fullPath = Path.Combine(oneDrivePath, path);

                    if (!File.Exists(fullPath))
                    {
                        Console.WriteLine("File not found in OneDrive");
                        return string.Empty;
                    }

                    return File.ReadAllText(fullPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading from OneDrive: {ex.Message}");
                    return string.Empty;
                }
            }

            private string GetOneDrivePath()
            {
                
                string[] possiblePaths = new[]
                {
                Environment.GetEnvironmentVariable("OneDrive"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "OneDrive - Personal")
            };

                foreach (var path in possiblePaths)
                {
                    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                    {
                        return path;
                    }
                }

                return null;
            }
        }
    
}
