using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CourseWork.Services
{
    public class StorageContext
    {
        private IStorageStrategy _strategy;

        public void SetStrategy(IStorageStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Save(string path, string content)
        {
            _strategy?.Save(path, content);
        }

        public string Load(string path)
        {
            return _strategy?.Load(path) ?? string.Empty;
        }
    }

    public interface IStorageStrategy
    {
        void Save(string path, string content);
        string Load(string path);
    }

    public class LocalFileStorage : IStorageStrategy
    {
        public void Save(string path, string content)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, content);
                Console.WriteLine($"Файл успешно сохранён локально: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении локального файла: {ex.Message}");
            }
        }

        public string Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("Локальный файл не найден");
                    return string.Empty;
                }
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке локального файла: {ex.Message}");
                return string.Empty;
            }
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
                    Console.WriteLine("Папка OneDrive не найдена на этом устройстве");
                    return;
                }

                string fullPath = Path.Combine(oneDrivePath, path);
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                File.WriteAllText(fullPath, content);
                Console.WriteLine($"Файл успешно сохранён в OneDrive: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в OneDrive: {ex.Message}");
            }
        }

        public string Load(string path)
        {
            try
            {
                string oneDrivePath = GetOneDrivePath();
                if (string.IsNullOrEmpty(oneDrivePath))
                {
                    Console.WriteLine("Папка OneDrive не найдена на этом устройстве");
                    return string.Empty;
                }

                string fullPath = Path.Combine(oneDrivePath, path);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("Файл не найден в OneDrive");
                    return string.Empty;
                }

                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из OneDrive: {ex.Message}");
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

            return possiblePaths.FirstOrDefault(p => !string.IsNullOrEmpty(p) && Directory.Exists(p));
        }
    }
}