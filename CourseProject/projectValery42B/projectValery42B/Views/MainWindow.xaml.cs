using projectValery42B.Data;
using System.Windows;

namespace projectValery42B.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerRepository _repository = new CustomerRepository();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllCustomers();
        }

        private void LoadAllCustomers()
        {
            ResultList.Items.Clear();
            foreach (var c in _repository.GetAllCustomers())
                ResultList.Items.Add(c);
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ResultList.Items.Clear();
            ButtonSearch.IsEnabled = false;

            var ids = IdsBox.Text
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out var id) ? id : -1)
                .Where(id => id > 0)
                .ToList();

            if (ids.Count == 0)
            {
                MessageBox.Show("Моля, въведете валидни ID-та.");
                ButtonSearch.IsEnabled = true;
                return;
            }

            Thread thread = new Thread(() =>
            {
                try
                {
                    var found = _repository.FindCustomers(ids);

                    Dispatcher.Invoke(() =>
                    {
                        ResultList.Items.Clear();
                        if (found.Length == 0)
                            ResultList.Items.Add("Няма намерени клиенти.");
                        else
                        {
                            foreach (var c in found)
                                ResultList.Items.Add(c);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Грешка: {ex.Message}");
                    });
                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        ButtonSearch.IsEnabled = true;
                    });
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Сигурни ли сте, че искате да излезете от приложението?",
                "Изход",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}