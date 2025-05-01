using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

   
    public sealed class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object _lock = new object();

        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public int FontSize { get; set; } = 12;
        public string Theme { get; set; } = "Default";
        public string GoogleDriveAccessToken { get; set; } = "";

        private AppSettings() { }

        public static AppSettings Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSettings();
                    }
                    return _instance;
                }
            }
        }

        public void ApplySettings()
        {
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = TextColor;
            Console.Clear();
        }
    }
}
