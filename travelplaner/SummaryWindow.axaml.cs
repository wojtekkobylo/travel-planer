using Avalonia.Controls;

namespace TravelPlanner
{
    public partial class SummaryWindow : Window
    {
        public SummaryWindow(string text)
        {
            InitializeComponent();
            SummaryText.Text = text;
        }
    }
}