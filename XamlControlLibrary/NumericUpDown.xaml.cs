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

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// A simple numeric up/down control exposing integer value editing with increment/decrement buttons.
/// </summary>
/// <remarks>
/// This partial class backs the XAML definition in <c>NumericUpDown.xaml</c>.
/// - The control stores the current value in the named <c>textBox</c> (defined in XAML).
/// - Values are clamped to the configured <see cref="MinValue"/> and <see cref="MaxValue"/>.
/// - Input is validated via <see cref="NumberValidationTextBox"/> (intended for PreviewTextInput).
/// </remarks>
public partial class NumericUpDown : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NumericUpDown"/> control.
    /// </summary>
    /// <remarks>
    /// Default configuration:
    /// - <see cref="MinValue"/> = <see cref="int.MinValue"/>
    /// - <see cref="MaxValue"/> = <see cref="int.MaxValue"/>
    /// - <see cref="Step"/> = 1
    /// </remarks>
    public NumericUpDown()
    {
        InitializeComponent();
        MinValue = int.MinValue;
        MaxValue = int.MaxValue;
        Step = 1;
    }

    #region properties

    /// <summary>
    /// Gets or sets the current integer value displayed by the control.
    /// </summary>
    /// <value>
    /// Setting the value updates the internal text representation and will clamp
    /// the assigned value to the range defined by <see cref="MinValue"/> and <see cref="MaxValue"/>.
    /// Getting the value converts the current text to an <see cref="int"/>.
    /// </value>
    /// <exception cref="FormatException">Thrown when the text in the underlying textbox is not a valid integer.</exception>
    /// <exception cref="OverflowException">Thrown when the text represents a number outside the range of <see cref="int"/>.</exception>
    public int Value
    {
        get => Convert.ToInt32(textBox.Text);
        set => textBox.Text = (value < MinValue ? MinValue :
            value > MaxValue ? MaxValue : value).ToString();
    }

    /// <summary>
    /// Gets or sets the minimum allowed value. Values assigned to <see cref="Value"/> will be clamped to this minimum.
    /// </summary>
    public int MinValue { get; set; }

    /// <summary>
    /// Gets or sets the maximum allowed value. Values assigned to <see cref="Value"/> will be clamped to this maximum.
    /// </summary>
    public int MaxValue { get; set; }

    /// <summary>
    /// Gets or sets the step used when incrementing or decrementing the <see cref="Value"/>.
    /// </summary>
    /// <remarks>
    /// Typical usage is 1 for single-step increments but can be set to any integer to change step granularity.
    /// </remarks>
    public int Step { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ContextMenu"/> attached to the inner text box.
    /// </summary>
    /// <remarks>
    /// This exposes the underlying <c>textBox.ContextMenu</c> defined in the XAML so callers can set or modify it.
    /// </remarks>
    public ContextMenu TBContextMenu
    {
        get => textBox.ContextMenu;
        set => textBox.ContextMenu = value; 
    }
    #endregion

    #region buttons events handling

    /// <summary>
    /// Handles the Up button click and increments the current <see cref="Value"/> by <see cref="Step"/>.
    /// </summary>
    /// <param name="sender">The source of the event (Up button).</param>
    /// <param name="e">Routed event data.</param>
    private void btnUp_Click(object sender, RoutedEventArgs e)
    {
        Value += Step;
    }

    /// <summary>
    /// Handles the Down button click and decrements the current <see cref="Value"/> by <see cref="Step"/>.
    /// </summary>
    /// <param name="sender">The source of the event (Down button).</param>
    /// <param name="e">Routed event data.</param>
    private void btnDown_Click(object sender, RoutedEventArgs e)
    {
        Value -= Step;
    }
    #endregion

    #region input validation

    /// <summary>
    /// Validates input typed into the text box, permitting only digits and a leading minus sign.
    /// </summary>
    /// <remarks>
    /// Intended to be used with the text box's <c>PreviewTextInput</c> event.
    /// The regular expression <c>[0-9-]+</c> allows digits and the '-' character.
    /// Note: additional validation (e.g., preventing multiple '-' characters or enforcing range)
    /// should be handled elsewhere if required.
    /// </remarks>
    /// <param name="sender">The text box control raising the event.</param>
    /// <param name="e">Text composition event arguments containing the input text to validate.</param>
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[0-9-]+");
        e.Handled = !regex.IsMatch(e.Text);
    }
    #endregion
}
