using SysPool.API;
using Newtonsoft.Json;
using SysPool.API.ResponseModels;
using System.Text;
using The49.Maui.BottomSheet;

namespace SysPool.Views;

public partial class TableView : ContentPage
{
	private readonly RestService _restService;
    private readonly HttpClient _client = new HttpClient();
    public TableView(int TableId)
	{
		InitializeComponent();

        _restService = new RestService();

        ReserveDate.MinimumDate = DateTime.Now;
        ReserveHour.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        bookingCalendar.Culture = new System.Globalization.CultureInfo("es");

        this.Appearing += async (sender, e) =>
        {
            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.Tables + TableId);
                TablesResponse table = JsonConvert.DeserializeObject<TablesResponse>(response)!;

                MesaTitle.Text = "Mesa de " + table.Tipo;

                MesaId.Text = table.MesaId.ToString();


                switch (table.Tipo)
                {
                    case "Pool":
                        table.ImagenMesa = "pooltable";
                        break;
                    case "Carambolas":
                        table.ImagenMesa = "freebilliardtable";
                        break;
                }

                MesaEstado.Text = table.Estado;
                MesaImg.Source = table.ImagenMesa;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        };
    }

    private void GoBack(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
    private async void Reserve(object sender, EventArgs e)
    {
        try
        {

            var json = new StringContent(JsonConvert.SerializeObject(new
            {
                UsuarioId = App.UserID,
                MesaId = int.Parse(MesaId.Text),
                FechaReserva = ReserveDate.Date.ToString("yyyy-MM-dd"),
                HoraReserva = ReserveHour.Time,
                Estado = "Pendiente"
            }), Encoding.UTF8, "application/json");

            var AddBookingResponse = await _client.PostAsync(Constants.BaseUrl + Constants.AddBooking, json);

            if (AddBookingResponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", $"Reserva agregada exitosamente para el {ReserveDate.Date} a las {ReserveHour.Time}.", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Error al agregar la reserva", "Ok");
            }
        }catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Ok");
        }

    }

    private void OpenAdditionsView(object sender, EventArgs e)
    {
        // Abrir el bottomsheet existente AdditionsView
        AdditionsView additionsView = new AdditionsView();
        additionsView.ShowAsync(this.Window);
    }
}