using iCreator.Elements;
using iCreator.External;
using iCreator.Utils;
using SandboxApp.iCreator.Models;

namespace SandboxApp.iCreator.Controllers
{
    public class iCreatorController : IController
    {
        Button acceptButton;
        TextField loginTextField;
        TextField passwordTextField;
        ILogger logger;

        Account account;

        public void Initialize()
        {
            account = new Account();

            acceptButton.AddOnClickListener(() =>
            {
                account.Login = loginTextField.Text;
                account.Password = passwordTextField.Text;

                logger.Info($"Login: { account.Login } Password: { account.Password }");
            });
        }
    }
}
