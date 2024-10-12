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

namespace LiteDesktop.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Point point;
        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            VisualBrush vb = new VisualBrush(sender as Button);

            rt.Width = button.ActualWidth;
            rt.Height = button.ActualHeight;
            rt.Fill = vb;

            Canvas.SetLeft(rt, Canvas.GetLeft(button));
            Canvas.SetTop(rt, Canvas.GetTop(button));

            point = e.GetPosition(myGrid);
            rt.Visibility = Visibility.Visible;
            DragDrop.DoDragDrop(button, button, DragDropEffects.Move);

        }

        private void myGrid_DragOver(object sender, DragEventArgs e)
        {
            var newpoint = e.GetPosition(myGrid);
            var leftmove = newpoint.X - point.X;
            var topmove = newpoint.Y - point.Y;
            point = newpoint;
            Canvas.SetLeft(rt, Canvas.GetLeft(rt) + leftmove);
            Canvas.SetTop(rt, Canvas.GetTop(rt) + topmove);

        }

        private void myGrid_Drop(object sender, DragEventArgs e)
        {
            rt.Visibility = Visibility.Collapsed;
        }
    }
}