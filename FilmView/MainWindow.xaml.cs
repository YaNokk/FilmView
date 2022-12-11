using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FilmView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var builder = new UriBuilder("http://www.omdbapi.com");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query.Add("t", FilmInput.Text);
            query.Add("apikey", "c3002c74");
            builder.Query = query.ToString();
            string url = builder.ToString();
            try
            {
                var response = await client.GetFromJsonAsync<Movie>(url, default);
                PropertyInfo[] properties = response.GetType().GetProperties();
                var success = bool.Parse(response.Response);
                if (success)
                {
                    foreach (var key in properties)
                    {
                        if (key.Name == "Response") continue;
                        var value = response.GetType().GetProperty(key.Name).GetValue(response);
                        var KeyPanel = new StackPanel { Orientation = Orientation.Horizontal };
                        var KeyBox = new TextBlock { Text = key.Name };
                        var ValueBox = new TextBlock { Text = value.ToString() };
                        KeyPanel.Children.Add(KeyBox);
                        KeyPanel.Children.Add(ValueBox);
                        FilmPanel.Children.Add(KeyPanel);
                    }
                } else
                {
                    MessageBox.Show("Фильм не найден");
                }
            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show("Ошибка во время запрос");
            }
        }
    }
}
