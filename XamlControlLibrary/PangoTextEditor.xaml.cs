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

using System.Windows.Controls;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// A small editor control tailored for editing Pango markup.
/// 
/// This control is a thin wrapper around an inner text editor control (named
/// <c>textBox</c>) and exposes common editing actions (open/save/new/undo/redo)
/// and simple markup helpers (bold/italic) which delegate to the inner editor.
/// </summary>
public partial class PangoTextEditor : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PangoTextEditor"/> control
    /// and loads the component XAML.
    /// </summary>
    public PangoTextEditor()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the textual content of the embedded editor.
    /// </summary>
    /// <remarks>
    /// This forwards to the inner control's <c>Value</c> property.
    /// Use this property to read or programmatically set the current Pango markup.
    /// </remarks>
    public string Value
    {
        get => textBox.Value;
        set => textBox.Value = value;
    }

    /// <summary>
    /// Handler for the Open button click. Opens a file using the embedded editor's
    /// file-open logic.
    /// </summary>
    private void btnOpen_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.OpenFile();

    /// <summary>
    /// Handler for the Save button click. Saves the current content using the
    /// embedded editor's file-save logic.
    /// </summary>
    private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.SaveFile();

    /// <summary>
    /// Handler for the New button click. Resets the editor content to a minimal
    /// Pango template.
    /// </summary>
    private void btnNew_Click(object sender, System.Windows.RoutedEventArgs e) => Value = "pango:<span>\n</span>";

    /// <summary>
    /// Handler for the Undo button click. Requests an undo operation on the inner editor.
    /// </summary>
    private void btnUnDo_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.Undo();

    /// <summary>
    /// Handler for the Redo button click. Requests a redo operation on the inner editor.
    /// </summary>
    private void btnReDo_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.Redo();

    /// <summary>
    /// Handler for the Bold button click. Wraps the current selection with a &lt;b&gt; tag.
    /// </summary>
    private void btnBold_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.AddTagToSelection("b");

    /// <summary>
    /// Handler for the Italic button click. Wraps the current selection with an &lt;i&gt; tag.
    /// </summary>
    private void btnItalic_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.AddTagToSelection("i");
}
