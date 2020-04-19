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
        
        private const int Size = 75;

        public UiSpikeElement(int row, int column)
        {
            Row = row;
            Column = column;
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
            StackPanel stackPanel = new StackPanel();
            
            if (spike.SoldiersCount > 5)
            {
                if (Row == 1)
                {
                    stackPanel.Children.Add(new Label
                    {
                        Content = spike.SoldiersCount, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 25,
                        Foreground = spike.Black ? Brushes.White : Brushes.Black,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Height = Size, Width = Size,
                        Background = new ImageBrush
                        {
                            ImageSource = GetSoldierImage(spike), Stretch = Stretch.UniformToFill
                        }
                    });
                }

                for (int i = 0; i < 4; i++)
                {
                    stackPanel.Children.Add(new Image{Source = GetSoldierImage(spike),Height = Size,Width = Size});
                }
                if (Row == 0)
                {
                    stackPanel.Children.Add(new Label
                    {
                        Content = spike.SoldiersCount, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 25,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Foreground = spike.Black ? Brushes.White : Brushes.Black,
                        Height = Size, Width = Size,
                        Background = new ImageBrush
                        {
                            ImageSource = GetSoldierImage(spike), Stretch = Stretch.UniformToFill
                        }
                    });
                }
            }
            else
            {
                for (int i = 0; i < spike.SoldiersCount; i++)
                {
                    stackPanel.Children.Add(new Image {Source = GetSoldierImage(spike),Height = Size,Width = Size});
                }
            }

            Content = stackPanel;
            
            
            VerticalContentAlignment = Row == 0 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
            BitmapImage image = new BitmapImage();
            if (spike.Marked)
            {
                image = GetMarkedImage(Row, Column);
            }
            else if (spike.PreviewMode)
            {
                image = GetPreviewImage(Row, Column);
            }
            else
            {
                image = GetBackgroundImage(Row, Column);
            }
            Background = new ImageBrush
            {
                ImageSource = image
            };
        }

        private static BitmapImage GetSoldierImage(Spike spike)
        {
            string path = $"Images/{(spike.Black ? "Black" : "White")}Player.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();
            return image;
        }

        private BitmapImage GetPreviewImage(int row, int column)
        {
            string path = $"Images/{((column + row) % 2 == 0 ? "Brown" : "Black")}SpikeMarked.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
        }
        
        private BitmapImage GetMarkedImage(int row, int column)
        {
            string path = $"Images/Marked{((column + row) % 2 == 0 ? "Brown" : "Black")}.png";
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.Rotation = Row == 0 ? Rotation.Rotate180 : Rotation.Rotate0;
            image.EndInit();
            return image;
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