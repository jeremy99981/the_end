using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows;
using TheEnd.Core.Models;
using TheEnd.Core.Services;

namespace TheEnd.App;

public partial class MainWindow : Window
{
    private static readonly RoutedUICommand ClearCommand = new("Effacer", "Clear", typeof(MainWindow));
    private readonly DraftStore _draftStore = new();
    private readonly DispatcherTimer _saveTimer;
    private bool _restoring;

    public MainWindow()
    {
        InitializeComponent();
        CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, (_, _) => PrintClick(this, new RoutedEventArgs())));
        CommandBindings.Add(new CommandBinding(ClearCommand, (_, _) => ClearClick(this, new RoutedEventArgs())));
        InputBindings.Add(new KeyBinding(ApplicationCommands.Print, new KeyGesture(Key.P, ModifierKeys.Control)));
        InputBindings.Add(new KeyBinding(ClearCommand, new KeyGesture(Key.Delete, ModifierKeys.Control | ModifierKeys.Shift)));
        DateText.Text = FrenchDateFormatter.Format(DateTime.Today);
        _saveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(700) };
        _saveTimer.Tick += async (_, _) => { _saveTimer.Stop(); await SaveDraftAsync(); };
        Loaded += LoadedAsync;
        Closing += (_, _) => SaveDraftAsync().GetAwaiter().GetResult();
    }

    private async void LoadedAsync(object sender, RoutedEventArgs e)
    {
        var draft = await _draftStore.LoadAsync();
        if (draft is null || draft.IsEmpty) return;
        var answer = MessageBox.Show("Un brouillon a été retrouvé. Voulez-vous le restaurer ?", "The End", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (answer != MessageBoxResult.Yes) return;
        _restoring = true;
        TeammateTextBox.Text = draft.Teammate; RemainingTextBox.Text = draft.RemainingTasks; GoalsTextBox.Text = draft.TomorrowGoals;
        _restoring = false;
    }

    private void InputChanged(object sender, TextChangedEventArgs e) { if (!_restoring) { _saveTimer.Stop(); _saveTimer.Start(); } }
    private Task SaveDraftAsync() => _draftStore.SaveAsync(new HandoverDraft(TeammateTextBox.Text, RemainingTextBox.Text, GoalsTextBox.Text));
    private HandoverDraft CurrentDraft => new(TeammateTextBox.Text, RemainingTextBox.Text, GoalsTextBox.Text);

    private void ClearClick(object sender, RoutedEventArgs e)
    {
        if (CurrentDraft.IsEmpty || MessageBox.Show("Effacer les informations saisies ?", "The End", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
        TeammateTextBox.Clear(); RemainingTextBox.Clear(); GoalsTextBox.Clear(); _draftStore.Delete();
    }

    private void PreviewClick(object sender, RoutedEventArgs e)
    {
        var preview = new Window { Title = "Aperçu — The End", Width = 850, Height = 1000, Owner = this, Background = Brushes.Gray };
        preview.Content = new DocumentViewer { Document = PrintDocumentFactory.Create(CurrentDraft, DateTime.Today) };
        preview.ShowDialog();
    }

    private void PrintClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;
            dialog.PrintDocument(PrintDocumentFactory.Create(CurrentDraft, DateTime.Today).Paginator, "The End — transmission de fin de journée");
            if (MessageBox.Show("Impression envoyée. Effacer la fiche pour préparer une nouvelle transmission ?", "The End", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ClearClick(sender, e);
        }
        catch (Exception ex) when (ex is InvalidOperationException or Win32Exception)
        { MessageBox.Show($"L’impression n’a pas pu être lancée.\n\n{ex.Message}", "The End", MessageBoxButton.OK, MessageBoxImage.Error); }
    }
}
