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
using System.Threading;
using System.Windows.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace EasySaveRemote
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread temp;
        SocketClient SC;
        public MainWindow()
        {
            //initialize the remote application
            InitializeComponent();
            GRRID.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            try
            {
                SC = new SocketClient(this);
            }
            catch(Exception e)
            {
                MessagePopup("echec de la connexion a serveur");
            }
        }

        //display messagesin a pop up window
        private void MessagePopup(string s)
        {
            Popup p = new Popup();
            p.PopupText.Text = s;
            p.Show();
        }

        //delete a backup
        private void DeleteBackup_Click(object sender, RoutedEventArgs e)
        {
            object[] selected = GRRID.SelectedItems.ToArray();
            string retour = "";
            if (selected.Length == 0)
            {
            }
            else
            {
                string launchId = "";
                foreach (SaveList t in selected)
                {
                    SC.trash(t.Id);
                }
                SC.Refresh();
            }
        }

        //launch a backup
        private void LaunchBackup_Click(object sender, RoutedEventArgs e)
        {
            object[] selected = GRRID.SelectedItems.ToArray();
            string retour = "";
            if (selected.Length == 0)
            {
            }
            else
            {
                string launchId = "";
                foreach (SaveList t in selected)
                {
                    launchId += $"{t.Id};";
                }
                launchId = launchId.Remove(launchId.Length - 1);
                SC.Launch(launchId);
                SC.Refresh();
            }
        }

        //display progress bar
        public void avancement(int sub)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Value = sub;
            });
        }

        //pause the backup work
        private void Pause_Click(object sender, RoutedEventArgs e)
        {         
            SC.requete("Pause");
        }

        //refresh the remote application 
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (SC == null)
            {
                try
                {
                    SC = new SocketClient(this);
                }
                catch (Exception a)
                {
                    MessagePopup("echec de la connexion a serveur");
                    return;
                }
            }
            SC.Refresh();
        }

        //Resume the backup work
        private void Resume_Click(object send, RoutedEventArgs e)
        {
            SC.requete("resume");        
        }
        public void resume()
        {
            Dispatcher.Invoke(() =>
            {
                ResumeButton.Visibility = Visibility.Hidden;
                PauseButton.Visibility = Visibility.Visible;
            });
        }

        //delete the backup work
        public void kill()
        {
            Dispatcher.Invoke(() =>
            {
                ResumeButton.Visibility = Visibility.Hidden;
                Animate("EndLaunch");
            });
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
        }

        //Cancel the backup work
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            SC.requete("kill");          
        }

        private void SearchText_Click(object sender, RoutedEventArgs e)
        {
        }

        //refresh the data grid 
         public void  actualise(List<SaveList> p)
        {
            Dispatcher.Invoke(() =>
            {
                GRRID.ItemsSource = p;
            });
        }

        // Modify grid content alignment
        private void DataGrid_AutoGeneratingColumn(object sender, Syncfusion.UI.Xaml.Grid.AutoGeneratingColumnArgs e)
        {
            e.Column.TextAlignment = TextAlignment.Center;
            e.Column.VerticalAlignment = VerticalAlignment.Center;
        }

        //pause the backup work
        public void pause()
        {
            Dispatcher.Invoke(() =>
            {
                ResumeButton.Visibility = Visibility.Visible;
                PauseButton.Visibility = Visibility.Hidden;
            });
        }

        //Animation when you click on pause, stop or resume the backup work
        public void Animate(string anim)
        {
            Dispatcher.Invoke(() =>
            {
                DoubleAnimation a = new DoubleAnimation();
                switch (anim)
                {
                    case "StartLaunch":
                        LaunchButton.Visibility = Visibility.Hidden;
                        PauseButton.Visibility = Visibility.Visible;
                        StopButton.Visibility = Visibility.Visible;
                        ProgressBar.Visibility = Visibility.Visible;

                        a.From = 133;
                        a.To = 193;
                        a.Duration = new Duration(TimeSpan.FromSeconds(1));

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
        private void close(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            Proces.close();
        }
    }
}

