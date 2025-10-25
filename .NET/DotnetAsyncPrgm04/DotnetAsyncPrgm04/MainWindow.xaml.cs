using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotnetAsyncPrgm04
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

        private async void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Изберете файла big.txt"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            string filePath = openFileDialog.FileName;
            ResultList.Items.Clear();
            ResultList.Items.Add("Зареждане и обработка...");

            ButtonLoad.IsEnabled = false;

            try
            {
                var wordCounts = await CountWordsAsync(filePath);

                ResultList.Items.Clear();
                foreach (var kvp in wordCounts.Take(200))
                {
                    ResultList.Items.Add($"{kvp.Key,-20} {kvp.Value,5}");
                }

                ResultList.Items.Add($"--- Общо различни думи: {wordCounts.Count} ---");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Грешка при обработка: {ex.Message}");
            }
            finally
            {
                ButtonLoad.IsEnabled = true;
            }
        }

        private async Task<Dictionary<string, int>> CountWordsAsync(string filePath)
        {
            return await Task.Run(() =>
            {
                string text = File.ReadAllText(filePath);
                var matches = Regex.Matches(text.ToLower(), @"\w+");

                var wordGroups = matches
                    .Cast<Match>()
                    .GroupBy(m => m.Value)
                    .Select(g => new { Word = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToDictionary(x => x.Word, x => x.Count);

                return wordGroups;
            });
        }
    }
}