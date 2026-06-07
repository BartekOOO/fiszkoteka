using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace QuizMaster.Wpf.Services
{
    public sealed class AppSettings : IAppSettings
    {
        private readonly string _filePath;

        public string SavedEmail { get; private set; }
        public bool RememberLogin { get; private set; }

        public AppSettings()
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "QuizMaster");

            Directory.CreateDirectory(folder);

            _filePath = Path.Combine(folder, "settings.json");
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
                return;

            var json = File.ReadAllText(_filePath);
            var settings = JsonSerializer.Deserialize<SettingsDto>(json);

            if (settings == null)
                return;

            SavedEmail = settings.SavedEmail;
            RememberLogin = settings.RememberLogin;
        }

        public void SaveRememberLogin(string email, bool rememberLogin)
        {
            RememberLogin = rememberLogin;
            SavedEmail = rememberLogin ? email : string.Empty;

            var settings = new SettingsDto
            {
                SavedEmail = SavedEmail,
                RememberLogin = RememberLogin
            };

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }

        private class SettingsDto
        {
            public string SavedEmail { get; set; }
            public bool RememberLogin { get; set; }
        }
    }
}
