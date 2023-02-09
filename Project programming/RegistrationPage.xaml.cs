using Registration;
using PasswordLogic;
using WorkWithEmail;

namespace Project_programming;
public partial class RegistrationPage : ContentPage
{
    private string Password;
    private string RepeatedPassword;
    private string Email;
    private DateTime Time;
    public RegistrationPage()
    {
        InitializeComponent();
    }
    private void NameEntry_Password(object sender, TextChangedEventArgs e) => Password = passwordEntry.Text;
    private void NameEntry_Email(object sender, TextChangedEventArgs e) => Email = emailEntry.Text;
    private void NameEntry_RepeatedPassword(object sender, TextChangedEventArgs e) => RepeatedPassword = repeatedPasswordEntry.Text;
    private async void ContinueButtonIsPressed(object sender, EventArgs e)
    {

        if (Time > DateTime.Now)
        {
            await DisplayAlert("", $"We have alredy sent you messege", "Ok");
            return;
        }
        if (RegistrationLogic.IsFieldsCorrect(Password, RepeatedPassword, Email))
        {
            await DisplayAlert("Attention", "All fields must be field", "Ok");
            return;

        }
        if (RegistrationLogic.IsPasswordEquals(Password, RepeatedPassword))
        {
            int Ñonfirmationcode = PasswordLog.RandomNumberGenerator();
            int countTry = 0;
            EmailWriter.SendMessage(Email, "Ñonfirmation code", "Code: " + Ñonfirmationcode.ToString());
            Time = new DateTime();
            Time = DateTime.Now;
            Time = Time.AddMinutes(2);
            while (countTry != 3)
            {
                if (int.TryParse(await DisplayPromptAsync("Confirmation", "Please write here code, which we sent you on Email", "Send", "", maxLength: 5, keyboard: Keyboard.Numeric), out int answer))
                {
                    if (answer != Ñonfirmationcode)
                    {
                        countTry++;
                        await DisplayAlert("Attention", "Check code correctness", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Great", "You have successfully registered", "Ok");
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
        }
        else
        {
            await DisplayAlert("Sorry", "Passwords are not eaqual, please check them", "Ok");
        }
    }
}