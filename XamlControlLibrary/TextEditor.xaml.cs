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

using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Casasoft.Xaml.Controls;

/// <summary>
/// Interaction logic for TextEditor.xaml
/// </summary>
public partial class TextEditor : UserControl
{
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
    public string Value
    {
        get => textBox.Text;
        set => textBox.Text = value;
    }

    public string SelectedText => textBox.SelectedText;

    public string OpenFileDialogFilter { get; set; }
    public string OpenFileDialogTitle { get; set; }
    public string SaveFileDialogFilter { get; set; }
    public string SaveFileDialogTitle { get; set; }
    public string SaveFileDefaultExt { get; set; }
    #endregion

    #region open file
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

    private void textBox_PreviewDragOver(object sender, DragEventArgs e) => e.Handled = true;
    #endregion

    #region context menu
    private void ClickPaste(object sender, RoutedEventArgs args) => textBox.Paste();
    private void ClickCopy(object sender, RoutedEventArgs args) => textBox.Copy();
    private void ClickCut(object sender, RoutedEventArgs args) => textBox.Cut();
    private void ClickSelectAll(object sender, RoutedEventArgs args) => textBox.SelectAll();
    private void ClickClear(object sender, RoutedEventArgs args) => textBox.Clear();
    private void ClickUndo(object sender, RoutedEventArgs args) => textBox.Undo();
    private void ClickRedo(object sender, RoutedEventArgs args) => textBox.Redo();

    private void ClickSelectLine(object sender, RoutedEventArgs args)
    {
        int lineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
        int lineStartingCharIndex = textBox.GetCharacterIndexFromLineIndex(lineIndex);
        int lineLength = textBox.GetLineLength(lineIndex);
        textBox.Select(lineStartingCharIndex, lineLength);
    }

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

    private void ClickSelectFile(object sender, RoutedEventArgs args) => OpenFile();
    private void cxmItemSaveFile_Click(object sender, RoutedEventArgs e) => SaveFile();
    #endregion

    #region routed methods
    public void Paste() => textBox.Paste();
    public void Copy() => textBox.Copy();
    public void Cut() => textBox.Cut();
    public void SelectAll() => textBox.SelectAll();
    public void Clear() => textBox.Clear();
    public void Undo() => textBox.Undo();
    public void Redo() => textBox.Redo();
    #endregion

    #region markup
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

    public void AddTagToSelection(string tag) => AddMarkupToSelection($"<{tag}>", $"</{tag}>");
    #endregion
}
