
using iCreator.Elements;
using iCreator.External;
using iCreator.Utils;
using SandboxApp.iCreator.Models;

namespace SandboxApp.iCreator.Controllers
{
    public class registerController : IController
    {
#pragma warning disable 0649
        private ILogger logger;

        private Checkbox maleCheckbox;
        private Checkbox femaleCheckbox;

        private TextField loginTextField;
        private TextField passwordTextField;
        private TextField repeatPasswordTextField;

        private Button registerButton;

        private Account account;
#pragma warning restore 0649

        public void Initialize()
        {
            maleCheckbox.AddOnSetCheckedListener((value) =>
            {
                if (value)
                    femaleCheckbox.SetChecked(false);
            });

            femaleCheckbox.AddOnSetCheckedListener((value) =>
            {
                if (value)
                    maleCheckbox.SetChecked(false);
            });

            registerButton.AddOnClickListener(() =>
            {
                if(string.IsNullOrEmpty(loginTextField.Text))
                {
                    logger.Warning("You must provide login.");

                    return;
                }

                if(string.IsNullOrEmpty(passwordTextField.Text))
                {
                    logger.Warning("You must provide password.");

                    return;

                }

                if(!passwordTextField.Equals(repeatPasswordTextField.Text))
                {
                    logger.Warning("Passwords must match.");

                    return;
                }

                Account account = new Account()
                {
                    Login = loginTextField.Text,
                    Password = passwordTextField.Text
                };

                logger.Info($"Successfully registered with " +
                    $"login \"{ account.Login }\" and password \"{ account.Password }\".");
            });
        }
    }
}
