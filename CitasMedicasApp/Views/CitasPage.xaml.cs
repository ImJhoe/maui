using CitasMedicasApp.ViewModels;

namespace CitasMedicasApp.Views;

public partial class CitasPage : ContentPage
{
    public CitasPage(CitasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}