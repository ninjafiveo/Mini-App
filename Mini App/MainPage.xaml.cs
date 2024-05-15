using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Mini_App
{
    public partial class MainPage : ContentPage
    {
        private const string CsvFilePath = "contact_info.csv";

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnSubmitClicked(object sender, EventArgs e)
        {
            // Collect data from input fields
            var firstName = FirstNameEntry.Text;
            var lastName = LastNameEntry.Text;
            var street = StreetEntry.Text;
            var city = CityEntry.Text;
            var state = StateEntry.Text;
            var zip = ZipEntry.Text;
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;
            var confirmPassword = ConfirmPasswordEntry.Text;

            // Validate passwords match
            if (password != confirmPassword)
            {
                DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            // Encrypt the password using SHA-256
            var encryptedPassword = ComputeSha256Hash(password);

            // Create a CSV line
            var csvLine = $"{firstName},{lastName},{street},{city},{state},{zip},{username},{encryptedPassword}";

            // Append the CSV line to the file
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fullPath = Path.Combine(documentsPath, CsvFilePath);
            File.AppendAllText(fullPath, csvLine + Environment.NewLine);

            // Optionally, clear the input fields after submission
            ClearFields();
        }

        private void OnClearClicked(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            StreetEntry.Text = string.Empty;
            CityEntry.Text = string.Empty;
            StateEntry.Text = string.Empty;
            ZipEntry.Text = string.Empty;
            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
