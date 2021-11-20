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

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for FileTextBox.xaml
/// </summary>
public partial class FileTextBox : UserControl
{
    public FileTextBox()
    {
        InitializeComponent();
        OpenFileDialogFilter = "All files (*.*)|*.*";
        OpenFileDialogTitle = "File";
    }

    #region properties
    public string Value
    {
        get => textBox.Text;
        set => textBox.Text = value;
    }

    public string OpenFileDialogFilter { get; set; } 
    public string OpenFileDialogTitle { get; set; }
    #endregion

    #region open file
    private void openFile()
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = OpenFileDialogFilter;
        openFileDialog.Title = OpenFileDialogTitle;
        openFileDialog.Multiselect = false;
        if(!string.IsNullOrEmpty(textBox.Text))
        {
            openFileDialog.InitialDirectory = Path.GetDirectoryName(textBox.Text);
        }

        if (openFileDialog.ShowDialog() == true)
        {
            textBox.Text = openFileDialog.FileName;
        }
    }

    private void btnOpen_Click(object sender, RoutedEventArgs e)
    {
        openFile();
    }

    private void textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        openFile();
    }
    #endregion

    #region dragdrop
    private void textBox_Drop(object sender, DragEventArgs e)
    {
        TextBox tb = (TextBox)sender;
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length != 0)
        {
            tb.Text = files[0];
        }
    }

    private void textBox_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Handled = true;
    }
    #endregion
}
