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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.ViewModel;
using WpfApp.Model;
using System.Windows.Forms;
using Syncfusion.UI.Xaml.Grid;

namespace WpfApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public progresse progre;
        Langue langue ;
        public MainWindow()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@31392e342e30jse9pqgnHKapAl3VwjfJIyrcv9mVvIYME0lVG0Yas+Q=");
            langue = new Langue();
            InitializeView();
            LoadGridView();
            progre = avancement;
            

            //GRRID.ItemsSource = JsonStateLog.Read();
        }

        private void LoadGridView()
        {
            GRRID.ItemsSource = States.GetSaveList();
        }
        private void InitializeView()
        {
            GRRID.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            SETTINGS.Visibility = Visibility.Hidden;
            ProgressBar.Visibility = Visibility.Hidden;
            Border___AddNew.Visibility = Visibility.Hidden;
            Encryption.Visibility= Visibility.Hidden;
            LabelTask.Content = langue.Translation(41);
            LabelSettings.Content = langue.Translation(42);
            SearchText.Text = langue.Translation(34);
            AddName.Content = langue.Translation(10);
            AddSource.Content = langue.Translation(11);
            AddDestination.Content = langue.Translation(12);
            AddBackupType.Content = langue.Translation(13);
            LabelLanguage.Content = langue.Translation(1);
            LabelStatePath.Content = langue.Translation(35);
            LabelLogPath.Content = langue.Translation(36);
            LabelCryptoSoftPath.Content = langue.Translation(37);
            LabelLogExtension.Content = langue.Translation(38);
            LabelCryptoExt.Content = langue.Translation(39);
            LabelLogicielMetier.Content = langue.Translation(43);
            Modify.Content = langue.Translation(40);
            ItemSettings conf = ConfigViewModel.getparam();
            if(conf.Language == "FR")
            {
                SetLanguage.SelectedItem = SetFR;
            }
            else
            {
                SetLanguage.SelectedItem = SetEN;
            }
            StatePath.Text = conf.PathStates;
            LogPath.Text = conf.PathLog;
            if(conf.LogExtension == "XML")
            {
                SetLogExtension.SelectedItem = SetXML;
            }
            else
            {
                SetLogExtension.SelectedItem = SetJSON;
            }
            CryptoPath.Text = conf.PathCrypt;
            if(conf.CyptoExtension != null)
            {
                CryptoExtension.Text = string.Join(",", conf.CyptoExtension);
            }
            LogicielMetier.Text = conf.buisnessoft;



        }
        private void MessagePopup(string s)
        {
            Popup p = new Popup();
            p.PopupText.Text = s;
            p.Show();
        }
        
        // Add a backup
        private void AddBackup_Click(object sender, RoutedEventArgs e)
        {
            bool Encry = Encryption.IsChecked.Value;
            MessagePopup(States.AddTask(Name.Text, Source.Text, Destination.Text, BackupType.Text, Encry));
            Name.Text = ""; Source.Text = ""; Destination.Text = ""; BackupType.Text = "";
            LoadGridView();
            Border___AddNew.Visibility = Visibility.Hidden;
            Encryption.Visibility = Visibility.Hidden;
        }
        
        // Delete a Backup
        private void DeleteBackup_Click(object sender, RoutedEventArgs e)
        {
            SaveList item = (SaveList)GRRID.SelectedItem;
            if (item == null)
            {
                MessagePopup(langue.Translation(23));
            }
            else
            {
                int id = int.Parse(item.Id);
                MessagePopup(States.DeleteStatus(id));
                LoadGridView();
            }
        }

        // Launch a backup
        private async void LaunchBackup_Click(object sender, RoutedEventArgs e)
        {
            LaunchButton.IsEnabled = false;
            Border___AddNew.Visibility = Visibility.Hidden;
            ProgressBar.Visibility = Visibility.Visible;
            Save save = new Save();
            var selected = GRRID.SelectedItems.ToArray();
            string retour = "";
            var task = Task.Run(() =>
            {
                if (selected.Length == 0)
                {
                    retour = langue.Translation(26);
                }
                else
                {
                    string launchId = "";
                    foreach (SaveList t in selected)
                    {
                        launchId += $"{t.Id};";
                    }
                    launchId = launchId.Remove(launchId.Length - 1);
                    retour = save.MakeASave(launchId, progre);
                }
            });
            await task;
            MessagePopup(retour);
            LaunchButton.IsEnabled = true;
            ProgressBar.Visibility = Visibility.Hidden;
        }

        /*
         *  Find path folder in a popup
         */
        private string Finder(string p)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            if (dialog.SelectedPath != "")
            {
                return dialog.SelectedPath;
            }
            else
            {
                return p;
            }
        }
        private void FindSource_Click(object sender, RoutedEventArgs e)
        {
            Source.Text = Finder(Source.Text);
        }
        private void FindDest_Click(object sender, RoutedEventArgs e)
        {
            Destination.Text = Finder(Destination.Text);
        }
        private void FindState_Click(object sender, RoutedEventArgs e)
        {
            StatePath.Text = Finder(StatePath.Text);
        }
        private void FindLog_Click(object sender, RoutedEventArgs e)
        {
            LogPath.Text = Finder(LogPath.Text);
        }
        private void FindCrypto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            CryptoPath.Text = dialog.FileName;
        }


        
        private void DataGrid_AutoGeneratingColumn(object sender, Syncfusion.UI.Xaml.Grid.AutoGeneratingColumnArgs e)
        {
            e.Column.TextAlignment = TextAlignment.Center;
            e.Column.VerticalAlignment = VerticalAlignment.Center;
        }

        private void ShowAddButton_Click(object sender, RoutedEventArgs e)
        {
            if(Border___AddNew.Visibility == Visibility.Visible)
            {
                Encryption.Visibility = Visibility.Hidden;
                Border___AddNew.Visibility = Visibility.Hidden;
            }
            else
            {
                Encryption.Visibility = Visibility.Visible;
                Border___AddNew.Visibility = Visibility.Visible;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchText.Text == "")
            {
                GRRID.ItemsSource = States.GetSaveList();
            }
            else
            {
                GRRID.ItemsSource = States.GetSaveList(SearchText.Text);
            }
        }
        private void SearchText_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Text = "";
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GRRID.ItemsSource = States.GetSaveList();
        }
        public async void avancement(int subject)
        {
            this.Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = subject;
            });
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            HOME.Visibility = Visibility.Hidden;
            SETTINGS.Visibility = Visibility.Visible;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            SETTINGS.Visibility = Visibility.Hidden;
            HOME.Visibility = Visibility.Visible;
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            ConfigViewModel.PutInConfig(SetLanguage.SelectedItem.ToString(), StatePath.Text, LogPath.Text, SetLogExtension.SelectedItem.ToString(), CryptoPath.Text, CryptoExtension.Text,LogicielMetier.Text);
            langue = new Langue();
            InitializeView();
            LoadGridView();
            HOME.Visibility = Visibility.Visible;
        }

    }
}