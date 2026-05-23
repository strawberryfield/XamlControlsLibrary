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

namespace Casasoft.Xaml.Controls;

/// <summary>
/// A lightweight text editor control that wraps a <see cref="TextBox"/> and
/// provides common editing features such as open/save dialogs, drag/drop
/// support, clipboard and undo/redo operations, and simple markup helpers.
/// </summary>
public partial class TextEditor : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextEditor"/> control.
    /// Default values for open/save dialog filters, titles and default extension
    /// are configured in the constructor.
    /// </summary>
    public TextEditor()
    {
        InitializeComponent();
        OpenFileDialogFilter = "All files (*.*)|*.*";
        OpenFileDialogTitle = "Load from file";
        SaveFileDialogFilter = "All files (*.*)|*.*";
        SaveFileDialogTitle = "Save to file";
        SaveFileDefaultExt = "txt";
    }

    #region properties

    /// <summary>
    /// Gets or sets the entire text content of the editor.
    /// This delegates to the inner <see cref="TextBox"/> instance.
    /// </summary>
    public string Value
    {
        get => textBox.Text;
        set => textBox.Text = value;
    }

    /// <summary>
    /// Gets the currently selected text in the editor.
    /// </summary>
    public string SelectedText => textBox.SelectedText;

    /// <summary>
    /// Filter string used by the <see cref="OpenFile"/> dialog (e.g. "All files (*.*)|*.*").
    /// </summary>
    public string OpenFileDialogFilter { get; set; }

    /// <summary>
    /// Title displayed by the <see cref="OpenFile"/> dialog.
    /// </summary>
    public string OpenFileDialogTitle { get; set; }

    /// <summary>
    /// Filter string used by the <see cref="SaveFile"/> dialog.
    /// </summary>
    public string SaveFileDialogFilter { get; set; }

    /// <summary>
    /// Title displayed by the <see cref="SaveFile"/> dialog.
    /// </summary>
    public string SaveFileDialogTitle { get; set; }

    /// <summary>
    /// Default extension applied by the <see cref="SaveFile"/> dialog (without the leading dot).
    /// </summary>
    public string SaveFileDefaultExt { get; set; }
    #endregion

    #region open file

    /// <summary>
    /// Opens a file selection dialog and loads the selected file's text into the editor.
    /// The dialog respects <see cref="OpenFileDialogFilter"/> and <see cref="OpenFileDialogTitle"/>.
    /// If no file is selected the method returns without modifying the content.
    /// </summary>
    public void OpenFile()
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = OpenFileDialogFilter;
        openFileDialog.Title = OpenFileDialogTitle;
        openFileDialog.Multiselect = false;
        if (openFileDialog.ShowDialog() == true)
        {
            textBox.Text = File.ReadAllText(openFileDialog.FileName);
        }
    }
    #endregion

    #region save file

    /// <summary>
    /// Opens a save file dialog and writes the editor content to the selected file.
    /// The dialog respects <see cref="SaveFileDialogFilter"/>, <see cref="SaveFileDialogTitle"/>
    /// and <see cref="SaveFileDefaultExt"/>.
    /// If the user cancels the dialog no file is written.
    /// </summary>
    public void SaveFile()
    {
        SaveFileDialog sd = new();
        sd.Filter = SaveFileDialogFilter;
        sd.Title = SaveFileDialogTitle;
        sd.DefaultExt = SaveFileDefaultExt;
        sd.AddExtension = true;
        sd.OverwritePrompt = true;
        sd.ShowDialog();
        if (!string.IsNullOrWhiteSpace(sd.FileName))
        {
            File.WriteAllText(sd.FileName, textBox.Text);
        }
    }
    #endregion

    #region dragdrop

    /// <summary>
    /// Handles files or text dragged and dropped onto the inner <see cref="TextBox"/>.
    /// If files are dropped the first file is loaded; otherwise plain text payload is used.
    /// </summary>
    private void textBox_Drop(object sender, DragEventArgs e)
    {
        TextBox tb = (TextBox)sender;
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        if (files != null && files.Length != 0)
        {
            tb.Text = File.ReadAllText(files[0]);
        }
        else
        {
            string txt = (string)e.Data.GetData(DataFormats.Text);
            if (string.IsNullOrWhiteSpace(txt))
            {
                tb.Text = txt;
            }
        }
    }

    /// <summary>
    /// Prevents default drag over handling so the control can accept drops.
    /// </summary>
    private void textBox_PreviewDragOver(object sender, DragEventArgs e) => e.Handled = true;
    #endregion

    #region context menu

    /// <summary>
    /// Paste command invoked from context menu.
    /// </summary>
    private void ClickPaste(object sender, RoutedEventArgs args) => textBox.Paste();

    /// <summary>
    /// Copy command invoked from context menu.
    /// </summary>
    private void ClickCopy(object sender, RoutedEventArgs args) => textBox.Copy();

    /// <summary>
    /// Cut command invoked from context menu.
    /// </summary>
    private void ClickCut(object sender, RoutedEventArgs args) => textBox.Cut();

    /// <summary>
    /// Selects all text in the editor (context menu).
    /// </summary>
    private void ClickSelectAll(object sender, RoutedEventArgs args) => textBox.SelectAll();

    /// <summary>
    /// Clears the editor content (context menu).
    /// </summary>
    private void ClickClear(object sender, RoutedEventArgs args) => textBox.Clear();

    /// <summary>
    /// Undo last change (context menu).
    /// </summary>
    private void ClickUndo(object sender, RoutedEventArgs args) => textBox.Undo();

    /// <summary>
    /// Redo last undone change (context menu).
    /// </summary>
    private void ClickRedo(object sender, RoutedEventArgs args) => textBox.Redo();

    /// <summary>
    /// Selects the entire current line where the caret is located.
    /// </summary>
    private void ClickSelectLine(object sender, RoutedEventArgs args)
    {
        int lineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
        int lineStartingCharIndex = textBox.GetCharacterIndexFromLineIndex(lineIndex);
        int lineLength = textBox.GetLineLength(lineIndex);
        textBox.Select(lineStartingCharIndex, lineLength);
    }

    /// <summary>
    /// Context menu opened handler that enables/disables items depending on state:
    /// - copy/cut is only enabled if there is a non-empty selection
    /// - paste is only enabled if the clipboard contains text
    /// </summary>
    private void CxmOpened(object sender, RoutedEventArgs args)
    {
        // Only allow copy/cut if something is selected to copy/cut.
        if (string.IsNullOrWhiteSpace(textBox.SelectedText))
        {
            cxmItemCopy.IsEnabled = false;
            cxmItemCut.IsEnabled = false;
        }
        else
        {
            cxmItemCopy.IsEnabled = false;
            cxmItemCut.IsEnabled = true;
        }

        // Only allow paste if there is text on the clipboard to paste.
        if (Clipboard.ContainsText())
        {
            cxmItemPaste.IsEnabled = true;
        }
        else
        {
            cxmItemPaste.IsEnabled = false;
        }
    }

    /// <summary>
    /// Opens the file selection dialog (context menu action).
    /// </summary>
    private void ClickSelectFile(object sender, RoutedEventArgs args) => OpenFile();

    /// <summary>
    /// Saves the current content to a file (context menu action).
    /// </summary>
    private void cxmItemSaveFile_Click(object sender, RoutedEventArgs e) => SaveFile();
    #endregion

    #region routed methods

    /// <summary>
    /// Paste the clipboard text into the editor.
    /// </summary>
    public void Paste() => textBox.Paste();

    /// <summary>
    /// Copy the current selection to the clipboard.
    /// </summary>
    public void Copy() => textBox.Copy();

    /// <summary>
    /// Cut the current selection to the clipboard.
    /// </summary>
    public void Cut() => textBox.Cut();

    /// <summary>
    /// Select all text in the editor.
    /// </summary>
    public void SelectAll() => textBox.SelectAll();

    /// <summary>
    /// Clear the editor's text content.
    /// </summary>
    public void Clear() => textBox.Clear();

    /// <summary>
    /// Undo the most recent edit operation.
    /// </summary>
    public void Undo() => textBox.Undo();

    /// <summary>
    /// Redo the most recent undone edit operation.
    /// </summary>
    public void Redo() => textBox.Redo();
    #endregion

    #region markup

    /// <summary>
    /// Wraps the current selection with the provided start and end strings.
    /// After replacement the selection is adjusted to contain the inserted markup.
    /// If there is no selection, the method does nothing.
    /// </summary>
    /// <param name="beforeStart">Text to insert before the selected text (e.g. an opening tag).</param>
    /// <param name="afterEnd">Text to insert after the selected text (e.g. a closing tag).</param>
    public void AddMarkupToSelection(string beforeStart, string afterEnd)
    {
        if (!string.IsNullOrEmpty(textBox.SelectedText))
        {
            int position = textBox.SelectionStart;
            string? replacedText = $"{beforeStart}{textBox.SelectedText}{afterEnd}";
            textBox.SelectedText = replacedText;
            textBox.Select(position, replacedText.Length);
            textBox.Focus();
        }
    }

    /// <summary>
    /// Helper that adds an XML/HTML-like tag wrapper around the current selection.
    /// Example: calling <c>AddTagToSelection("b")</c> will replace a selection with &lt;b&gt;...&lt;/b&gt;.
    /// </summary>
    /// <param name="tag">The tag name to use (without angle brackets).</param>
    public void AddTagToSelection(string tag) => AddMarkupToSelection($"<{tag}>", $"</{tag}>");
    #endregion
}
