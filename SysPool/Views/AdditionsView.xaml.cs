using Newtonsoft.Json;
using SysPool.API;
using SysPool.API.ResponseModels;
using System.Collections.ObjectModel;
using The49.Maui.BottomSheet;

namespace SysPool.Views;

public partial class AdditionsView : BottomSheet
{
    private readonly RestService _restService;

    public AdditionsView()
    {
        InitializeComponent();
        _restService = new RestService();
        GetProducts();
    }


    private void SeeProductDetails(object sender, EventArgs e)
    {
        Button? button = sender as Button;
        if (button != null)
        {
            ProductsResponse? product = button.CommandParameter as ProductsResponse;
            if (product != null)
            {
                int productId = product.IdProducto;
                SelectedProduct productView = new SelectedProduct(productId);
                productView.ShowAsync();
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
            var response = await _restService.GetResource(Constants.BaseUrl + Constants.Products);

            List<ProductsResponse> products = JsonConvert.DeserializeObject<List<ProductsResponse>>(response)!;

            // Hide the loading indicator in 1 second
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