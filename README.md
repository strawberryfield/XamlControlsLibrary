# Casasoft XAML Controls Library

[![NuGet](https://img.shields.io/nuget/v/Casasoft.Xaml.Controls)](https://www.nuget.org/packages/Casasoft.Xaml.Controls)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](https://www.gnu.org/licenses/agpl-3.0)
[![.NET](https://img.shields.io/badge/.NET-8.0%20%7C%2010.0-purple)](https://dotnet.microsoft.com)

A collection of ready-to-use WPF UserControls that fill the gaps left by the standard toolkit — file pickers, numeric inputs, image viewers and text editors, all wired up and ready to drop into your project.

**Repository:** [github.com/strawberryfield/XamlControlsLibrary](https://github.com/strawberryfield/XamlControlsLibrary)  
**Author:** Roberto Ceccarelli — [The Strawberry Field](http://strawberryfield.altervista.org)

---

## Installation

```
dotnet add package Casasoft.Xaml.Controls
```

Or search for **Casasoft.Xaml.Controls** in the NuGet Package Manager inside Visual Studio.

Targets **net8.0-windows** and **net10.0-windows**.

---

## Controls at a glance

| Control | Description |
|---|---|
| `FileTextBox` | TextBox + browse button for file paths |
| `FileTextBoxLabel` | `FileTextBox` with an attached caption label |
| `NumericUpDown` | Integer spinner with configurable range and step |
| `ImageViewer` | Pan / zoom / rotate / flip image viewer |
| `TextEditor` | Multi-line editor with open/save, drag-and-drop and markup helpers |
| `PangoTextEditor` | `TextEditor` pre-configured for Pango markup editing |

---

## Controls in detail

### FileTextBox

A `TextBox` pre-wired for file path entry. Users can type a path directly, click the browse button, double-click the text box, or simply **drag and drop** a file from Explorer. A full context menu covers cut, copy, paste, undo, redo, select-all, clear and open-file.

```xml
xmlns:cs="clr-namespace:Casasoft.Xaml.Controls;assembly=Casasoft.Xaml.Controls"
...
<cs:FileTextBox x:Name="myFilePicker"
                OpenFileDialogFilter="Images (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*"
                OpenFileDialogTitle="Choose an image" />
```

```csharp
string path = myFilePicker.Value;   // read the chosen path
myFilePicker.Value = @"C:\default"; // set a default path
```

**Key properties**

| Property | Type | Description |
|---|---|---|
| `Value` | `string` | The current file path |
| `OpenFileDialogFilter` | `string` | Standard Win32 filter string |
| `OpenFileDialogTitle` | `string` | Dialog window title |

---

### FileTextBoxLabel

A thin composite control that stacks a `Label` above a `FileTextBox`. Handy when you need a labelled file picker without writing extra XAML every time.

```xml
<cs:FileTextBoxLabel Caption="Input file"
                     OpenFileDialogFilter="Text files (*.txt)|*.txt"
                     OpenFileDialogTitle="Select a text file" />
```

**Key properties** — all of `FileTextBox`, plus:

| Property | Type | Description |
|---|---|---|
| `Caption` | `string?` | Text shown in the label above the picker |

---

### NumericUpDown

A compact integer spinner with Up/Down buttons, keyboard input validation (digits and leading minus only) and configurable range clamping.

```xml
<cs:NumericUpDown x:Name="spinner"
                  MinValue="0" MaxValue="100" Step="5" />
```

```csharp
spinner.Value = 42;
int current = spinner.Value;
```

**Key properties**

| Property | Type | Default | Description |
|---|---|---|---|
| `Value` | `int` | `0` | Current value (clamped to range) |
| `MinValue` | `int` | `int.MinValue` | Lower bound |
| `MaxValue` | `int` | `int.MaxValue` | Upper bound |
| `Step` | `int` | `1` | Increment/decrement amount |

---

### ImageViewer

A self-contained image viewer with mouse-driven **pan**, **scroll-wheel zoom**, and a context menu for **flip** (horizontal/vertical) and **rotation** (90°, 180°, 270°, free). All transformations are composited through a single `TransformGroup` for predictable rendering.

```xml
<cs:ImageViewer x:Name="viewer" BorderBackground="Black" />
```

```csharp
viewer.Source = new BitmapImage(new Uri("photo.jpg", UriKind.Relative));

// Programmatic control
viewer.Rotate(90);
viewer.FlipHorizontal();
viewer.Zoom(0.5);
viewer.Reset();
```

**Key members**

| Member | Description |
|---|---|
| `Source` | Gets/sets the `ImageSource` to display |
| `Image` | Exposes the inner `Image` element for advanced customisation |
| `BorderBackground` | Background brush shown behind the image |
| `FlipHorizontal()` | Toggles horizontal mirror |
| `FlipVertical()` | Toggles vertical mirror |
| `Rotate(int angle)` | Adds `angle` degrees to the current rotation |
| `Zoom(double delta)` | Adjusts zoom (min 0.2 enforced) |
| `Reset()` | Restores default transform state |

---

### TextEditor

A multi-line `TextBox` wrapper with a complete editing experience out of the box:

- **Open / Save** dialogs (configurable filter, title, default extension)  
- **Drag and drop** — drop a file to load it, or drop plain text to insert it  
- **Context menu** with cut, copy, paste, undo, redo, select-all, select-line, clear, load, save  
- **Public API** for all editing actions (suitable for toolbar buttons)  
- **Markup helpers** — wrap the current selection with arbitrary markup or paired XML tags

```xml
<cs:TextEditor x:Name="editor"
               OpenFileDialogFilter="Markdown (*.md)|*.md|All files (*.*)|*.*"
               SaveFileDialogFilter="Markdown (*.md)|*.md|All files (*.*)|*.*"
               SaveFileDefaultExt="md" />
```

```csharp
editor.Value = File.ReadAllText("draft.md");
editor.AddTagToSelection("b");           // wraps selection with <b>...</b>
editor.AddMarkupToSelection("**", "**"); // wraps selection with **...**
editor.SaveFile();
```

**Key members**

| Member | Description |
|---|---|
| `Value` | Full text content |
| `SelectedText` | Currently selected text (read-only) |
| `OpenFile()` / `SaveFile()` | Show the respective dialog and read/write |
| `AddTagToSelection(tag)` | Wraps selection with `<tag>…</tag>` |
| `AddMarkupToSelection(before, after)` | Generic markup wrapper |
| `Undo()` / `Redo()` | Routed undo/redo |

---

### PangoTextEditor

A `TextEditor` pre-configured for [Pango markup](https://docs.gtk.org/Pango/pango_markup.html), with a built-in toolbar providing Open, Save, New, Undo, Redo, **Bold** and *Italic* buttons. The file dialog defaults to `.pango` files.

```xml
<cs:PangoTextEditor x:Name="pangoEditor" />
```

```csharp
pangoEditor.Value = "pango:<span>Hello <b>world</b></span>";
string markup = pangoEditor.Value;
```

---

## Adding the namespace in XAML

```xml
xmlns:cs="clr-namespace:Casasoft.Xaml.Controls;assembly=Casasoft.Xaml.Controls"
```

---

## License

Casasoft XAML Controls Library is released under the **GNU Affero General Public License v3.0**.  
See the [LICENSE](LICENSE) file for full details.

---

*copyright © 2021-2026 Roberto Ceccarelli — Casasoft*
