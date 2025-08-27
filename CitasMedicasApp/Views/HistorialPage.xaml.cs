using CitasMedicasApp.ViewModels;

namespace CitasMedicasApp.Views;

public partial class HistorialPage : ContentPage
{
    public HistorialPage(HistorialViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}