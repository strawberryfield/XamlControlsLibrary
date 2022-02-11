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

using System.Windows.Controls;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for PangoTextEditor.xaml
/// </summary>
public partial class PangoTextEditor : UserControl
{
    public PangoTextEditor()
    {
        InitializeComponent();
    }

    private void btnOpen_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.OpenFile();

    private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e) => textBox.SaveFile();
}
