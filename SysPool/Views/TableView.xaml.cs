using SysPool.API;
using Newtonsoft.Json;
using SysPool.API.ResponseModels;
using System.Text;
using Plugin.Maui.Calendar.Models;
using System.Net.Http;

namespace SysPool.Views;

public partial class TableView : ContentPage
{
    public Dictionary<DateTime, List<EventModel>> Events { get; set; } = new Dictionary<DateTime, List<EventModel>>();
    private readonly RestService _restService;
    private readonly HttpClient _client = new HttpClient();
    private int _tableId;

    public TableView(int tableId)
    {
        InitializeComponent();
        _tableId = tableId;
        _restService = new RestService();
        ReserveDate.MinimumDate = DateTime.Now;
        ReserveHour.Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        bookingCalendar.Culture = new System.Globalization.CultureInfo("es");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await InitializeTableDetails(_tableId);
        await LoadBookingsAsync(_tableId);
    }

    private async Task InitializeTableDetails(int tableId)
    {
        try
        {
            string response = await _restService.GetResource(Constants.BaseUrl + Constants.Tables + tableId);
            TablesResponse table = JsonConvert.DeserializeObject<TablesResponse>(response)!;

            MesaTitle.Text = "Mesa de " + table.Tipo;
            MesaId.Text = table.MesaId.ToString();
            switch (table.Tipo)
            {
                case "Pool":
                    MesaImg.Source = "pooltable";
                    break;
                case "Carambolas":
                    MesaImg.Source = "freebilliardtable";
                    break;
            }
            MesaEstado.Text = table.Estado;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task LoadBookingsAsync(int tableId)
    {
        try
        {
            string response = await _client.GetStringAsync(Constants.BaseUrl + "reservas/mesa/" + tableId);
            var bookings = JsonConvert.DeserializeObject<List<BookingsResponse>>(response);

            EventCollection eventCollection = new EventCollection();

            foreach (var booking in bookings)
            {
                DateTime bookingDate = DateTime.Parse(booking.FechaFormateada);

                if (eventCollection.ContainsKey(bookingDate))
                {
                    // Copiar los eventos existentes a una nueva lista
                    var existingEvents = new List<EventModel>((IEnumerable<EventModel>)eventCollection[bookingDate]);
                    existingEvents.Add(new EventModel { Name = "Mesa ocupada a las ", Hour = booking.Hora });

                    // Reasignar la nueva lista al diccionario
                    eventCollection[bookingDate] = existingEvents;
                }
                else
                {
                    // Si no hay eventos para esa fecha, crea una nueva lista y agrega el evento
                    eventCollection[bookingDate] = new List<EventModel>
        {
            new EventModel { Name = "Mesa ocupada a las ", Hour = booking.Hora }
        };
                }
            }


            bookingCalendar.Events = eventCollection;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
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
                Fecha = ReserveDate.Date,
                Hora = ReserveHour.Time,
                Estado = "Pendiente"
            }), Encoding.UTF8, "application/json");

            var AddBookingResponse = await _client.PostAsync(Constants.BaseUrl + Constants.AddBooking, json);

            var jsonBill = new StringContent(JsonConvert.SerializeObject(new
            {
                MesaId = int.Parse(MesaId.Text),
                UsuarioId = App.UserID,
                Estado = "Pendiente",
                Impuestos = 1.19,
                Total = 0,
                Fecha = DateTime.Now,
            }), Encoding.UTF8, "application/json");

            await DisplayAlert("Info", Constants.BaseUrl + Constants.Bill, "Ok");
            var AddBillResponse = await _client.PostAsync(Constants.BaseUrl + Constants.Bill, jsonBill);

            if (AddBillResponse.IsSuccessStatusCode)
            {
                Extra.ShowToast("Factura agregada exitosamente");
            }
            else
            {
                await DisplayAlert("Error", "Error al agregar la factura", "Ok");
            }

            if (AddBookingResponse.IsSuccessStatusCode)
            {
                Extra.ShowToast($"Reserva agregada exitosamente para el {ReserveDate.Date} a las {ReserveHour.Time}.");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Error al agregar la reserva", "Ok");
            }
        }
        catch (Exception ex)
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

    public class EventModel
    {
        public required string Name { get; set; }
        public TimeSpan Hour { get; set; }
    }
}
