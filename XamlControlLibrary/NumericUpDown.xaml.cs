// copyright (c) 2021 Roberto Ceccarelli - Casasoft
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
/// Interaction logic for NumericUpDown.xaml
/// </summary>
public partial class NumericUpDown : UserControl
{
    public NumericUpDown()
    {
        InitializeComponent();
        MinValue = int.MinValue;
        MaxValue = int.MaxValue;
        Step = 1;
    }

    #region properties
    public int Value
    {
        get => Convert.ToInt32(textBox.Text);
        set => textBox.Text = (value < MinValue ? MinValue :
            value > MaxValue ? MaxValue : value).ToString();
    }

    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int Step { get; set; }
    #endregion

    #region buttons events handling
    private void btnUp_Click(object sender, RoutedEventArgs e)
    {
        Value += Step;
    }

    private void btnDown_Click(object sender, RoutedEventArgs e)
    {
        Value -= Step;
    }
    #endregion

    #region input validation
    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex("[0-9-]+");
        e.Handled = !regex.IsMatch(e.Text);
    }
    #endregion
}
