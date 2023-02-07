using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITMO.ADO.Control
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection connection = new OleDbConnection();
        string teststring; 
        Menu men;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void close_win(object sender, EventArgs e)
        {
            nameBOX.Text = "";
            password.Text = "";
            but.IsEnabled = true;
            this.Activate();


        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
            try
            {
                string name = nameBOX.Text;
                string pas = password.Text;
                if (isLocal.IsChecked== true)
                {
                    teststring = $@"Provider=MSOLEDBSQL.1; Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dtSource.Text}; User Id = {name}; Password = {pas}";
                }
                else
                {
                    teststring = $@"Provider=SQLOLEDB.1;Persist Security Info=False;Initial Catalog=LogCable;User Id = {name}; Password = {pas}; Data Source={dtSource.Text}";
                }
               
                connection.ConnectionString = teststring;
                connection.Open();
                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM PERSONS WHERE NAME = '" + name + "'";

                OleDbDataReader reader = command.ExecuteReader();
                int i = 0;
                string fnameDb = "";
                string lnameDb = "";
                string fatherName = "";
                string postDb = "";
                int pers_id = 0;
                while (reader.Read())
                {
                    
                    i++;
                    fnameDb = reader["first_name"].ToString();
                    lnameDb = reader["last_name"].ToString();
                    postDb = reader["post"].ToString();
                    fatherName = reader["father_name"].ToString();
                    pers_id = Int32.Parse(reader["person_id"].ToString());
                    
                }
                reader.Close();
                if (i == 1)
                {
                    
                    command.CommandText = "SELECT POST FROM POSTS WHERE POST_ID = " + postDb;
                    OleDbDataReader postread = command.ExecuteReader();
                    string postPers = "";
                    while (postread.Read())

                    {

                        postPers = postread["post"].ToString();

                    }
                    postread.Close();
                    men = new Menu(teststring);
                    men.Closed += new EventHandler(this.close_win);
                    men.Owner = this;
                    men.person = pers_id;
                    men.FirstName.Content = fnameDb;
                    men.FatherName.Content = fatherName;
                    men.LastName.Content = lnameDb;
                    men.Post.Content = postPers;
                    men.Show();
                    but.IsEnabled= false;
                    
                }
                else 
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }
                

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            { connection.Close(); }
           
        }

        private void dtSource_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            label.Content = "AttachDbFilename";
            dtSource.Text = "C:\\Users\\HP\\source\\repos\\ITMO.ADO.Control\\ITMO.ADO.Control\\LogCable.mdf";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            label.Content = "Data Source";
            dtSource.Text = "DESKTOP-LE67O4M\\SQLEXPRESS";
        }
    }
}
