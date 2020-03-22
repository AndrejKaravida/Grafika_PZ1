using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika
{

    public partial class MainWindow : Window
    {
        public List<UIElement> executed = new List<UIElement>();
        public List<UIElement> canceled = new List<UIElement>();
        public List<Point> polygon;
        public string ObjectSelected { get; set; } = "";

        public MainWindow()
        {
            InitializeComponent();
            polygon = new List<Point>();
        }

        private void elipse_Click(object sender, RoutedEventArgs e)
        {
            ObjectSelected = "Ellipse";
            elipsebtn.Background = Brushes.LightBlue;
            rectanglebtn.Background = Brushes.LightGray;
            polygonbtn.Background = Brushes.LightGray;
            imagebtn.Background = Brushes.LightGray;

            elipsebtn.BorderThickness = new Thickness(2);
            imagebtn.BorderThickness = new Thickness(1);
            rectanglebtn.BorderThickness = new Thickness(1);
            polygonbtn.BorderThickness = new Thickness(1);

            ClearDots();
        }
        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            ObjectSelected = "Rectangle";
            rectanglebtn.Background = Brushes.LightBlue;
            elipsebtn.Background = Brushes.LightGray;
            polygonbtn.Background = Brushes.LightGray;
            imagebtn.Background = Brushes.LightGray;

            imagebtn.BorderThickness = new Thickness(1);
            elipsebtn.BorderThickness = new Thickness(1);
            rectanglebtn.BorderThickness = new Thickness(2);
            polygonbtn.BorderThickness = new Thickness(1);

            ClearDots();
        }
        private void polygon_Click(object sender, RoutedEventArgs e)
        {
            ObjectSelected = "Polygon";
            polygonbtn.Background = Brushes.LightBlue;
            elipsebtn.Background = Brushes.LightGray;
            rectanglebtn.Background = Brushes.LightGray;
            imagebtn.Background = Brushes.LightGray;

            imagebtn.BorderThickness = new Thickness(1);
            elipsebtn.BorderThickness = new Thickness(1);
            rectanglebtn.BorderThickness = new Thickness(1);
            polygonbtn.BorderThickness = new Thickness(2);
        }
        private void image_Click(object sender, RoutedEventArgs e)
        {
            ObjectSelected = "Image";
            polygonbtn.Background = Brushes.LightGray;
            elipsebtn.Background = Brushes.LightGray;
            rectanglebtn.Background = Brushes.LightGray;
            imagebtn.Background = Brushes.LightBlue;

            imagebtn.BorderThickness = new Thickness(2);
            elipsebtn.BorderThickness = new Thickness(1);
            rectanglebtn.BorderThickness = new Thickness(1);
            polygonbtn.BorderThickness = new Thickness(1);

            ClearDots();
        }
        private void undo_Click(object sender, RoutedEventArgs e)
        {
            if (executed.Count != 0)
            {
                if (myCanvas.Children.Contains(executed[executed.Count - 1]))
                {
                    myCanvas.Children.Remove(executed[executed.Count - 1]);
                    canceled.Add(executed[executed.Count - 1]);
                    executed.RemoveAt(executed.Count - 1);
                }
                else
                {
                    while (!myCanvas.Children.Contains(executed[executed.Count - 1]))
                    {
                        myCanvas.Children.Add(executed[executed.Count - 1]);
                        canceled.Add(executed[executed.Count - 1]);
                        executed.RemoveAt(executed.Count - 1);
                        if (executed.Count == 0)
                            break;
                    }
                }
            }
            else if(polygon.Count > 0)
                ClearDots();
            else
            {
                MessageBox.Show("No objects in queue for undo!");
            }

        }
        private void redo_Click(object sender, RoutedEventArgs e)
        {
            if (canceled.Count != 0)
            {
                if (!myCanvas.Children.Contains(canceled[canceled.Count - 1]))
                {
                    myCanvas.Children.Add(canceled[canceled.Count - 1]);
                    executed.Add(canceled[canceled.Count - 1]);
                    canceled.RemoveAt(canceled.Count - 1);
                }
                else
                {
                    while (myCanvas.Children.Contains(canceled[canceled.Count - 1]))
                    {
                        myCanvas.Children.Remove(canceled[canceled.Count - 1]);
                        executed.Add(canceled[canceled.Count - 1]);
                        canceled.RemoveAt(canceled.Count - 1);
                        if (canceled.Count == 0)
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("No objects in queue for redo!");
            }
        }
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            ObjectSelected = "";
            int numOfChildren = myCanvas.Children.Count;

            for (int i = 0; i < numOfChildren; i++)
            {
                executed.Add(myCanvas.Children[i]);
            }
            ClearDots();
            myCanvas.Children.Clear();

            elipsebtn.Background = Brushes.LightGray;
            rectanglebtn.Background = Brushes.LightGray;
            polygonbtn.Background = Brushes.LightGray;
            imagebtn.Background = Brushes.LightGray;

            elipsebtn.BorderThickness = new Thickness(1);
            imagebtn.BorderThickness = new Thickness(1);
            rectanglebtn.BorderThickness = new Thickness(1);
            polygonbtn.BorderThickness = new Thickness(1);

        }
        private void myCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = Mouse.GetPosition(myCanvas);
            Point startingPoint = new Point(position.X, position.Y);
            Point pos = e.GetPosition((Canvas)sender);

            if (ObjectSelected.Length == 0)
            {
                MessageBox.Show("Please choose the object first!");
            }
            else if(ObjectSelected == "Polygon")
            {

                Rectangle dot = new Rectangle() { Width = 4, Height = 4, Fill = Brushes.Black };

                if (polygon.Count > 0)
                {
                    dot.Fill = Brushes.Black;
                    dot.Width -= 1;
                    dot.Height -= 1;

                    Line l = new Line() { X1 = polygon[polygon.Count - 1].X, Y1 = polygon[polygon.Count - 1].Y, X2 = pos.X, Y2 = pos.Y, Stroke = Brushes.Black };

                    myCanvas.Children.Add(l);
                }

                myCanvas.Children.Add(dot);
                Canvas.SetLeft(dot, pos.X - dot.Width / 2);
                Canvas.SetTop(dot, pos.Y - dot.Height / 2);

                polygon.Add(pos);
            }
            else
            {
                SelectAtributesWindow window = new SelectAtributesWindow(ObjectSelected, startingPoint);
                window.ShowDialog();
            }
        }
        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = Mouse.GetPosition(myCanvas);
            Point startingPoint = new Point(position.X, position.Y);

            if (polygon.Count >= 3 && ObjectSelected == "Polygon")
            {
                SelectAtributesWindow window = new SelectAtributesWindow("Polygon", startingPoint, null, false, null, polygon);
                window.ShowDialog();
            }
        }
        public void object_clicked(object sender, EventArgs e)
        {
            string objectCalled = sender.ToString().Split('.')[3];

            if(objectCalled.Trim().ToLower() == "ellipse")
            {
                Ellipse ellipse = sender as Ellipse;
                SelectAtributesWindow window = new SelectAtributesWindow(objectCalled.ToLower(), new Point(), ellipse, true);
                window.ShowDialog();

            }
            else if(objectCalled.Trim().ToLower() == "rectangle")
            {
                Rectangle rectangle = sender as Rectangle;    
                SelectAtributesWindow window = new SelectAtributesWindow(objectCalled.ToLower(), new Point(), rectangle, true);
                window.ShowDialog();

            }
            else if (objectCalled.Trim().ToLower() == "polygon")
            {
                Polygon polygon = sender as Polygon;
                SelectAtributesWindow window = new SelectAtributesWindow(objectCalled.ToLower(), new Point(), polygon, true);
                window.ShowDialog();
            }
            else
            {
                Image image = sender as Image;
                SelectAtributesWindow window = new SelectAtributesWindow(objectCalled.ToLower(), new Point(), null, true, image);
                window.ShowDialog();
            }

     
        }
        private void ClearDots()
        {
            if(polygon.Count > 0)
            {
                 myCanvas.Children.RemoveRange(myCanvas.Children.Count - (2 * polygon.Count - 1), 2 * polygon.Count - 1);
                 polygon.Clear();
            }
        }
    }
}
