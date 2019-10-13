using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources;

namespace MTGApro
{
    /// <summary>
    /// Логика взаимодействия для CardRenderWindow.xaml
    /// </summary>
    public partial class CardRenderWindow : Window
    {
        public CardRenderWindow()
        {
            InitializeComponent();  
            StreamResourceInfo sriCurs = Application.GetResourceStream(
            new Uri("pack://application:,,,/Resources/testcur.cur"));
            Cursor = new Cursor(sriCurs.Stream);
        }
    }
}
