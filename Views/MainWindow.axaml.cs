using Avalonia.Controls;
using Avalonia.Interactivity;
using GameStore.Models;
using GameStore.Services;

namespace GameStore.Views;

public partial class MainWindow : Window
{
    private readonly GameService _service;
    private VideoGame? _selectedGame;

    public MainWindow(GameService service)
    {
        InitializeComponent();
        _service = service;
        LoadGames();
    }

    private void LoadGames()
    {
        GamesList.ItemsSource = _service.GetGames();
    }

    private void Add_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            _service.AddGame(NameBox.Text ?? "", GenreBox.Text ?? "",
                PlatformBox.Text ?? "", PriceBox.Text ?? "", GetStatus());
            LoadGames();
            ClearForm();
            ShowMessage("Videojuego registrado correctamente.");
        }
        catch (Exception ex) { ShowMessage(ex.Message); }
    }

    private void Update_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (_selectedGame is null)
                throw new ArgumentException("Selecciona un videojuego de la lista.");
            _service.UpdateGame(_selectedGame.Id, NameBox.Text ?? "", GenreBox.Text ?? "",
                PlatformBox.Text ?? "", PriceBox.Text ?? "", GetStatus());
            LoadGames();
            ClearForm();
            ShowMessage("Videojuego actualizado correctamente.");
        }
        catch (Exception ex) { ShowMessage(ex.Message); }
    }

    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (_selectedGame is null)
                throw new ArgumentException("Selecciona un videojuego de la lista.");
            _service.DeleteGame(_selectedGame.Id);
            LoadGames();
            ClearForm();
            ShowMessage("Videojuego eliminado correctamente.");
        }
        catch (Exception ex) { ShowMessage(ex.Message); }
    }

    private void Refresh_Click(object? sender, RoutedEventArgs e)
    {
        LoadGames();
        ShowMessage("Lista actualizada.");
    }

    private void Clear_Click(object? sender, RoutedEventArgs e)
    {
        ClearForm();
        ShowMessage("Formulario limpio.");
    }

    private void GamesList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedGame = GamesList.SelectedItem as VideoGame;
        if (_selectedGame is null) return;

        NameBox.Text = _selectedGame.Name;
        GenreBox.Text = _selectedGame.Genre;
        PlatformBox.Text = _selectedGame.Platform;
        PriceBox.Text = _selectedGame.Price.ToString(
            System.Globalization.CultureInfo.CurrentCulture);

        for (int i = 0; i < StatusBox.Items.Count; i++)
        {
            if (StatusBox.Items[i] is ComboBoxItem item &&
                string.Equals(item.Content?.ToString(), _selectedGame.Status,
                    StringComparison.OrdinalIgnoreCase))
            {
                StatusBox.SelectedIndex = i;
                break;
            }
        }
        ShowMessage($"Seleccionado: {_selectedGame.Name}");
    }

    private string GetStatus() =>
        (StatusBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Pendiente";

    private void ClearForm()
    {
        _selectedGame = null;
        GamesList.SelectedItem = null;
        NameBox.Text = "";
        GenreBox.Text = "";
        PlatformBox.Text = "";
        PriceBox.Text = "";
        StatusBox.SelectedIndex = 0;
    }

    private void ShowMessage(string message) => MessageText.Text = message;
}
