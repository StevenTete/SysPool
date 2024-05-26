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
    private readonly HttpClient _client;


    public SelectedProduct(int productId)
    {
        InitializeComponent();

        _restService = new RestService();
        _dbService = new LocalDbService();
        _client = new HttpClient();
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
            HttpResponseMessage response = await client.PostAsync( Constants.BaseUrl + Constants.BillDetails, new StringContent(
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


    internal async void getInfo(int idProductoSeleccionado)
    {
        try
        {
            HttpResponseMessage response_producto = await _client.GetAsync(Constants.BaseUrl + Constants.Products + idProductoSeleccionado);

            string content = await response_producto.Content.ReadAsStringAsync();

            ProductsResponse product = JsonConvert.DeserializeObject<ProductsResponse>(content)!;

            HttpResponseMessage response_inventario = await _client.GetAsync(Constants.BaseUrl + Constants.Stock);

            string response_inventario_string = await response_inventario.Content.ReadAsStringAsync();

            List<InventarioResponse> product_stock = JsonConvert.DeserializeObject<List<InventarioResponse>>(response_inventario_string);

            InventarioResponse productInventario = product_stock.FirstOrDefault(p => p.ProductoId == idProductoSeleccionado);

            BindingContext = product;
            BindingContext = product_stock;

            ProductName.Text = product.Nombre;
            ProductId.Text = product.ProductoId.ToString();
            ProductStock.Text = "(" + productInventario?.Stock.ToString() + ")";
            ProductImage.Source = product.ImagenProducto;
            ProductPrice.Text = product.PrecioVenta.ToString();
            cantidadDisponible.Text = productInventario.Stock.ToString();


        }
        catch (Exception ex)
        {
            Extra.ShowToast(ex.Message);
        }
    }
}