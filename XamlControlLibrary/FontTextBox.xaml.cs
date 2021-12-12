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

using System.Windows.Controls;
using System.Windows.Input;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for FontTextBox.xaml
/// </summary>
public partial class FontTextBox : UserControl
{
    public FontTextBox()
    {
        InitializeComponent();
    }

    public string Value
    {
        get => textBox.Text;
        set => textBox.Text = value;
    }

    private void openFont()
    {
        System.Windows.Forms.FontDialog openFileDialog = new();
        openFileDialog.FontMustExist = true;

        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            System.Drawing.Font? font = openFileDialog.Font;
            textBox.Text = font.Name;
            if (font.Style != System.Drawing.FontStyle.Regular)
            {
                textBox.Text += " " + font.Style;
            }
        }
    }

    private void textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        openFont();
    }

    private void btnOpen_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        openFont();
    }
}
