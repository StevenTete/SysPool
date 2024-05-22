using SysPool.API;
using SysPool.API.ResponseModels;
using Newtonsoft.Json;

namespace SysPool.Views;

public partial class Account : ContentPage
{
    private readonly RestService _restService;
    public Account()
	{
		InitializeComponent();

        _restService = new RestService();

        this.Appearing += async (sender, e) =>
        {
            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.Users + App.UserID);

                UsersResponse user = JsonConvert.DeserializeObject<UsersResponse>(response);

                    profilePhoto.Source = user.Foto;
                    name.Text = user.nombre;
                    lastName.Text = user.apellidos;
                    id.Text = user.usuarioId.ToString();
                    email1.Text = user.email;
                    email2.Text = user.email;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        };
    }

    private async void Logout(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new LoginView());
    }

    private void SaveChanges(object sender, EventArgs e)
    {

    }
}