using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotnetAsyncPrgm03
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

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAllCustomersAsync();
        }

        private async Task LoadAllCustomersAsync()
        {
            ResultList.Items.Clear();
            ResultList.Items.Add("Зареждане на клиенти...");

            await Task.Delay(500); 

            ResultList.Items.Clear();
            foreach (var c in _repository.GetAllCustomers())
                ResultList.Items.Add(c);
        }

        private async void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            ResultList.Items.Clear();
            ButtonSearch.IsEnabled = false;

            try
            {
                var ids = IdsBox.Text
                    .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out var id) ? id : -1)
                    .Where(id => id > 0)
                    .ToList();

                if (ids.Count == 0)
                {
                    MessageBox.Show("Моля, въведете валидни ID-та.");
                    return;
                }

                var foundCustomers = await _repository.FindCustomersAsync(ids);

                if (foundCustomers.Length == 0)
                {
                    ResultList.Items.Add("Няма намерени клиенти.");
                }
                else
                {
                    foreach (var c in foundCustomers)
                        ResultList.Items.Add(c);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка: {ex.Message}");
            }
            finally
            {
                ButtonSearch.IsEnabled = true;
            }
        }
    }
}