using Newtonsoft.Json;
using SysPool.API;
using SysPool.API.ResponseModels;
using SysPool.Local;
using System.Text;
using The49.Maui.BottomSheet;

namespace SysPool.Views;

public partial class SelectedProduct : BottomSheet
{
    private readonly RestService _restService;
    private readonly LocalDbService _dbService;


    public SelectedProduct(int productId)
    {
        InitializeComponent();

        _restService = new RestService();
        _dbService = new LocalDbService();
        getInfo(productId);
    }



    private async void AddToCart(object sender, EventArgs e)
    {
        try
        {
            // Verificamos si el id del producto a agregar ya existe en la base de datos
            List<ProductToBuy> products = await _dbService.GetProductsToBuy();
            if (products.Any(p => p.idProducto == int.Parse(ProductId.Text)))
            {
                Extra.ShowToast("El producto ya está en el carrito");
                return;
            }

            string imagePath = string.Empty;

            // Obtener la ruta de la imagen según el tipo de ImageSource
            if (ProductImage.Source is FileImageSource fileImageSource)
            {
                imagePath = fileImageSource.File;
            }
            else if (ProductImage.Source is StreamImageSource streamImageSource)
            {
                imagePath = streamImageSource.Stream.ToString()!;
            }

            await _dbService.AddProduct(new ProductToBuy
            {
                idProducto = int.Parse(ProductId.Text),
                idUsuario = 123,
                nombreProducto = ProductName.Text,
                cantidadSeleccionada = int.Parse(quantityLabel.Text),
                imagenProducto = imagePath,
                precio = double.Parse(ProductPrice.Text)
            });

            Extra.ShowToast("Product added to cart");
        }
        catch (Exception ex)
        {
            Extra.ShowToast(ex.Message);
        }
    }


    private void Counter(object sender, ValueChangedEventArgs e)
    {
        // Actualiza el valor mostrado de la cantidad
        quantityLabel.Text = e.NewValue.ToString();
    }

    private async void BuyProduct(object sender, EventArgs e)
    {
        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync( Constants.BaseUrl + Constants.Bills, new StringContent(
                JsonConvert.SerializeObject(new
                {
                    idProducto = ProductId.Text,
                    idUsuario = 123,
                    nombreProductoComprado = ProductName.Text,
                    cantidadComprada = quantityLabel.Text,
                    total = ProductPrice.Text
                }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                Extra.ShowToast("Product purchased successfully");
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            Extra.ShowToast(ex.Message);
        }
    }

    private void BackToHome(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    internal async void getInfo(int idProductoSeleccionado)
    {
        try
        {
            string response_producto = await _restService.GetResource(Constants.BaseUrl + Constants.Products + idProductoSeleccionado);
            ProductsResponse product = JsonConvert.DeserializeObject<ProductsResponse>(response_producto)!;

            string response_inventario = await _restService.GetResource($"https://9fb4-2800-484-4285-f400-69ba-39c4-dea-8efb.ngrok-free.app/api/inventario/");

            List<InventarioResponse> product_stock = JsonConvert.DeserializeObject<List<InventarioResponse>>(response_inventario)!;

            InventarioResponse productInventario = product_stock.FirstOrDefault(p => p.ProductoId == idProductoSeleccionado)!;

            BindingContext = product;
            BindingContext = product_stock;

            ProductName.Text = product?.Nombre;
            ProductId.Text = product?.IdProducto.ToString();
            ProductStock.Text = "Stock: " + productInventario?.Stock.ToString();
            ProductDescription.Text = product?.Descripcion;
            ProductImage.Source = product?.ImagenProducto;
            ProductPrice.Text = product?.PrecioVenta.ToString();

            quantityStepper.Maximum = productInventario.Stock;


        }
        catch (Exception ex)
        {
            Extra.ShowToast(ex.Message);
        }



}

}