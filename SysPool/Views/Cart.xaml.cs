using SysPool.Local;

namespace SysPool.Views;

public partial class Cart : ContentPage
{
    private readonly LocalDbService _dbService;

    public Cart()
    {
        InitializeComponent();

        _dbService = new LocalDbService();

        Task.Run(async () => productList.ItemsSource = await _dbService.GetProductsToBuy());

        calculateTotal();

    }

    private async void DeleteProduct(object sender, EventArgs e)
    {
        try
        {

            ImageButton? button = sender as ImageButton;
            int productId = (int)button.CommandParameter;
            await _dbService.DeleteProduct(productId);
            productList.ItemsSource = await _dbService.GetProductsToBuy();
            calculateTotal();
            await DisplayAlert("Success", "Product deleted from cart", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    async void calculateTotal()
    {
        var products = await _dbService.GetProductsToBuy();
        double total = 0;
        foreach (var product in products)
        {
            total += product.precio * product.cantidadSeleccionada;
        }
        subTotalLabel.Text = "Subtotal: $" + total;
        totalLabel.Text = "Total: $" + total * 1.19;
    }


    private void Logout(object sender, EventArgs e)
    {
        Navigation.PushAsync(new LoginView());
    }

    private void Checkout(object sender, EventArgs e)
    {

    }


}