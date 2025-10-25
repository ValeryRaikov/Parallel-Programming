using System.Net.Http;
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

namespace DotnetAsyncPrgm02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            ResultBox.Text = "";
            ButtonStart.IsEnabled = false;

            if (!int.TryParse(DelayBox.Text, out int delay) || delay < 0 || delay > 10)
            {
                MessageBox.Show("Въведете валидно закъснение (0–10 секунди).");
                ButtonStart.IsEnabled = true;
                return;
            }

            if (!int.TryParse(TimeoutBox.Text, out int timeout) || timeout <= 0)
            {
                MessageBox.Show("Въведете валиден timeout (в секунди).");
                ButtonStart.IsEnabled = true;
                return;
            }

            try
            {
                string result = await GetHttpResponseWithTimeoutAsync(delay, timeout);
                ResultBox.Text = result;
            }
            catch (TaskCanceledException)
            {
                ResultBox.Text = $"Времето за отговор ({timeout} сек.) изтече!";
            }
            catch (Exception ex)
            {
                ResultBox.Text = $"Грешка: {ex.Message}";
            }
            finally
            {
                ButtonStart.IsEnabled = true;
            }
        }

        private async Task<string> GetHttpResponseWithTimeoutAsync(int delay, int timeoutSeconds)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            string url = $"https://httpbin.org/delay/{delay}";
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}