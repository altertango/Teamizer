using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace Teamizer {
    /// <summmmmmary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private string[] teaList = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "TeaList.txt");
        private string[] friendData = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "FriendList.txt");
        private Dictionary<string, Friend> friendDict = new Dictionary<string, Friend>();
        private static List<string> teas = new List<String>();
        public MainWindow() {
            InitializeComponent();
            init();
            LB1.ItemsSource = Friend.fList;
            this.Closed += new EventHandler(MainWindow_Closed);
        }
        public void init() {
            foreach (string line in friendData)
            {
                string[] lineSplit = line.Split(',');
                if (Friend.fList.Contains(lineSplit[0]))
                {
                    friendDict[lineSplit[0]].AddTea(line);
                }
                else
                {
                    friendDict.Add(lineSplit[0], new Friend(lineSplit[0]));
                    if (lineSplit.Length > 1)
                    {
                        friendDict[lineSplit[0]].AddTea(line);
                        if (!teas.Contains(lineSplit[1]))
                        {
                            teas.Add(lineSplit[1]);
                            
                        }

                        
                    }


                }
            }
        }

        private void New_Button_Click(object sender, RoutedEventArgs e) {
            string NewFriend = NewWindow.Prompt("Enter the name of the new friend:", "New Friend", inputType: NewWindow.InputType.Text);
            if (!String.IsNullOrEmpty(NewFriend))
            {
                if (Friend.fList.Contains(NewFriend))
                {
                    do
                    {
                        NewFriend = NewWindow.Prompt("That name is already in use \nEnter the name of the new friend", "New Friend", inputType: NewWindow.InputType.Text);
                    } while (Friend.fList.Contains(NewFriend));
                    
                }
                if (!String.IsNullOrEmpty(NewFriend))
                {
                    friendDict.Add(NewFriend, new Friend(NewFriend));
                    LB1.Items.Refresh();
                }
                    

            }
            


        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e) {
            List<string> delFriend = LB1.SelectedItems.Cast<String>().ToList();
            MessageBoxResult result = MessageBox.Show($"Are you shure you want to delete all selected friends?", "Delete Friends", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    foreach (string f in delFriend)
                    {
                        friendDict.Remove(f);
                        Friend.RemoveF(f);
                    }
                    LB1.Items.Refresh();

                    string msg = String.Join(", ", delFriend.ToArray()); ;
                    MessageBox.Show("Deleted Friends:\n" + msg, "My App");
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void Rate_Button_Click(object sender, RoutedEventArgs e) {
            List<string> selectedFriends = LB1.SelectedItems.Cast<String>().ToList();
            foreach (string f in selectedFriends)
            {
                string newTeaRate=RateTeaWindow.Prompt(f, teas.ToArray());
                if (!String.IsNullOrEmpty(newTeaRate))
                {
                    friendDict[f].AddTea(newTeaRate);
                    string tea = newTeaRate.Split(',')[1];
                    if (!teas.Contains(tea))
                    {
                        teas.Add(tea);
                        MessageBox.Show("New type of tea added: \n"+tea);

                    }

                }
            }
            //NewWindow.Prompt("Enter the name of the new friend:", "New Friend", inputType: NewWindow.InputType.Text);
        }

        private void Info_Button_Click(object sender, RoutedEventArgs e) {
            List<string> selectedFriends = LB1.SelectedItems.Cast<String>().ToList();
            foreach (string f in selectedFriends)
            {
                MessageBox.Show(friendDict[f].ToString());
            }
                

        }

        private void Party_Button_Click(object sender, RoutedEventArgs e) {
            List<string> selectedFriends = LB1.SelectedItems.Cast<String>().ToList();
            Dictionary<string, double> teaRatings = new Dictionary<string, double>();
            List<int> auxList = new List<int>();
            foreach (string t in teaList)
            {
                foreach (string f in selectedFriends)
                {
                    if (friendDict[f].teaRatings.ContainsKey(t))
                    {
                        auxList.Add(friendDict[f].teaRatings[t]);
                    }
                }
                if (auxList.Any())
                {
                    teaRatings.Add(t, auxList.Average());
                    auxList.Clear();
                }
            }
            string msg = "Raking of teas for this group: \n";
            var sortedDict = from entry in teaRatings orderby entry.Value descending select entry;
            foreach (KeyValuePair<String, double> kvp in sortedDict)
            {
                msg += $"{kvp.Key}: {kvp.Value} points\n";
            }
            MessageBox.Show(msg);
        }
        void MainWindow_Closed(object sender, EventArgs e) {
            string text = "";
            string path = AppDomain.CurrentDomain.BaseDirectory + "FriendList.txt";
            foreach (KeyValuePair<string,Friend> f in friendDict)
            {
                foreach (KeyValuePair<string,int> r in f.Value.teaRatings)
                {
                    text += f.Key + "," + r.Key + "," + r.Value+"\n";
                }
            }
            File.WriteAllText(path, text);
        }
    }
}
