using SysPool.API;
using Newtonsoft.Json;
using SysPool.API.ResponseModels;

namespace SysPool.Views;

public partial class TableView : ContentPage
{
	private readonly RestService _restService;
    public TableView(int TableId)
	{
		InitializeComponent();

        _restService = new RestService();

        this.Appearing += async (sender, e) =>
        {
            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.Tables + TableId);
                TablesResponse table = JsonConvert.DeserializeObject<TablesResponse>(response);

                MesaTitle.Text = "Mesa de " + table.Tipo;

                MesaId.Text = "Mesa #" + table.MesaId.ToString();


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
}