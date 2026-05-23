// copyright (c) 2021-2026 Roberto Ceccarelli - Casasoft
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
/// A lightweight image viewer control that supports interactive transformations:
/// pan (drag), zoom, rotation and horizontal/vertical flipping.
/// The control hosts an internal <see cref="Image"/> which is exposed via the <see cref="Image"/> property.
/// </summary>
public partial class ImageViewer : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageViewer"/> control.
    /// </summary>
    public ImageViewer()
    {
        InitializeComponent();
    }

    #region properties
    /// <summary>
    /// Gets or sets the <see cref="ImageSource"/> displayed by the viewer.
    /// Setting this updates the internal <see cref="Image.Source"/>.
    /// </summary>
    public ImageSource Source
    {
        get => img.Source;
        set => img.Source = value;
    }

    /// <summary>
    /// Gets the internal <see cref="Image"/> element used to render the <see cref="Source"/>.
    /// Exposed for advanced customization (events, stretch, etc.) without replacing the viewer.
    /// </summary>
    public Image Image => img;

    /// <summary>
    /// Gets or sets the background brush applied to the control's border.
    /// Useful to change the viewer background when the image does not fill the control.
    /// </summary>
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

    /// <summary>
    /// Applies the current transformation state (translation, flips, rotation, zoom)
    /// to the internal <see cref="System.Windows.UIElement.RenderTransform"/>.
    /// The transforms are composed into a <see cref="TransformGroup"/> and applied with
    /// a render origin centered on the image.
    /// </summary>
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
    /// <summary>
    /// Toggles a horizontal flip of the displayed image and reapplies transforms.
    /// </summary>
    public void FlipHorizontal()
    {
        isFlipH = !isFlipH;
        ApplyTrans();
    }

    /// <summary>
    /// Toggles a vertical flip of the displayed image and reapplies transforms.
    /// </summary>
    public void FlipVertical()
    {
        isFlipV = !isFlipV;
        ApplyTrans();
    }

    /// <summary>
    /// Rotates the image by the specified angle in degrees.
    /// Positive values rotate clockwise; negative values rotate counter-clockwise.
    /// Rotation is normalized into the range [0, 360).
    /// </summary>
    /// <param name="angle">Amount to rotate in degrees.</param>
    public void Rotate(int angle)
    {
        rotation += angle;
        if (rotation < 0) rotation += 360;
        if (rotation >= 360) rotation -= 360;
        ApplyTrans();
    }

    /// <summary>
    /// Adjusts the zoom factor by <paramref name="delta"/> and reapplies transforms.
    /// The zoom is constrained so that resulting zoom remains greater than 0.2.
    /// </summary>
    /// <param name="delta">Delta value to add to the current zoom (e.g. 0.2 or -0.2).</param>
    public void Zoom(double delta)
    {
        if (zoom + delta > .2)
        {
            zoom += delta;
            ApplyTrans();
        }
    }

    /// <summary>
    /// Resets all transformations to the default state:
    /// no flips, zero rotation, unit zoom and reset pan origin.
    /// </summary>
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

    /// <summary>
    /// Mouse wheel handler: zooms in when delta &gt; 0, zooms out otherwise.
    /// </summary>
    private void img_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        => Zoom(e.Delta > 0 ? .2 : -.2);

    bool isMoving = false;

    /// <summary>
    /// Begins a pan operation by capturing the start mouse position and switching the cursor.
    /// </summary>
    private void img_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        start = e.GetPosition(img);
        this.Cursor = Cursors.Hand;
        isMoving = true;
    }

    /// <summary>
    /// Ends the pan operation and restores the default cursor.
    /// </summary>
    private void img_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        isMoving = false;
        this.Cursor = Cursors.Arrow;
    }

    /// <summary>
    /// Continues the pan operation while the mouse is moved during dragging.
    /// The image origin is adjusted by the movement delta and transforms are reapplied.
    /// </summary>
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
