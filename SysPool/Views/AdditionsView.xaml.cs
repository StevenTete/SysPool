using Newtonsoft.Json;
using SysPool.API;
using SysPool.API.ResponseModels;
using System.Collections.ObjectModel;
using The49.Maui.BottomSheet;

namespace SysPool.Views;

public partial class AdditionsView : BottomSheet
{
    private readonly HttpClient _httpClient;
    private readonly RestService _restService;

    public AdditionsView()
    {
        InitializeComponent();
        _restService = new RestService();
        _httpClient = new HttpClient();
        GetProducts();
    }


    private async void SeeProductDetails(object sender, EventArgs e)
    {
        Button button = sender as Button;
        if (button != null)
        {
            ProductsResponse product = button.CommandParameter as ProductsResponse;
            if (product != null)
            {
                int productoSeleccionado = product.ProductoId;

                SelectedProduct productView = new SelectedProduct(productoSeleccionado);
                await productView.ShowAsync();
            }
        }
    }

    private async void Logout(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
    }

    private void prueba_carro(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Cart());
    }

    public async void GetProducts()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(Constants.BaseUrl + Constants.Products);
            string content = await response.Content.ReadAsStringAsync();
            List<ProductsResponse> products = JsonConvert.DeserializeObject<List<ProductsResponse>>(content)!;

            await Task.Delay(800);
            Loader.IsVisible = false;

            ObservableCollection<ProductsResponse> productsCollection = new ObservableCollection<ProductsResponse>(products);

            BindingContext = productsCollection;
        }
        catch (Exception ex)
        {
            Extra.ShowToast(ex.Message);
        }
    }
}