using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SheshBesh
{
    public class UiSpikeElement : Label
    {
        public int Row { get; }
        public int Column { get; }

        public UiSpikeElement(int row, int column)
        {
            Row = row;
            Column = column;
            Background = new ImageBrush
            {
                ImageSource = GetBackgroundImage(row, column)
            };
        }

        public UiSpikeElement(int row, int column, int soldierCount, bool black)
        {
            Row = row;
            Column = column;
            Update(soldierCount, black);
            BorderBrush = new SolidColorBrush(Colors.Black);
            Background = new ImageBrush
            {
                ImageSource = GetBackgroundImage(row, column)
            };
        }

        public void Update(int soldierCount, bool black)
        {
            string path = $"Images/{(black ? "Black" : "White")}Player.png";
            StackPanel stackPanel = new StackPanel();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative); 
            image.EndInit();
            for (int i = 0; i < soldierCount; i++)
            {
                stackPanel.Children.Add(new Image{Source = image});
            }
            Content = stackPanel;
            VerticalContentAlignment = Row == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }
        
        public void Update(Spike spike)
        {
            string path = $"Images/{(spike.Black ? "Black" : "White")}Player.png";
            StackPanel stackPanel = new StackPanel();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative); 
            image.EndInit();
            for (int i = 0; i < spike.SoldiersCount; i++)
            {
                stackPanel.Children.Add(new Image{Source = image});
            }
            Content = stackPanel;
            VerticalContentAlignment = Row == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            BorderThickness = new Thickness(spike.Marked ? 10 : 0);
        }

        private BitmapImage GetBackgroundImage(int row, int column)
        {
            string path = $"Images/{((column + row) % 2 == 0 ? "Brown" : "Black")}Spike.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }

        public override string ToString()
        {
            return $"Row: {Row}, Column: {Column}";
        }
    }
}