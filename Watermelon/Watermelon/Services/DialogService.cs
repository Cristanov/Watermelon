
using System.Windows;
using System.Windows.Forms;
using Watermelon.Interfaces;
namespace Watermelon.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public bool ShowMessageYesNo(string message, string caption)
        {
            if ((System.Windows.MessageBox.Show(message, caption, MessageBoxButton.YesNo) == MessageBoxResult.Yes))
            {
                return true;
            }
            else return false;
        }

        public string ShowOpenDialog(string filter, bool multiSelect)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.Multiselect = multiSelect;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public string ShowSaveDialog(string filter)
        {
            SaveFileDialog saveFIleDialog = new SaveFileDialog();
            saveFIleDialog.Filter = filter;
            if (saveFIleDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFIleDialog.FileName;
            }
            else
                return string.Empty;
        }

        public string ShowFolderDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            else
            {
                return string.Empty;
            }
        }


        public string[] ShowOpenDialog(string filter)
        {
            OpenFileDialog openFIleDialog = new OpenFileDialog();
            openFIleDialog.Filter = filter;
            openFIleDialog.Multiselect = true;
            if (openFIleDialog.ShowDialog() == DialogResult.OK)
            {
                return openFIleDialog.FileNames;
            }
            return new string[]{};
        }
    }
}
