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
/// A small composite <see cref="UserControl"/> that exposes a labeled file picker text box.
/// </summary>
/// <remarks>
/// This control is a thin wrapper around an inner `fileTextBox` control and a `label` control
/// defined in the corresponding XAML file. Properties on this class forward to the inner
/// controls so consumers can set the file value, and configure the open-file dialog behavior,
/// while also providing a caption via the `Caption` property.
/// </remarks>
public partial class FileTextBoxLabel : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileTextBoxLabel"/> class
    /// and loads the associated XAML UI.
    /// </summary>
    public FileTextBoxLabel()
    {
        InitializeComponent();
    }

    #region properties

    /// <summary>
    /// Gets or sets the current file path text shown in the inner file text box.
    /// </summary>
    /// <remarks>
    /// This property forwards to the inner `fileTextBox.Value`. Setting this updates the UI;
    /// getting returns the current value from the inner control.
    /// </remarks>
    public string Value
    {
        get => fileTextBox.Value;
        set => fileTextBox.Value = value;
    }

    /// <summary>
    /// Gets or sets the filter string used by the open-file dialog opened from the inner control.
    /// </summary>
    /// <example>
    /// "Text files (*.txt)|*.txt|All files (*.*)|*.*"
    /// </example>
    public string OpenFileDialogFilter
    {
        get => fileTextBox.OpenFileDialogFilter;
        set => fileTextBox.OpenFileDialogFilter = value;
    }

    /// <summary>
    /// Gets or sets the title displayed by the open-file dialog opened from the inner control.
    /// </summary>
    public string OpenFileDialogTitle
    {
        get => fileTextBox.OpenFileDialogTitle;
        set => fileTextBox.OpenFileDialogTitle = value;
    }

    /// <summary>
    /// Gets or sets the text displayed by the label next to the file text box.
    /// </summary>
    /// <remarks>
    /// The label's content may be null; setting <see cref="Caption"/> to <c>null</c> clears the label.
    /// The getter returns the label's current content as a string (calls <c>ToString()</c> on the content).
    /// </remarks>
    public string? Caption
    {
        get => label.Content.ToString();
        set => label.Content = value;
    }
    #endregion
}
