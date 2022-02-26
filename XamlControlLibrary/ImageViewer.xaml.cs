// copyright (c) 2021-2022 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft XAML Controls Library
// https://github.com/strawberryfield/XamlControlsLibrary
// 
// Casasoft XAML Controls Library is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Casasoft XAML Controls Library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with Casasoft XAML Controls Library.  
// If not, see <http://www.gnu.org/licenses/>.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for ImageViewer.xaml
/// </summary>
public partial class ImageViewer : UserControl
{
    public ImageViewer()
    {
        InitializeComponent();
    }

    #region properties
    public ImageSource Source
    {
        get => img.Source;
        set => img.Source = value;
    }

    public Image Image => img;

    public Brush BorderBackground
    {
        get => border.Background;
        set => border.Background = value;
    }
    #endregion

    #region transformations
    private ScaleTransform flipH = new() { ScaleX = -1 };
    private bool isFlipH = false;

    private ScaleTransform flipV = new() { ScaleY = -1 };
    private bool isFlipV = false;

    private int rotation = 0;
    private double zoom = 1;

    private Point origin = new(0, 0);
    private Point start;

    private void ApplyTrans()
    {
        img.RenderTransformOrigin = new Point(0.5, 0.5);
        TransformGroup tg = new();
        tg.Children.Add(new TranslateTransform(origin.X, origin.Y));
        if (isFlipH) tg.Children.Add(flipH);
        if (isFlipV) tg.Children.Add(flipV);
        if (rotation != 0) tg.Children.Add(new RotateTransform(rotation));
        if (zoom != 1) tg.Children.Add(new ScaleTransform(zoom, zoom));
        img.RenderTransform = tg;
    }
    #endregion

    #region public methods
    public void FlipHorizontal()
    {
        isFlipH = !isFlipH;
        ApplyTrans();
    }

    public void FlipVertical()
    {
        isFlipV = !isFlipV;
        ApplyTrans();
    }

    public void Rotate(int angle)
    {
        rotation += angle;
        if (rotation < 0) rotation += 360;
        if (rotation >= 360) rotation -= 360;
        ApplyTrans();
    }

    public void Zoom(double delta)
    {
        if (zoom + delta > .2)
        {
            zoom += delta;
            ApplyTrans();
        }
    }

    public void Reset()
    {
        isFlipH = false;
        isFlipV = false;
        rotation = 0;
        zoom = 1;
        origin = new(0, 0);
        ApplyTrans();
    }
    #endregion

    #region event handlers
    private void MenuItem_FlipH_Click(object sender, System.Windows.RoutedEventArgs e) => FlipHorizontal();
    private void MenuItem_FlipV_Click(object sender, System.Windows.RoutedEventArgs e) => FlipVertical();

    private void MenuItem_R90_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(90);
    private void MenuItem_R270_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(-90);
    private void MenuItem_R180_Click(object sender, System.Windows.RoutedEventArgs e) => Rotate(180);

    private void MenuItem_Reset_Click(object sender, System.Windows.RoutedEventArgs e) => Reset();

    private void img_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        => Zoom(e.Delta > 0 ? .2 : -.2);

    bool isMoving = false;
    private void img_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        start = e.GetPosition(img);
        this.Cursor = Cursors.Hand;
        isMoving = true;
    }

    private void img_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        isMoving = false;
        this.Cursor = Cursors.Arrow;
    }

    private void img_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (isMoving)
        {
            Vector v = start - e.GetPosition(img);
            origin.X -= v.X;
            origin.Y -= v.Y;
            ApplyTrans();
        }
    }
    #endregion

}
