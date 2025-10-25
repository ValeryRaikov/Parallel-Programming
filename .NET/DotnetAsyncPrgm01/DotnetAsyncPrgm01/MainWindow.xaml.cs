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

namespace DotnetAsyncPrgm01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _cts;
        private bool _isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonStart_ClickAsync(object sender, RoutedEventArgs e)
        {
            // SlowWork();
            // ButtonStart.IsEnabled = false;
            // await SlowWorkAsync();
            // ButtonStart.IsEnabled = true;

            if (!_isRunning)
            {
                _isRunning = true;
                ButtonStart.Content = "Стоп";
                _cts = new CancellationTokenSource();

                try
                {
                    await SlowWorkAsync(_cts.Token);
                }
                catch (OperationCanceledException)
                {
                    LabelOutput.Content = "Прекъснато";
                }
                finally
                {
                    _isRunning = false;
                    ButtonStart.Content = "Старт";
                    _cts.Dispose();
                }
            }
            else
            {
                _cts.Cancel();
            }
        }

        // using Thread.Sleep method -> screen freezes during execution
        private void SlowWork()
        {
            for (int i = 1; i <= 100; i++)
            {
                LabelOutput.Content = i.ToString();
                Thread.Sleep(1000); 
            }
        }

        // using Task.Delay method -> screen remains responsive during execution (async method)
        private async Task SlowWorkAsync(CancellationToken token)
        {
            for (int i = 1; i <= 100; i++)
            {
                token.ThrowIfCancellationRequested();

                LabelOutput.Content = i.ToString();
                await Task.Delay(1000); 
            }

            LabelOutput.Content = "Готово!";
        }
    }
}