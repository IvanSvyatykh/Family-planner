using PasswordLogic;
using Project_programming.WorkWithEmail;

namespace Project_programming;
public partial class ForgottenPasswordPage : ContentPage
{
    private string _email;
    private DateTime Time;
    public ForgottenPasswordPage()
    {
        InitializeComponent();
    }
    private void NameEntry_Email(object sender, TextChangedEventArgs e) => _email = emailEntry.Text;
    private async void SendEmail(object sender, EventArgs e)
    {
        if (!CheckEmailCorectness.ConnectionAvailable())
        {
            await DisplayAlert("Attention", "There is no internet connection", "Ok");
            return;
        }
        if (!CheckEmailCorectness.IsValidEmail(_email))
        {
            await DisplayAlert("Attention", "Are you sure that you wrote Email address?", "Continue");
            emailEntry.Text = string.Empty;
            return;
        }

        if (Time > DateTime.Now)
        {
            await DisplayAlert("", $"We have alredy sent you messege", "Ok");
            return;
        }

        int confirmationCode = PasswordLog.RandomNumberGenerator();
        int countTry = 0;

        if (!EmailWriter.SendMessage(_email, "confirmationCode code", "Code: " + confirmationCode.ToString()))
        {
            await DisplayAlert("O_o", "You  have written a non-existent password", "Ok");
            emailEntry.Text = string.Empty;
            return;
        }


        while (countTry != 3)
        {
            if (int.TryParse(await DisplayPromptAsync("Confirmation", "Please write here code, which we sent you on Email", "Send", "", maxLength: 5, keyboard: Keyboard.Numeric), out int answer))
            {
                if (answer != confirmationCode)
                {
                    countTry++;
                    await DisplayAlert("Attention", "Check code correctness", "Ok");
                }
                else
                {
                    await DisplayAlert("Great", "N+ow you can restore your password", "Ok");
                    break;
                }
            }
            else
            {
                await DisplayAlert("Attention", "Check code correctness", "Ok");
                countTry++;
            }
            if (countTry == 2)
            {
                await DisplayAlert("Attention", "You have last attempt, then you will have to wait untill two minutes will be over", "Ok");
            }
        }
        if (countTry == 3)
        {
            await DisplayAlert("Sorry", "You write wrong code three time, wait for two minutes", "Ok");
            return;
        }
        return;
    }
}