using CitasMedicasApp.ViewModels;

namespace CitasMedicasApp.Views;

public partial class CambiarPasswordPage : ContentPage
{
    public CambiarPasswordPage(CambiarPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}