using SysPool.API;
using SysPool.API.ResponseModels;
using SysPool.Views;

namespace SysPool
{
    public partial class LoginView : ContentPage
    {
        private readonly RestService _restService;
        Auth authService = new Auth();
        public LoginView()
        {
            InitializeComponent();
            _restService = new RestService();
        }

        private async void Login(object sender, EventArgs e)
        {
            Loader.IsVisible = true;

            try
            {
                if (string.IsNullOrEmpty(Email.Text) || string.IsNullOrEmpty(Password.Text))
                {
                    await DisplayAlert("Campos vacíos", "Por favor, rellene todos los campos", "OK");
                    Loader.IsVisible = false;
                    return;
                }

                LoginResponse result = await authService.AuthorizeAsync(Email.Text, Password.Text);

                if (result != null && result.Message == "Login successful")
                {
                    App.UserID = result.UserId;

                    await Navigation.PushAsync(new Menu());
                }
                else
                {
                    Extra.ShowToast("Email o contraseña incorrectos.");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error interno", ex.Message, "OK");
            }
            finally
            {
                Loader.IsVisible = false;
            }
        }

        private void OpenRegisterView(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterView());
        }

        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {

                bool result = await DisplayAlert("Confirm", "¿Deseas salir de la aplicación?", "Sí", "No");

                if (result)
                {
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            });

            return true;
        }
    }
}