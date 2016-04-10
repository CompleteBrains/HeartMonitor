using System;
using System.Windows;

namespace Views
{
    public interface IDialog
    {
        void Show(Exception exception);
    }

    public class Dialog : IDialog
    {
        public void Show(Exception exception)
        {
            MessageBox.Show(exception.Message, exception.TargetSite.ReflectedType?.FullName, MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}