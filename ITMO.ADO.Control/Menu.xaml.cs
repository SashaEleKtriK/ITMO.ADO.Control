using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ITMO.ADO.Control
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        string connectToOther;
        public int person;
        InventaryWindow NewInv;
        adminWin admin;
        public Menu(string connectionString)
        {
            
            connectToOther = connectionString;
            InitializeComponent();
            
            

        }
        private void win_closed(object sender, EventArgs e)
        {
            this.IsEnabled = true;
            this.Activate();
        }

        private void keys_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            string text = btn.Content.ToString();
            int tag = 0;
            this.IsEnabled = false;
            if (text.Length>0)
            {
                tag = Int32.Parse(btn.Tag.ToString());
            }
            
            try
            {
                if (text == "Добавить в БД" && Post.Content.ToString() == "admin")
                {
                    admin = new adminWin(connectToOther);
                    admin.Owner = this;
                    admin.FirstName.Content = FirstName.Content;
                    admin.LastName.Content = LastName.Content;
                    admin.FatherName.Content = FatherName.Content;
                    admin.Post.Content = Post.Content;
                    admin.Post.Tag = person.ToString();
                    admin.Closed += new EventHandler(win_closed);
                    admin.Show();
                }
                if (text == "Добавить в БД" && Post.Content.ToString() != "admin")
                { MessageBox.Show("Необходимо обладать правами администратора"); }
                if (tag > 0)
                {
                    NewInv = new InventaryWindow(tag, person, connectToOther);
                    NewInv.Owner = this;
                    NewInv.FirstName.Content = FirstName.Content;
                    NewInv.LastName.Content = LastName.Content;
                    NewInv.FatherName.Content = FatherName.Content;
                    NewInv.Post.Content = Post.Content;
                    NewInv.Post.Tag = person.ToString();
                    NewInv.Closed += new EventHandler(win_closed); 
                    NewInv.Show();


                }
                
            }
            catch(Exception werr)
            {
                MessageBox.Show(werr.Message);
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
