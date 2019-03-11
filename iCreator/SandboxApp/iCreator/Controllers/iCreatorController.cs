using iCreator.Elements;
using iCreator.External;
using iCreator.Utils;
using SandboxApp.iCreator.Models;

namespace SandboxApp.iCreator.Controllers
{
    public class iCreatorController : IController
    {
#pragma warning disable 0649
        private Button loginButton;
        private Label newAccountButton;
        private TextField loginTextField;
        private TextField passwordTextField;
        private ILogger logger;
#pragma warning restore 0649

        private Account account;

        public void Initialize()
        {
            loginButton.AddOnClickListener(() =>
            {
                if (string.IsNullOrEmpty(loginTextField.Text))
                {
                    logger.Warning("You must provide login.");

                    return;
                }

                if (string.IsNullOrEmpty(passwordTextField.Text))
                {
                    logger.Warning("You must provide password.");

                    return;

                }

                Account account = new Account()
                {
                    Login = loginTextField.Text,
                    Password = passwordTextField.Text
                };

                logger.Info($"Successfully logged in with " +
                    $"login \"{ account.Login }\" and password \"{ account.Password }\".");
            });

            newAccountButton.AddOnClickListener(() => SceneChanger.ChangeScene("register"));
        }
    }
}
