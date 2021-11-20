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
