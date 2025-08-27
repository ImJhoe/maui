namespace CitasMedicasApp.Views;

public partial class ConsultarCitasPage : ContentPage
{
    public ConsultarCitasPage()
    {
        InitializeComponent();
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}