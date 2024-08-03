using IFC_Table_View.Infracrucrure.Commands.Base;
using System.Windows;

namespace IFC_Table_View.Infracrucrure.Commands
{
    internal class CloseApplicationCommand : BaseCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}