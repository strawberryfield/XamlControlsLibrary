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

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for FileTextBox.xaml
/// A UserControl that wraps a `TextBox` to represent a file path and provides
/// common file-selection and editing features:
/// - Open file dialog (button click or text box double-click)
/// - Drag &amp; drop support for file paths
/// - Context menu with clipboard and edit commands (paste, copy, cut, select all, clear, undo, redo)
/// </summary>
public partial class FileTextBox : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileTextBox"/> control.
    /// Sets default dialog filter and title.
    /// </summary>
    public FileTextBox()
    {
        InitializeComponent();
        OpenFileDialogFilter = "All files (*.*)|*.*";
        OpenFileDialogTitle = "File";
    }

    #region properties

    /// <summary>
    /// Gets or sets the current text shown in the inner `TextBox`.
    /// This is typically used to store a file path.
    /// </summary>
    public string Value
    {
        get => textBox.Text;
        set => textBox.Text = value;
    }

    /// <summary>
    /// Gets or sets the filter string used by the open file dialog (e.g. "Text files (*.txt)|*.txt").
    /// Default: "All files (*.*)|*.*".
    /// </summary>
    public string OpenFileDialogFilter { get; set; }

    /// <summary>
    /// Gets or sets the title shown in the open file dialog.
    /// Default: "File".
    /// </summary>
    public string OpenFileDialogTitle { get; set; }
    #endregion

    #region open file

    /// <summary>
    /// Opens an <see cref="OpenFileDialog"/>, optionally starting in the directory of the current text,
    /// and sets the `TextBox` text to the selected file path when the user confirms.
    /// </summary>
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

    /// <summary>
    /// Handles the open-button click and shows the file dialog.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Routed event args.</param>
    private void btnOpen_Click(object sender, RoutedEventArgs e) => openFile();

    /// <summary>
    /// Opens the file dialog when the text box is double-clicked.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Mouse button event args.</param>
    private void textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) => openFile();
    #endregion

    #region dragdrop

    /// <summary>
    /// Handles a file being dropped onto the inner `TextBox`. If one or more files are dropped,
    /// the first file path is placed into the text box.
    /// </summary>
    /// <param name="sender">The `TextBox` receiving the drop.</param>
    /// <param name="e">Drag event args containing the dropped data.</param>
    private void textBox_Drop(object sender, DragEventArgs e)
    {
        TextBox tb = (TextBox)sender;
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length != 0)
        {
            tb.Text = files[0];
        }
    }

    /// <summary>
    /// Prevents default drag-over handling so the control can accept drops.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Drag event args.</param>
    private void textBox_PreviewDragOver(object sender, DragEventArgs e) => e.Handled = true;
    #endregion

    #region context menu

    /// <summary>Paste clipboard text into the `TextBox`.</summary>
    private void ClickPaste(object sender, RoutedEventArgs args) => textBox.Paste();
    /// <summary>Copy selected text from the `TextBox` to the clipboard.</summary>
    private void ClickCopy(object sender, RoutedEventArgs args) => textBox.Copy();
    /// <summary>Cut selected text from the `TextBox` to the clipboard.</summary>
    private void ClickCut(object sender, RoutedEventArgs args) => textBox.Cut();
    /// <summary>Select all text in the `TextBox`.</summary>
    private void ClickSelectAll(object sender, RoutedEventArgs args) => textBox.SelectAll();
    /// <summary>Clear the `TextBox` content.</summary>
    private void ClickClear(object sender, RoutedEventArgs args) => textBox.Clear();
    /// <summary>Undo the last edit in the `TextBox`.</summary>
    private void ClickUndo(object sender, RoutedEventArgs args) => textBox.Undo();
    /// <summary>Redo the last undone edit in the `TextBox`.</summary>
    private void ClickRedo(object sender, RoutedEventArgs args) => textBox.Redo();

    /// <summary>
    /// Updates the enabled state of context-menu items when the menu opens:
    /// - Enables copy/cut only when there is a selection.
    /// - Enables paste only when the clipboard contains text.
    /// </summary>
    /// <param name="sender">Context menu or menu item sender.</param>
    /// <param name="args">Routed event args.</param>
    private void CxmOpened(object sender, RoutedEventArgs args)
    {
        // Only allow copy/cut if something is selected to copy/cut.
        if (textBox.SelectedText == "")
            cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = false;
        else
            cxmItemCopy.IsEnabled = cxmItemCut.IsEnabled = true;

        // Only allow paste if there is text on the clipboard to paste.
        if (Clipboard.ContainsText())
            cxmItemPaste.IsEnabled = true;
        else
            cxmItemPaste.IsEnabled = false;
    }

    /// <summary>
    /// Menu command to open the file dialog and select a file.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="args">Routed event args.</param>
    private void ClickSelectFile(object sender, RoutedEventArgs args) => openFile();

    #endregion
}
