using Newtonsoft.Json;
using SysPool.API.ResponseModels;
using SysPool.API;
using System.Collections.ObjectModel;

namespace SysPool.Views;

public partial class Home : ContentPage
{
    private readonly RestService _restService;
    public Home()
	{
		InitializeComponent();
        _restService = new RestService();

        this.Appearing += async (sender, e) =>
        {
            Loader.IsVisible = true;
            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.Tables);

                List<TablesResponse> tables = JsonConvert.DeserializeObject<List<TablesResponse>>(response)!;


                ObservableCollection<TablesResponse> tablesCollection = new ObservableCollection<TablesResponse>(tables);

                BindingContext = tablesCollection;

                Loader.IsVisible = false;

                foreach (TablesResponse table in tablesCollection)
                {
                    switch (table.Estado)
                    {
                        case "Disponible":
                            table.ColorEstado = "#208D2B";
                            break;
                        case "Ocupada":
                            table.ColorEstado = "#DB3535";
                            break;
                        case "Reservada":
                            table.ColorEstado = "#3563DB";
                            break;
                    }

                    switch (table.Tipo)
                    {
                        case "Pool":
                            table.ImagenMesa = "pooltable";
                            break;
                        case "Carambolas":
                            table.ImagenMesa = "freebilliardtable";
                            break;
                    }
                }
                string nameResponse = await _restService.GetResource(Constants.BaseUrl + Constants.Users + App.UserID);
                UsersResponse user = JsonConvert.DeserializeObject<UsersResponse>(nameResponse)!;
                WelcomeName.Text = "Bienvenido, " + user.nombre + ".";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        };
    }

    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {

            bool result = await DisplayAlert("Salir", "¿Deseas salir de la aplicación?", "Sí", "No");

            if (result)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        });

        return true;
    }

    private async void OpenTableView(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        var tableId = button?.CommandParameter as int?;

        if (tableId.HasValue)
        {
            await Navigation.PushAsync(new TableView(tableId.Value));
        }
        else
        {
            await DisplayAlert("Error", "MesaId no válido", "OK");
        }
    }
}