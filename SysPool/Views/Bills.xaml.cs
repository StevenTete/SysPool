using Newtonsoft.Json;
using SysPool.API;
using SysPool.API.ResponseModels;

namespace SysPool.Views;

public partial class Bills : ContentPage
{
    private readonly RestService _restService;
    public Bills()
	{
		InitializeComponent();

        _restService = new RestService();

        this.Appearing += async (sender, e) =>
        {
            Loader.IsVisible = true;

            try
            {
                string response = await _restService.GetResource(Constants.BaseUrl + Constants.UserBills + App.UserID);

                if (response == "0")
                {
                    noBills.IsVisible = true;
                    return;
                }

                noBills.IsVisible = false;

                List<BillsResponse> bills = JsonConvert.DeserializeObject<List<BillsResponse>>(response)!;

                BindingContext = bills;

                Loader.IsVisible = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert(Title, ex.Message, "OK");
            }
        };
    }
}