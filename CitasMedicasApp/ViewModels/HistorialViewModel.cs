using System.Collections.ObjectModel;
using System.Windows.Input;
using CitasMedicasApp.Models;
using CitasMedicasApp.Services;

namespace CitasMedicasApp.ViewModels;

public class HistorialViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    private string _cedulaPaciente = string.Empty;
    private HistorialClinico? _historialSeleccionado;
    private bool _hasResults = false;

    public HistorialViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Historial Clínico";

        HistorialesList = new ObservableCollection<HistorialClinico>();

        BuscarHistorialCommand = new Command(async () => await BuscarHistorialAsync(), CanBuscarHistorial);
        LimpiarCommand = new Command(LimpiarResultados);
    }

    public ObservableCollection<HistorialClinico> HistorialesList { get; }

    public string CedulaPaciente
    {
        get => _cedulaPaciente;
        set
        {
            SetProperty(ref _cedulaPaciente, value);
            ((Command)BuscarHistorialCommand).ChangeCanExecute();
        }
    }

    public HistorialClinico? HistorialSeleccionado
    {
        get => _historialSeleccionado;
        set => SetProperty(ref _historialSeleccionado, value);
    }

    public bool HasResults
    {
        get => _hasResults;
        set => SetProperty(ref _hasResults, value);
    }

    public ICommand BuscarHistorialCommand { get; }
    public ICommand LimpiarCommand { get; }

    private bool CanBuscarHistorial()
    {
        return !IsBusy && !string.IsNullOrWhiteSpace(CedulaPaciente) && CedulaPaciente.Length >= 10;
    }

    private async Task BuscarHistorialAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            HistorialesList.Clear();
            HasResults = false;

            // Validar cédula ecuatoriana (10 dígitos)
            if (CedulaPaciente.Length != 10 || !CedulaPaciente.All(char.IsDigit))
            {
                await ShowAlertAsync("Error", "Ingrese una cédula válida de 10 dígitos");
                return;
            }

            var response = await _apiService.BuscarHistorialPorCedulaAsync(CedulaPaciente);

            if (response.Success && response.Data != null)
            {
                foreach (var historial in response.Data)
                {
                    HistorialesList.Add(historial);
                }

                HasResults = HistorialesList.Count > 0;

                if (!HasResults)
                {
                    await ShowAlertAsync("Sin Resultados", "No se encontró historial clínico para la cédula ingresada");
                }
            }
            else
            {
                await ShowAlertAsync("Error", response.Message ?? "Error al buscar historial clínico");
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void LimpiarResultados()
    {
        CedulaPaciente = string.Empty;
        HistorialesList.Clear();
        HasResults = false;
        HistorialSeleccionado = null;
    }
}