using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Teamizer {
    /// <summary>
    /// Interaction logic for RateTeaWindow.xaml
    /// </summary>
    public partial class RateTeaWindow : Window {
        public RateTeaWindow(string f, string[] teas) {
            Rater = f;
            Teas = teas;
            InitializeComponent();
            txtQuestion.Text = "Add a tea rating for " + f;
            TeaCB.ItemsSource = Teas;
        }
        public static string Prompt(string f, string[] teas) {
            RateTeaWindow inst = new RateTeaWindow(f,teas);
            inst.ShowDialog();
            if (inst.DialogResult == true)
                return inst.ResponseText;
            return null;

        }
        public string Rater { get; set; }
        public string[] Teas { get; set; }
        public string ResponseText
        {
            get
            {
                return Rater + "," + TeaCB.Text + "," + RateCB.Text;
            }
        }
                private void btnOk_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
