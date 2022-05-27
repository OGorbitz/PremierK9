using Mobile.Models;
using Mobile.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Mobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly Regex passwordRegExp = new Regex("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,20})");

        public ValidatableObject<string> Username { get; set; } = new ValidatableObject<string>();
        public ValidatableObject<string> Password { get; set; } = new ValidatableObject<string>();

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            AddValidationRules();
        }

        private async void OnLoginClicked(object obj)
        {
            if (AreFieldsValid())
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");

                Unit unit = new Unit()
                {
                    ID = Guid.NewGuid(),
                    Name = "Test"
                };
            }
        }

        public void AddValidationRules()
        {
            Username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter username" });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Enter password" });

        }

        bool AreFieldsValid()
        {
            bool isUsernameValid = Username.Validate();
            bool isPasswordValid = Password.Validate();

            return isUsernameValid && isPasswordValid;
        }
    }
}
