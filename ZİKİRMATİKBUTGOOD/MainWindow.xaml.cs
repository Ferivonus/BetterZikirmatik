using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Timer = System.Timers.Timer;  // Timer alias, karışıklığı önler

namespace ZİKİRMATİKBUTGOOD
{
    public partial class MainWindow : Window
    {
        private int zikirCount = 0;
        private MediaPlayer player = new MediaPlayer();
        private DateTime lastClickTime = DateTime.Now;

        private readonly double defaultWidth = 300;
        private readonly double defaultHeight = 400;
        private readonly Timer shrinkTimer = new Timer(100);
        private bool isWindowShrinking = false;

        private readonly Brush defaultBackground = new SolidColorBrush(Color.FromRgb(17, 17, 17)); // #111111
        private readonly Brush fastClickBackground = new SolidColorBrush(Color.FromRgb(255, 0, 100)); // neon kırmızı

        private MediaPlayer backgroundPlayer = new MediaPlayer();


        public MainWindow()
        {
            InitializeComponent();

            player.Open(new Uri("Sounds/hıhıhısound.wav", UriKind.Relative));
            player.Volume = 1.0;

            // Arka plan müziği ayarla
            backgroundPlayer.Open(new Uri("Sounds/duledulediledile.wav", UriKind.Relative));
            backgroundPlayer.Volume = 0.1; // Ses seviyesi, gerektiği gibi ayarla
            backgroundPlayer.MediaEnded += BackgroundPlayer_MediaEnded; // Döngü için event
            backgroundPlayer.Play();

            shrinkTimer.Elapsed += ShrinkTimer_Elapsed;

            this.KeyDown += MainWindow_KeyDown;
        }

        private void BackgroundPlayer_MediaEnded(object? sender, EventArgs e)
        {
            backgroundPlayer.Position = TimeSpan.Zero;
            backgroundPlayer.Play();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                // Space tuşuna basılmış gibi davran, zikir arttır
                ZikirEkle();
                e.Handled = true; // event'in başka yerlere gitmesini engelle
            }
        }

        private void ZikirButton_Click(object sender, RoutedEventArgs e)
        {
            ZikirEkle();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            zikirCount = 0;
            ZikirLabel.Text = "0";
        }

        private void ZikirEkle()
        {
            zikirCount++;
            ZikirLabel.Text = zikirCount.ToString();

            var now = DateTime.Now;
            double millisecondsSinceLastClick = (now - lastClickTime).TotalMilliseconds;
            lastClickTime = now;

            double speed = 1.0;
            if (millisecondsSinceLastClick < 200)
                speed = 1.8;
            else if (millisecondsSinceLastClick < 400)
                speed = 1.4;
            else if (millisecondsSinceLastClick < 600)
                speed = 1.1;

            player.Stop();
            player.SpeedRatio = speed;
            player.Position = TimeSpan.Zero;
            player.Play();

            if (millisecondsSinceLastClick < 300)
            {
                Width += 20;
                Height += 20;

                Background = fastClickBackground;

                if (!isWindowShrinking)
                {
                    shrinkTimer.Start();
                    isWindowShrinking = true;
                }
            }
        }

        private void ShrinkTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Width > defaultWidth)
                    Width -= 10;
                if (Height > defaultHeight)
                    Height -= 10;

                // Rengi yavaşça geri al
                if (Width <= defaultWidth + 10 && Height <= defaultHeight + 10)
                    Background = defaultBackground;

                if (Width <= defaultWidth && Height <= defaultHeight)
                {
                    shrinkTimer.Stop();
                    isWindowShrinking = false;
                }
            });
        }
    }
}
