using System.Collections.ObjectModel;
using System.Windows.Input;
using CitasMedicasApp.Models;
using CitasMedicasApp.Services;

namespace CitasMedicasApp.ViewModels;

public class CitasViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    private DateTime _fechaInicio = DateTime.Today;
    private DateTime _fechaFin = DateTime.Today.AddDays(7);
    private Especialidad? _especialidadSeleccionada;
    private Medico? _medicoSeleccionado;
    private bool _hasResults = false;
    private string _currentUserRole = string.Empty;
    private bool _canSelectMedico = true;

    public CitasViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Consultar Citas";

        CitasList = new ObservableCollection<Cita>();
        EspecialidadesList = new ObservableCollection<Especialidad>();
        MedicosList = new ObservableCollection<Medico>();

        LoadUserInfo();

        // Comandos
        BuscarCitasCommand = new Command(async () => await BuscarCitasAsync(), CanBuscarCitas);
        ConsultarPorFechasCommand = new Command(async () => await ConsultarPorFechasAsync(), CanConsultarPorFechas);
        LimpiarCommand = new Command(LimpiarResultados);
        CargarEspecialidadesCommand = new Command(async () => await CargarEspecialidadesAsync());
        CargarMedicosCommand = new Command(async () => await CargarMedicosAsync());

        // Cargar datos iniciales
        _ = Task.Run(async () =>
        {
            await CargarEspecialidadesAsync();
            await CargarMedicosAsync();
        });
    }

    public ObservableCollection<Cita> CitasList { get; }
    public ObservableCollection<Especialidad> EspecialidadesList { get; }
    public ObservableCollection<Medico> MedicosList { get; }

    public DateTime FechaInicio
    {
        get => _fechaInicio;
        set
        {
            SetProperty(ref _fechaInicio, value);
            ((Command)ConsultarPorFechasCommand).ChangeCanExecute();
        }
    }

    public DateTime FechaFin
    {
        get => _fechaFin;
        set
        {
            SetProperty(ref _fechaFin, value);
            ((Command)ConsultarPorFechasCommand).ChangeCanExecute();
        }
    }

    public Especialidad? EspecialidadSeleccionada
    {
        get => _especialidadSeleccionada;
        set
        {
            SetProperty(ref _especialidadSeleccionada, value);
            ((Command)BuscarCitasCommand).ChangeCanExecute();
        }
    }

    public Medico? MedicoSeleccionado
    {
        get => _medicoSeleccionado;
        set
        {
            SetProperty(ref _medicoSeleccionado, value);
            ((Command)BuscarCitasCommand).ChangeCanExecute();
            ((Command)ConsultarPorFechasCommand).ChangeCanExecute();
        }
    }

    public bool HasResults
    {
        get => _hasResults;
        set => SetProperty(ref _hasResults, value);
    }

    public string CurrentUserRole
    {
        get => _currentUserRole;
        set => SetProperty(ref _currentUserRole, value);
    }

    public bool CanSelectMedico
    {
        get => _canSelectMedico;
        set => SetProperty(ref _canSelectMedico, value);
    }

    public ICommand BuscarCitasCommand { get; }
    public ICommand ConsultarPorFechasCommand { get; }
    public ICommand LimpiarCommand { get; }
    public ICommand CargarEspecialidadesCommand { get; }
    public ICommand CargarMedicosCommand { get; }

    private void LoadUserInfo()
    {
        var currentUser = _authService.GetCurrentUser();
        if (currentUser != null)
        {
            CurrentUserRole = currentUser.NombreRol;

            // Si es médico, no puede seleccionar otro médico (solo ve sus citas)
            CanSelectMedico = !CurrentUserRole.Equals("Medico", StringComparison.OrdinalIgnoreCase) &&
                             !CurrentUserRole.Equals("Doctor", StringComparison.OrdinalIgnoreCase);
        }
    }

    private bool CanBuscarCitas()
    {
        return !IsBusy && (EspecialidadSeleccionada != null || MedicoSeleccionado != null);
    }

    private bool CanConsultarPorFechas()
    {
        return !IsBusy && FechaInicio <= FechaFin;
    }

    private async Task BuscarCitasAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            CitasList.Clear();
            HasResults = false;

            var response = await _apiService.ConsultarCitasPorEspecialidadYMedicoAsync(
                EspecialidadSeleccionada?.IdEspecialidad,
                MedicoSeleccionado?.IdDoctor);

            if (response.Success && response.Data != null)
            {
                foreach (var cita in response.Data)
                {
                    CitasList.Add(cita);
                }

                HasResults = CitasList.Count > 0;

                if (!HasResults)
                {
                    await ShowAlertAsync("Sin Resultados",
                        "No se encontraron citas para los filtros seleccionados");
                }
            }
            else
            {
                await ShowAlertAsync("Error", response.Message ?? "Error al consultar citas");
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

    private async Task ConsultarPorFechasAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            CitasList.Clear();
            HasResults = false;

            if (FechaInicio > FechaFin)
            {
                await ShowAlertAsync("Error", "La fecha de inicio no puede ser mayor a la fecha fin");
                return;
            }

            var fechaInicioStr = FechaInicio.ToString("yyyy-MM-dd");
            var fechaFinStr = FechaFin.ToString("yyyy-MM-dd");

            var response = await _apiService.ConsultarCitasPorRangoFechasAsync(
                fechaInicioStr,
                fechaFinStr,
                MedicoSeleccionado?.IdDoctor);

            if (response.Success && response.Data != null)
            {
                foreach (var cita in response.Data)
                {
                    CitasList.Add(cita);
                }

                HasResults = CitasList.Count > 0;

                if (!HasResults)
                {
                    var medicoInfo = MedicoSeleccionado != null ?
                        $" para el médico {MedicoSeleccionado.NombreCompleto}" : "";

                    await ShowAlertAsync("Sin Resultados",
                        $"No se encontraron citas en el rango de fechas seleccionado{medicoInfo}");
                }
            }
            else
            {
                await ShowAlertAsync("Error", response.Message ?? "Error al consultar citas por fechas");
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

    private async Task CargarEspecialidadesAsync()
    {
        try
        {
            var response = await _apiService.ObtenerEspecialidadesAsync();

            if (response.Success && response.Data != null)
            {
                EspecialidadesList.Clear();
                foreach (var especialidad in response.Data)
                {
                    EspecialidadesList.Add(especialidad);
                }
            }
        }
        catch (Exception)
        {
            // Error silencioso para no interrumpir la carga
        }
    }

    private async Task CargarMedicosAsync()
    {
        try
        {
            var response = await _apiService.ObtenerMedicosAsync();

            if (response.Success && response.Data != null)
            {
                MedicosList.Clear();
                foreach (var medico in response.Data)
                {
                    MedicosList.Add(medico);
                }
            }
        }
        catch (Exception)
        {
            // Error silencioso para no interrumpir la carga
        }
    }

    private void LimpiarResultados()
    {
        CitasList.Clear();
        HasResults = false;
        EspecialidadSeleccionada = null;
        MedicoSeleccionado = null;
        FechaInicio = DateTime.Today;
        FechaFin = DateTime.Today.AddDays(7);
    }
}