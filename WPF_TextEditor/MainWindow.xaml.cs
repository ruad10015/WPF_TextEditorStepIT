using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace WPF_TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtFileName.Text = $"Select Text File ==>";
        }

        private bool isAutoSaveEnabled = false;

        

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = FileManage.Path;
            openFileDialog.Filter = "TXT files|*.txt";
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                FileManage.Path = openFileDialog.FileName;
                txtFileName.Text = openFileDialog.SafeFileName;
                txtContent.IsEnabled = true;
                txtContent.Text = FileManage.ReadFile();
            }
        }

        private void btnAutoSave_Checked(object sender, RoutedEventArgs e)
        {
            isAutoSaveEnabled = true;
            txtContent.Focus();
            MessageBox.Show("Checked","Checked");
            txtContent.Focus();
        }

        private void btnAutoSave_Unchecked(object sender, RoutedEventArgs e)
        {
            isAutoSaveEnabled = false;
            MessageBox.Show("Unchecked","Unchecked");
            txtContent.Focus();
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            FileManage.WriteFile(txtContent.Text);
            MessageBox.Show("Have been saved","Save");
            txtContent.Focus();
        }

        private void btnCut_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation.isAnyTextSelected(txtContent)) return;

            var textBox = txtContent as TextBox;
            if (textBox.SelectedText != null)
            {
                int selectionStart = textBox.SelectionStart;
                int selectionLength = textBox.SelectionLength;
                string currentText = textBox.Text;

                Clipboard.SetText(currentText.Substring(selectionStart, selectionLength));

                textBox.Text = currentText.Remove(textBox.SelectionStart, textBox.SelectionLength);
                textBox.CaretIndex = selectionStart;
            }
            txtContent.Focus();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation.isAnyTextSelected(txtContent)) return;

            var textBox = txtContent as TextBox;
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            if (textBox.SelectedText != null) Clipboard.SetText(textBox.Text.Substring(selectionStart, selectionLength));

            textBox.CaretIndex = selectionStart;
            txtContent.Focus();
        }

        private void btnPaste_Click(object sender, RoutedEventArgs e)
        {
            var textBox = txtContent as TextBox;
            int caretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Insert(caretIndex, Clipboard.GetText());
            textBox.CaretIndex = caretIndex += Clipboard.GetText().Length;
            txtContent.Focus();
        }

        private void btnSelectAll_Click(object sender, RoutedEventArgs e)
        {
            txtContent.SelectAll();
            txtContent.Focus();
        }
        private void txtContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isAutoSaveEnabled) FileManage.WriteFile(txtContent.Text);
        }

    }
}