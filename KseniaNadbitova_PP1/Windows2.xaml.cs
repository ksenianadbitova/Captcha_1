using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KseniaNadbitova_PP1
{
    public partial class Windows2 : Window
    {
        private string currentTextCaptcha;
        private int textErrors = 0;
        private const int MaxErrors = 3;

        private string currentImageCaptcha;
        private int imageErrors = 0;

        private readonly Random rnd = new Random();

        public Windows2()
        {
            InitializeComponent();
            GenerateTextCaptcha();
            GenerateImageCaptcha();
        }

        private void GenerateTextCaptcha()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            char[] result = new char[5];
            for (int i = 0; i < result.Length; i++)
                result[i] = chars[rnd.Next(chars.Length)];

            currentTextCaptcha = new string(result);
            txtTextCaptcha.Text = currentTextCaptcha;
            txtTextInput.Clear();
        }

        private void btnCheckText_Click(object sender, RoutedEventArgs e)
        {
            if (textErrors >= MaxErrors) return;

            string input = txtTextInput.Text.Trim();

            if (string.Equals(input, currentTextCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                txtTextStatus.Foreground = Brushes.Green;
                txtTextStatus.Text = "Правильно!";
                DisableAll("Правильно введена текстовая капча.");
            }
            else
            {
                textErrors++;
                if (textErrors >= MaxErrors)
                {
                    txtTextStatus.Foreground = Brushes.DarkRed;
                    txtTextStatus.Text = "3 ошибки. Временно пользователь не может ничего сделать.";
                    DisableAll("Вы допустили 3 ошибки в текстовой капче.");
                }
                else
                {
                    txtTextStatus.Foreground = Brushes.DarkRed;
                    txtTextStatus.Text = $"Неверно. Ошибок: {textErrors} из {MaxErrors}. Новая капча.";
                    GenerateTextCaptcha();
                }
            }
        }

        private void GenerateImageCaptcha()
        {
            currentImageCaptcha = rnd.Next(1000, 9999).ToString();

            imgCaptcha.Source = DrawCaptchaImage(currentImageCaptcha);
            txtImageInput.Clear();
        }

        private BitmapSource DrawCaptchaImage(string text)
        {
            int width = 220;
            int height = 80;

            var visual = new DrawingVisual();
            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, width, height));

                for (int i = 0; i < 6; i++)
                {
                    var pen = new Pen(new SolidColorBrush(
                        Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256))), 1);
                    dc.DrawLine(pen,
                        new Point(rnd.Next(width), rnd.Next(height)),
                        new Point(rnd.Next(width), rnd.Next(height)));
                }

                for (int i = 0; i < 60; i++)
                {
                    var brush = new SolidColorBrush(
                        Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256)));
                    dc.DrawEllipse(brush, null,
                        new Point(rnd.Next(width), rnd.Next(height)), 1.5, 1.5);
                }

                var typeface = new Typeface(new FontFamily("Arial"),
                                            FontStyles.Italic, FontWeights.Bold, FontStretches.Normal);

                for (int i = 0; i < text.Length; i++)
                {
                    var color = Color.FromRgb((byte)rnd.Next(80, 200),
                                              (byte)rnd.Next(80, 200),
                                              (byte)rnd.Next(80, 200));
                    var ft = new FormattedText(
                        text[i].ToString(),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        typeface, 34, new SolidColorBrush(color), 96);

                    double angle = rnd.Next(-25, 25);
                    var center = new Point(25 + i * 45, height / 2.0);

                    dc.PushTransform(new RotateTransform(angle, center.X, center.Y));
                    dc.DrawText(ft, new Point(15 + i * 45, 15));
                    dc.Pop();
                }
            }

            var rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);
            rtb.Freeze();
            return rtb;
        }

        private void btnCheckImage_Click(object sender, RoutedEventArgs e)
        {
            if (imageErrors >= MaxErrors) return;

            string input = txtImageInput.Text.Trim();

            if (input == currentImageCaptcha)
            {
                txtImageStatus.Foreground = Brushes.Green;
                txtImageStatus.Text = "Правильно!";
                DisableAll("Правильно введена капча с картинками.");
            }
            else
            {
                imageErrors++;
                if (imageErrors >= MaxErrors)
                {
                    txtImageStatus.Foreground = Brushes.DarkRed;
                    txtImageStatus.Text = "3 ошибки. Временно пользователь не может ничего сделать.";
                    DisableAll("Вы допустили 3 ошибки в капче с картинками.");
                }
                else
                {
                    txtImageStatus.Foreground = Brushes.DarkRed;
                    txtImageStatus.Text = $"Неверно. Ошибок: {imageErrors} из {MaxErrors}. Новая капча.";
                    GenerateImageCaptcha();
                }
            }
        }

        private void DisableAll(string reason)
        {
            txtTextInput.IsEnabled = false;
            btnCheckText.IsEnabled = false;
            txtImageInput.IsEnabled = false;
            btnCheckImage.IsEnabled = false;

            string msg = reason + "\nВременно пользователь не может ничего сделать (3 ошибки).";
            MessageBox.Show(msg, "Блокировка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}