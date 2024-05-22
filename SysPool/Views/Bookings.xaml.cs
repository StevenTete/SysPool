using Newtonsoft.Json;
using SysPool.API;
using SysPool.API.ResponseModels;
using System.Collections.ObjectModel;

namespace SysPool.Views;

public partial class Bookings : ContentPage
{
    private readonly RestService _restService;
    public Bookings()
    {
        InitializeComponent();
        _restService = new RestService();

        this.Appearing += async (sender, e) =>
        {
            Loader.IsVisible = true;
            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.Bookings + App.UserID);

                List<BookingsResponse> bookings = JsonConvert.DeserializeObject<List<BookingsResponse>>(response);
                ObservableCollection<BookingsResponse> bookingsCollection = new ObservableCollection<BookingsResponse>(bookings);

                foreach (BookingsResponse booking in bookingsCollection)
                {
                    switch (booking.Estado)
                    {
                        case "Pendiente":
                            booking.ColorEstado = "#ffaa00";
                            break;
                        case "Cancelada":
                            booking.ColorEstado = "#DB3535";
                            break;
                        case "Finalizada":
                            booking.ColorEstado = "#208D2B";
                            break;
                    }

                    string responseTipoMesa = await _restService.GetResource(Constants.BaseUrl + Constants.Tables + booking.MesaId);
                    TablesResponse tableResponse = JsonConvert.DeserializeObject<TablesResponse>(responseTipoMesa);
                    booking.TipoMesa = tableResponse.Tipo;
                    booking.ImagenMesa = "main" + tableResponse.Tipo.ToLower() + ".jpg";
                }

                BindingContext = bookingsCollection;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                Loader.IsVisible = false;
            }
        };
    }
}
