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

namespace KseniaNadbitova_PP1
{
    public partial class MainWindow : Window
    {
        Image[] p;
        int sel = -1;
        public MainWindow()
        {
            InitializeComponent();
            p = new[] { Img0
                , Img1, Img2, Img3 };
            Shuffle();
        }
        void Shuffle()
        {
            var rnd = new Random();
            var ids = Enumerable.Range(0, 4).OrderBy(x => rnd.Next()).ToArray();
            for (int i = 0; i < 4; i++)
            {
                p[i].Source = new BitmapImage(new Uri($"Image/{ids[i] + 1}.png", UriKind.Relative));
                p[i].Tag = ids[i];
            }
        }
        private void Clicl(object s, MouseButtonEventArgs e)
        {
            var img = (Image)s;
            int idx = Array.IndexOf(p, img);
            if (sel == -1) sel = idx;
            else if (sel != idx)
            {
                var t1 = p[sel].Source;
                p[sel].Source = img.Source;
                img.Source = t1;
                var t2 = p[sel].Tag;
                p[sel].Tag = img.Tag;
                img.Tag = t2;
                sel = -1;
                btnLogin.IsEnabled = !p.Where((x, i) => (int)x.Tag != i).Any();
            }
        }

        private void btnOpenCaptcha_Click(object sender, RoutedEventArgs e)
        {
            Windows2 win2 = new Windows2();
            win2.Show();
        }
    }
}