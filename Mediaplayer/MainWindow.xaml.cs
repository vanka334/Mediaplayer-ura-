using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Mediaplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /* FF6E4DB5*/
        public bool IsLoop = false;
        public bool IsRnd = false;
        public List<string> songs_now = new();
        public List<string> rnd_songs = new();
        public bool isPlay = true;
        public Uri pause = new Uri("C:\\Users\\Vanya\\OneDrive\\Рабочий стол\\проекти)\\Mediaplayer\\Mediaplayer\\icons8-стоп-в-круге-50 (1).png", UriKind.Absolute);
        public Uri play = new Uri("C:\\Users\\Vanya\\OneDrive\\Рабочий стол\\проекти)\\Mediaplayer\\Mediaplayer\\icons8-воспроизведение-50 (1).png", UriKind.Absolute);
        public TimeSpan ts;
        public TimeSpan tr;

        public MainWindow()
        {
            InitializeComponent();
            media.Volume = 0.5;
            
        }

        private void Song_Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Songs.SelectedIndex > 0)
            {
                Songs.SelectedIndex -= 1;
            }

        
            else
            {
               Songs.SelectedIndex = Songs.Items.Count - 1;
            }
            
        }

        private void Song_Plus_Click(object sender, RoutedEventArgs e)
        {
            if(Songs.SelectedIndex < Songs.Items.Count - 1) {
            
            Songs.SelectedIndex += 1;
            }
            else
            {
                Songs.SelectedIndex = 0;
            }
        }
        private void Play_Pause_Click(object sender, RoutedEventArgs e)
        {
           if(isPlay == true) { 
            media.Pause();
                PlayorPause.Source = new BitmapImage(play);
                isPlay= false;
            }
            else
            {
                media.Play();
                PlayorPause.Source = new BitmapImage(pause);
                isPlay= true;
            }

        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            if(IsLoop== false) {

                IsLoop = true;
                Repeat.Background = new SolidColorBrush( Color.FromRgb(110, 77, 181));
            }
            else
            {
                IsLoop = false;
                Repeat.Background = new SolidColorBrush(Color.FromRgb(50, 11, 134));
            }
        }

        private void Random_Song_Click(object sender, RoutedEventArgs e)
        {
            if (IsRnd == false)
            {

                IsRnd = true;
                Random_Song.Background = new SolidColorBrush(Color.FromRgb(110, 77, 181));
                Randomizer();


            }
            else
            {
                IsRnd = false;
                Random_Song.Background = new SolidColorBrush(Color.FromRgb(50, 11, 134));
                Songs.ItemsSource = songs_now;
                Songs.Items.Refresh();
            }

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpening();
            
            Songs.ItemsSource= songs_now;
            Songs.SelectedIndex = 0;
            media.Source = new Uri(Songs.SelectedItem.ToString());
            media.Play();
            isPlay = true;
            PlayorPause.Source = new BitmapImage(pause);
            Thread thread = new Thread( _ =>
            {
                while (true)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        sliderauto.Value = media.Position.Ticks;
                       
                        ts = media.Position;    
                        Timer.Text= string.Format("{0:00}:{1:00}/{2:00}:{3:00}",
                                    ts.Minutes, ts.Seconds,tr.Minutes,tr.Seconds);
                    }));

                 Thread.Sleep(1000);
                }
            });
            thread.Start();
            

        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(sliderauto.Value));
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            sliderauto.Maximum = media.NaturalDuration.TimeSpan.Ticks;
            tr = media.NaturalDuration.TimeSpan;

        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if(IsLoop == true)
            {
                Songs.SelectedIndex += 1;
                Songs.SelectedIndex -= 1;
            }
            else if (Songs.SelectedIndex < Songs.Items.Count - 1)
            {

                Songs.SelectedIndex += 1;
            }
           
            else
            {
                Songs.SelectedIndex = 0;
            }
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            media.Source = new Uri(Songs.SelectedItem.ToString());
            media.Play();

        }

        private void FileOpening()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog() { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(dialog.FileName, "*.mp3");
                foreach (string file in files)
                {
                    songs_now.Add(file);
                }
            }

        }
        private void Randomizer()
        {
            rnd_songs = songs_now.ToList();
            Random rand = new Random();

            for (int i = rnd_songs.Count - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                string tmp = rnd_songs[j];
                rnd_songs[j] = rnd_songs[i];
                rnd_songs[i] = tmp;
            }
            Songs.ItemsSource = rnd_songs;
            Songs.Items.Refresh();

        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            
            media.Volume = sliderVolume.Value / 10;






        }
      
    }
}
