using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grafika
{
    public partial class SelectAtributesWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string componentType = "";
        private string headline;
        private string objwidth;
        private string objheight;
        private Color fillColor;
        private Color borderColor;
        private string objBorderThickness;
        private string imagePath;

        public void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public string Headline
        {
            get
            {
                return headline;
            }
            set
            {
                if (value != headline)
                {
                    headline = value;
                    OnPropertyChanged("Headline");
                }
            }
        }
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                if (value != imagePath)
                {
                    imagePath = value;
                    OnPropertyChanged("ImagePath");
                }
            }
        }
        public string ObjWidth
        {
            get
            {
                return objwidth;
            }
            set
            {
                if (value != objwidth)
                {
                    objwidth = value;
                    OnPropertyChanged("ObjWidth");
                }
            }
        }
        public string ObjHeight
        {
            get
            {
                return objheight;
            }
            set
            {
                if (value != objheight)
                {
                    objheight = value;
                    OnPropertyChanged("ObjHeight");
                }
            }
        }
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                if (value != fillColor)
                {
                    fillColor = value;
                    OnPropertyChanged("FillColor");
                }
            }
        }
        public Color BorderColor { get => borderColor; set => borderColor = value; }
        public string ObjBorderThickness
        {
            get
            {
                return objBorderThickness;
            }
            set
            {
                if (value != objBorderThickness)
                {
                    objBorderThickness = value;
                    OnPropertyChanged("ObjBorderThickness");
                }
            }
        }
        public double StartingX { get; set; }
        public double StartingY { get; set; }
        public Image CanvasImage { get; set; }
        public Shape Shape { get; set; }
        public Image ImageSelected { get; set; }
        public List<Point> PolygonPoints { get; set; }

        public SelectAtributesWindow(string cmpType, Point startingCoordinates, Shape shape = null, bool isChange = false, Image image = null, List<Point> points = null)
        {
            InitializeComponent();
            this.DataContext = this;
            componentType = cmpType;
            cmbBox1.ItemsSource = typeof(Colors).GetProperties();
            cmbBox2.ItemsSource = typeof(Colors).GetProperties();
            cmbBox1.SelectedIndex = 3;
            cmbBox2.SelectedIndex = 4;
            StartingX = startingCoordinates.X;
            StartingY = startingCoordinates.Y;
          
            if (points != null)
                PolygonPoints = points;

            if (cmpType.ToLower() == "polygon")
            {
                objWidth.Visibility = Visibility.Hidden;
                objHeight.Visibility = Visibility.Hidden;
                widthtextblock.Visibility = Visibility.Hidden;
                heighttextblock.Visibility = Visibility.Hidden;
            }

            if (cmpType.ToLower() == "ellipse")
            {
                widthtextblock.Text = "RadiusX:";
                heighttextblock.Text = "RadiusY:";
            }

            if (cmpType.ToLower() == "image")
            {
                cmbBox1.Visibility = Visibility.Hidden;
                cmbBox2.Visibility = Visibility.Hidden;
                objborderThickness.Visibility = Visibility.Hidden;
                borderthicknesstextblock.Visibility = Visibility.Hidden;
                bordercolortextblock.Visibility = Visibility.Hidden;
                fillcolortextblock.Visibility = Visibility.Hidden;
                objborderThickness.Visibility = Visibility.Hidden;
                borderthicknesstextblock.Visibility = Visibility.Hidden;
            }

            if (isChange)
            {
                Headline = "Change a " + cmpType;
                changebtn.Visibility = Visibility.Visible;
                drawbtn.Visibility = Visibility.Hidden;
                objWidth.IsReadOnly = true;
                objHeight.IsReadOnly = true;

                if (shape != null)
                {
                    Shape = shape;
                    loadimagebutton.Visibility = Visibility.Hidden;
                    ObjBorderThickness = shape.StrokeThickness.ToString();
                    ObjWidth = shape.Width.ToString();
                    ObjHeight = shape.Height.ToString();
                    cmbBox1.SelectedIndex = Int32.Parse(shape.Name.Split('_')[1]);
                    cmbBox2.SelectedIndex = Int32.Parse(shape.Name.Split('_')[2]);

                }
                else
                {
                    ImageSelected = image;
                    ObjWidth = image.Width.ToString();
                    ObjHeight = image.Height.ToString();              
                }
            }
            else
            {
                Headline = "Draw a " + cmpType;
                changebtn.Visibility = Visibility.Hidden;
                drawbtn.Visibility = Visibility.Visible;

                if(cmpType.ToLower() != "image")
                    loadimagebutton.Visibility = Visibility.Hidden;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void draw_Click(object sender, RoutedEventArgs e)
        {
            bool validationError = false;

            if(objWidth.Text == "" && objWidth.Visibility == Visibility.Visible)
            {
                objWidth.BorderBrush = Brushes.Red;
                validationError = true;
            }
            else
                objWidth.BorderBrush = Brushes.Black;

            if (objHeight.Text == "" && objHeight.Visibility == Visibility.Visible)
            {
                objHeight.BorderBrush = Brushes.Red;
                validationError = true;
            }
            else
                objHeight.BorderBrush = Brushes.Black;

            if (objborderThickness.Visibility == Visibility.Visible && objborderThickness.Text == "")
            {
                objborderThickness.BorderBrush = Brushes.Red;
                validationError = true;
            }
            else
                objborderThickness.BorderBrush = Brushes.Black;

            double result1;
            double result2;
            double result3;

            if ((!Double.TryParse(objWidth.Text, out result1) || result1 <= 0) && objWidth.Visibility == Visibility.Visible)
            {
                validationError = true;
                objWidth.BorderBrush = Brushes.Red;
            }
            else
                objWidth.BorderBrush = Brushes.Black;

            if ((!Double.TryParse(objHeight.Text, out result2) || result2 <= 0) && objHeight.Visibility == Visibility.Visible)
            {
                validationError = true;
                objHeight.BorderBrush = Brushes.Red;
            }
            else
                objHeight.BorderBrush = Brushes.Black;

            if ((!Double.TryParse(objborderThickness.Text, out result3) || result3 <= 0) && objborderThickness.Visibility == Visibility.Visible)
            {
                validationError = true;
                objborderThickness.BorderBrush = Brushes.Red;
            }
            else
                objborderThickness.BorderBrush = Brushes.Black;

            if (validationError)
                MessageBox.Show("You need to enter valid data !");
            else
            {
                MainWindow main = ((MainWindow)Application.Current.MainWindow);
                SolidColorBrush fillColor = new SolidColorBrush(FillColor);
                SolidColorBrush borderColor = new SolidColorBrush(BorderColor);

                if(componentType == "Rectangle")
                {
                    Rectangle rectangle = new Rectangle();

                    rectangle.Name = "rectangle_" + cmbBox1.SelectedIndex.ToString() + "_" + cmbBox2.SelectedIndex.ToString();
                    rectangle.Fill = fillColor;
                    rectangle.Stroke = borderColor;
                    rectangle.Width = Double.Parse(objWidth.Text);
                    rectangle.Height = Double.Parse(objHeight.Text);
                    rectangle.StrokeThickness = Double.Parse(objborderThickness.Text);
                    main.myCanvas.Children.Add(rectangle);
                    main.executed.Add(rectangle);
                    Canvas.SetLeft(rectangle, StartingX);
                    Canvas.SetTop(rectangle, StartingY);                             
                    rectangle.MouseLeftButtonUp += main.object_clicked;

                    this.Close();
                }

                if(componentType == "Ellipse")
                {
                    Ellipse ellipse = new Ellipse();

                    ellipse.Name = "ellipse_" + cmbBox1.SelectedIndex.ToString() + "_" + cmbBox2.SelectedIndex.ToString();
                    ellipse.Fill = fillColor;
                    ellipse.Stroke = borderColor;
                    ellipse.Width = Double.Parse(objWidth.Text);
                    ellipse.Height = Double.Parse(objHeight.Text);
                    ellipse.StrokeThickness = Double.Parse(objborderThickness.Text);
                    main.myCanvas.Children.Add(ellipse);
                    main.executed.Add(ellipse);
                    Canvas.SetLeft(ellipse, StartingX);
                    Canvas.SetTop(ellipse, StartingY);
                    ellipse.MouseLeftButtonUp += main.object_clicked;
                   
                    this.Close();
                }

                if(componentType == "Polygon")
                {

                    Polygon polygon = new Polygon();

                    foreach (Point point in PolygonPoints)
                    {
                        polygon.Points.Add(point);
                    }

                    polygon.Name = "polygon_" + cmbBox1.SelectedIndex.ToString() + "_" + cmbBox2.SelectedIndex.ToString();
                    polygon.Fill = fillColor;
                    polygon.Stroke = borderColor;
                    polygon.StrokeThickness = Double.Parse(objborderThickness.Text);
                            
                    main.myCanvas.Children.Add(polygon);
                    main.executed.Add(polygon);
                    polygon.MouseLeftButtonUp += main.object_clicked;

                    main.myCanvas.Children.RemoveRange(main.myCanvas.Children.Count - 2 * main.polygon.Count, 2 * main.polygon.Count - 1);
                    main.polygon.Clear();

                    this.Close();
                }

                if(componentType == "Image")
                {
                    if(CanvasImage != null)
                    {
                        CanvasImage.Width = Double.Parse(objWidth.Text);
                        CanvasImage.Height = Double.Parse(objHeight.Text);
                        CanvasImage.Stretch = Stretch.Fill;
                        main.myCanvas.Children.Add(CanvasImage);
                        main.executed.Add(CanvasImage);
                        Canvas.SetLeft(CanvasImage, StartingX);
                        Canvas.SetTop(CanvasImage, StartingY);
                        CanvasImage.MouseLeftButtonUp += main.object_clicked;
                        CanvasImage.Name = "image_" + StartingX + "_" + StartingY;
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid image!");
                    }

                    this.Close();
                }
            }
        }

        private void loadimagebutton_Click(object sender, RoutedEventArgs e)
        {
            CanvasImage = new Image();
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (.jpg;.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (.png)|.png";
            if (op.ShowDialog() == true)
            {
                CanvasImage.Source = new BitmapImage(new Uri(op.FileName));
                string[] words = CanvasImage.Source.ToString().Split('/');
                int length = words.Length;
                ImagePath = words[length-1];
            }
        }

        private void cmbBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillColor = (Color)(cmbBox1.SelectedItem as PropertyInfo).GetValue(null, null);
        }

        private void cmbBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BorderColor = (Color)(cmbBox2.SelectedItem as PropertyInfo).GetValue(null, null);
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            bool validationError = false;

            if (objborderThickness.Visibility == Visibility.Visible && objborderThickness.Text == "")
            {
                objborderThickness.BorderBrush = Brushes.Red;
                validationError = true;
            }
            else
                objborderThickness.BorderBrush = Brushes.Black;

            double result;
            
            if ((!Double.TryParse(objborderThickness.Text, out result) || result <= 0) && objborderThickness.Visibility == Visibility.Visible)
            {
                validationError = true;
                objborderThickness.BorderBrush = Brushes.Red;
            }
            else
                objborderThickness.BorderBrush = Brushes.Black;

            if(validationError)
                MessageBox.Show("You need to enter valid data !");
            else 
            {
                SolidColorBrush fillColor = new SolidColorBrush(FillColor);
                SolidColorBrush borderColor = new SolidColorBrush(BorderColor);

                if (componentType != "image")
                {                
                    Shape.Fill = fillColor;
                    Shape.Stroke = borderColor;
                    Shape.StrokeThickness = Double.Parse(objborderThickness.Text);
                    Shape.Name = "shape_" + cmbBox1.SelectedIndex.ToString() + "_" + cmbBox2.SelectedIndex.ToString();
                }             
                else
                {
                    ImageSelected.Source = CanvasImage.Source;
                }

                Close();
            }
        }
    }
}
