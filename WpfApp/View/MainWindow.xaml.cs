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
using System.Diagnostics;
using WpfApp.ViewModel;
using WpfApp.Model;
using System.Windows.Forms;
using Syncfusion.UI.Xaml.Grid;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace WpfApp
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public progresse progre;
        Langue langue ;
        Save save;
        public MainWindow()
        {
            Process[] LocalByName = Process.GetProcessesByName("WpfApp");
            if (LocalByName.Length > 1)
            {
                Langue lang = new Langue();
                MessagePopup(lang.Translation(47));
                Close();
            }
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
            LabelFilesPrio.Content = langue.Translation(45);
            LabelLimitSize.Content = langue.Translation(46);
            ItemSettings conf = ConfigViewModel.getparam();
            SetLanguage.SelectedItem = conf.Language == "FR" ? SetFR : SetEN;
            StatePath.Text = conf.PathStates;
            SetLogExtension.SelectedItem = conf.LogExtension == "XML" ? SetXML : SetJSON;
            LogPath.Text = conf.PathLog;
            CryptoPath.Text = conf.PathCrypt;
            if(conf.CyptoExtension != null)
            {
                CryptoExtension.Text = string.Join(",", conf.CyptoExtension);
            }
            LogicielMetier.Text = conf.Buisnessoft;
            FilesPrio.Text = conf.LimitSize;

        }
        // Show info message in a popup
        private void MessagePopup(string s)
        {
            Popup p = new Popup();
            p.PopupText.Text = s;
            p.Show();
        }

        /*
         * Switch between Settings and Home view
         */
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

        /*
         * Options play | delete | pause | resume | stop
         */
        
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
            save = new Save();
            object[] selected = GRRID.SelectedItems.ToArray();
            string retour = "";
            if (selected.Length == 0)
            {
                retour = langue.Translation(26);
            }
            else
            {
                Animate("StartLaunch");
                Task task = Task.Run(() =>
                {
                    string launchId = "";
                    foreach (SaveList t in selected)
                    {
                        launchId += $"{t.Id};";
                    }
                    launchId = launchId.Remove(launchId.Length - 1);
                    retour = save.MakeASave(launchId, progre);

                });
                await task;
                Animate("EndLaunch");
            }
            MessagePopup(retour);
        }
        
        private async void Resume_Click(object sender, RoutedEventArgs e)
        {
            save.resume("all");
            ResumeButton.Visibility = Visibility.Hidden;
            PauseButton.Visibility = Visibility.Visible;
        }

        // Put save in pause
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            ResumeButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
            /*object[] selected = GRRID.SelectedItems.ToArray();
            string retour = "";
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
                save.pause(launchId);
             }*/
            save.pause("all");
        }
        // Stop save
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            save.kill("all");
            ResumeButton.Visibility = Visibility.Hidden;
            Animate("EndLaunch");
        }
        /*
         *  Find path folder in a popup
         */
        private string Finder(string p)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            return dialog.SelectedPath != "" ? dialog.SelectedPath : p;
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
            if(dialog.FileName != "")
            {
                CryptoPath.Text = dialog.FileName;
            }
        }

        // Modify grid content alignment
        private void DataGrid_AutoGeneratingColumn(object sender, Syncfusion.UI.Xaml.Grid.AutoGeneratingColumnArgs e)
        {
            e.Column.TextAlignment = TextAlignment.Center;
            e.Column.VerticalAlignment = VerticalAlignment.Center;
        }

        // Show form to add backup
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

        /*
         * SearchBar
         */
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

        // Refresh grid
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            GRRID.ItemsSource = States.GetSaveList();
        }

        public async void avancement(int subject)
        {
            Dispatcher.Invoke(() =>
            {
                LoadGridView();
                ProgressBar.Value = subject;
            });
        }

        // Modify Settings
        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            ConfigViewModel.PutInConfig(SetLanguage.SelectedItem.ToString(), StatePath.Text, LogPath.Text, SetLogExtension.SelectedItem.ToString(), CryptoPath.Text, CryptoExtension.Text, LogicielMetier.Text, FilesPrio.Text, LimitSize.Text);
            langue = new Langue();
            InitializeView();
            LoadGridView();
            HOME.Visibility = Visibility.Visible;
        }

        // Animation for play | pause | resume | stop menu
        private void Animate(string anim)
        {
            Dispatcher.Invoke(() =>
            {
                DoubleAnimation a = new DoubleAnimation();
                switch (anim)
                {
                    case "StartLaunch":
                        LaunchButton.Visibility = Visibility.Hidden;
                        PauseButton.Visibility = Visibility.Visible;
                        Border___AddNew.Visibility = Visibility.Hidden;
                        ProgressBar.Visibility = Visibility.Visible;

                        a.From = 133;
                        a.To = 193;
                        a.Duration = new Duration(TimeSpan.FromSeconds(1));
                        a.Completed += (s, e) =>
                        {
                            StopButton.Visibility = Visibility.Visible;
                        };
                        Border_Options.BeginAnimation(Border.WidthProperty, a);
                        break;

                    case "EndLaunch":
                        a.From = 193;
                        a.To = 133;
                        a.Duration = new Duration(TimeSpan.FromSeconds(1));
                        StopButton.Visibility = Visibility.Hidden;
                        Border_Options.BeginAnimation(Border.WidthProperty, a);

                        ProgressBar.Visibility = Visibility.Hidden;
                        LaunchButton.Visibility = Visibility.Visible;
                        PauseButton.Visibility = Visibility.Hidden;

                        break;
                    default:
                        break;
                }
            });

        }

        // Allow only numeric value for LimitSize textbox 
        private void LimitSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textbox = sender as System.Windows.Controls.TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");

        }

    }
}