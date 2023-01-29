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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        string post = "";
        OleDbConnection connection = new OleDbConnection();
        public Window1(string postId, string connectionString)
        {
            post = postId;
            connection.ConnectionString = connectionString;
            InitializeComponent();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            string text = btn.Content.ToString();
            if (text == "Отмена")
                this.Close();
            else
            {
                try
                {
                    connection.Open();
                    OleDbTransaction OleTran = connection.BeginTransaction();
                    OleDbCommand command = connection.CreateCommand();

                    try
                    {
                        if (FirstName.Text.Length > 0 && FatherName.Text.Length > 0 && LastName.Text.Length > 0 && LogName.Text.Length > 0 && Password.Text.Length > 0)
                        {
                            

                            command.Transaction = OleTran;
                            command.CommandText = "INSERT persons (post, first_name, last_name, father_name, name, password) VALUES ('" + post + "', '" + FirstName.Text + "', '" + LastName.Text + "', '" + FatherName.Text + "', '" + LogName.Text + "', '" + Password.Text + "') ";
                            command.ExecuteNonQuery();
                            command.CommandText = $"CREATE LOGIN [{LogName.Text}] WITH PASSWORD='{Password.Text}', DEFAULT_DATABASE=[LogCable], DEFAULT_LANGUAGE=[русский], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF";
                            command.ExecuteNonQuery();
                            command.CommandText = $"ALTER LOGIN [{LogName.Text}] ENABLE";
                            command.ExecuteNonQuery();
                            if (post == "1")
                            {
                                command.CommandText = $"ALTER SERVER ROLE [sysadmin] ADD MEMBER [{LogName.Text}]";
                                command.ExecuteNonQuery();
                            }
                            command.CommandText = $"USE LogCable";
                            command.ExecuteNonQuery();
                            command.CommandText = $"CREATE USER [{LogName.Text}] FOR LOGIN [{LogName.Text}] WITH DEFAULT_SCHEMA=[dbo] ";
                            command.ExecuteNonQuery();
                            command.CommandText = $"ALTER ROLE [db_datareader] ADD MEMBER [{LogName.Text}]";
                            command.ExecuteNonQuery();
                            command.CommandText = $"ALTER ROLE [db_datawriter] ADD MEMBER [{LogName.Text}]";
                            command.ExecuteNonQuery();

                            OleTran.Commit();

                        }
                        else
                        {
                            MessageBox.Show("Проверьте правильность введенных данных");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        try

                        {

                            OleTran.Rollback();

                        }

                        catch (Exception exRollback)

                        {

                            MessageBox.Show(exRollback.Message);

                        }
                    }
                    finally
                    {
                        connection.Close();
                        this.Close();
                    }


                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
                
               
            }
        }
    }
}
