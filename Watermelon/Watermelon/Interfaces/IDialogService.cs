
namespace Watermelon.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string message);

        bool ShowMessageYesNo(string message, string caption);

        string ShowOpenDialog(string filter, bool multiSelect);

        string[] ShowOpenDialog(string filter);

        string ShowSaveDialog(string filter);

        string ShowFolderDialog();
    }
}
