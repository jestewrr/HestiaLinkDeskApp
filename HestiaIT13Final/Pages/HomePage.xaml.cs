namespace HestiaIT13Final.Pages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Redirect to login if not authenticated
        Shell.Current.GoToAsync("login");
    }
}