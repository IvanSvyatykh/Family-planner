using Registration;
using PasswordLogic;
using Project_programming.WorkWithEmail;

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
        if (!CheckEmailCorectness.ConnectionAvailable())
        {
            await DisplayAlert("Attention", "There is no internet connection", "Ok");
            return;
        }
        if (!CheckEmailCorectness.IsValidEmail(Email))
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
        if (!RegistrationLogic.IsFieldsCorrect(Password, RepeatedPassword, Email))
        {
            await DisplayAlert("Attention", "All fields must be field", "Ok");
            return;

        }
        if (RegistrationLogic.IsPasswordEquals(Password, RepeatedPassword))
        {
            int confirmationCode = PasswordLog.RandomNumberGenerator();
            int countTry = 0;

            if (!EmailWriter.SendMessage(Email, "Ñonfirmation code", "Code: " + confirmationCode.ToString()))
            {
                await DisplayAlert("O_o", "You  have written a non-existent password", "Ok");
                emailEntry.Text = string.Empty;
                return;
            }

            Time = new DateTime();
            Time = DateTime.Now;
            Time = Time.AddMinutes(2);

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
                        await DisplayAlert("Great", "You have successfully registered", "Ok");
                        await Shell.Current.GoToAsync("JoinFamilyPage");
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