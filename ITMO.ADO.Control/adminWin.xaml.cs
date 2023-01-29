using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITMO.ADO.Control
{
    /// <summary>
    /// Логика взаимодействия для adminWin.xaml
    /// </summary>
    public partial class adminWin : Window
    {
        Window1 winpers;
        string connectToOther;
        public OleDbConnection connection = new OleDbConnection();
        public adminWin(string connectionString)
        {
            InitializeComponent();
            invNumber.IsEnabled= false;
            connectToOther = connectionString;
            connection.ConnectionString = connectToOther;
            dataload();
        }
        private string currentType()
        {
            StackPanel selected = new StackPanel();
            selected = inventaryList.SelectedItem as StackPanel;
            Label tag = selected.Children[1] as Label;
            string typefilter = tag.Tag.ToString();
            return typefilter;
        }
        private string currentPost(int i)
        {
            StackPanel selected = new StackPanel();
            selected = persBox.SelectedItem as StackPanel;
            if (i == 0)
            {
                Label tag = selected.Children[0] as Label;
                string postfilter = tag.Content.ToString();
                return postfilter;
            }
            else
            {
                Label tag = selected.Children[1] as Label;
                string postfilter = tag.Tag.ToString();
                return postfilter;
            }
            
        }
        private void inv_log_fill()
        {
            try
            {
                logBox.Items.Clear();
                OleDbCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM inventary WHERE type_id =" + currentType();
                OleDbDataReader reader = command.ExecuteReader();
                string invNum;
                string status;
                while (reader.Read())
                {

                    StackPanel item;
                    item = new StackPanel();
                    item.Orientation = Orientation.Horizontal;
                    invNum = reader["number"].ToString();
                    string typeId = reader["type_id"].ToString();
                    string bitStatus = reader["status"].ToString();
                    if (bitStatus == "False")
                    {
                        status = "Выписано ";
                    }
                    else
                    {
                        status = "Списано ";
                    }
                    Label Num = new Label();
                    Num.Content = invNum;
                    Label stat = new Label();
                    stat.Content = status;
                    item.Children.Add(Num);
                    item.Children.Add(stat);
                    if (typeId == currentType())
                    {
                        logBox.Items.Add(item);
                    }
                    

                }
                reader.Close();





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    
        
        private void dataload()
        {
            try
            {
               
                connection.Open();
                checkBoxInv();
                persBoxLoad();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connection.Close(); }
            
        }
        private void persBoxLoad()
        {
            try
            {

                OleDbCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM posts";
                OleDbDataReader reader = command.ExecuteReader();
                string name;
                string id;
                while(reader.Read())
                {
                    StackPanel item;
                    item = new StackPanel();
                    item.Orientation = Orientation.Horizontal;
                    name = reader["post"].ToString();
                    id = reader["post_id"].ToString();
                    Label postname = new Label();
                    postname.Content = name;
                    Label postId = new Label();
                    postId.Tag = id;
                    item.Children.Add(postname);
                    item.Children.Add(postId);
                    persBox.Items.Add(item);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkBoxInv()
        {

            try
            {

                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM types";
                OleDbDataReader reader = command.ExecuteReader();
                string typeName;
                string typeId;
                inventaryList.Items.Clear();
                while (reader.Read())
                {

                    StackPanel item;
                    item = new StackPanel();
                    item.Orientation = Orientation.Horizontal;
                    typeName = reader["name"].ToString();
                    typeId = reader["type_id"].ToString();
                    Label tName = new Label();
                    tName.Content = typeName;
                    Label tId = new Label();
                    tId.Tag = typeId;
                    item.Children.Add(tName);
                    item.Children.Add(tId);
                    inventaryList.Items.Add(item);
                    
                }
                reader.Close();





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void reload_invNum(string type)
        {
            bool connect = false;
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                    connect = true;
                    
                }
                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM inventary WHERE type_id = " + type;
                OleDbDataReader reader = command.ExecuteReader();
                int i = 1;
                while (reader.Read())
                {
                    i++;
                }
                reader.Close();
                Label invNumm= new Label();
                invNumm.Content = i.ToString();
                invNumber.Items.Add(invNumm);
                invNumber.SelectedItem = invNumm;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connect)
                {
                    inv_log_fill();
                    connection.Close();  
                }

            }
        }

        private void inventaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            reload_invNum(currentType());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                personsLog.Items.Clear();
                connection.Open();
                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM persons WHERE post = " + currentPost(1);
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StackPanel item;
                    item = new StackPanel();
                    item.Orientation = Orientation.Horizontal;
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Vertical;
                    StackPanel stackPanel1 = new StackPanel();
                    stackPanel1.Orientation = Orientation.Vertical;
                    string FirstName = reader["first_name"].ToString();
                    string FatherName = reader["father_name"].ToString();
                    string LastName = reader["last_name"].ToString();
                    string login = reader["name"].ToString();
                    Label fstName = new Label();
                    fstName.Content = FirstName;
                    Label fthrName = new Label();
                    fthrName.Content = FatherName;
                    Label lstName = new Label();
                    lstName.Content = LastName;
                    Label logName = new Label();
                    logName.Content = "Логин - " + login;
                    Label post = new Label();
                    post.Content = currentPost(0);

                    item.Children.Add(post);
                    stackPanel.Children.Add(fstName);
                    stackPanel.Children.Add(fthrName);
                    stackPanel.Children.Add(lstName);
                    item.Children.Add(stackPanel);
                    stackPanel1.Children.Add(logName);
                    item.Children.Add(stackPanel1);
                    personsLog.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
                
            }

        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Label num = invNumber.SelectedItem as Label;
                connection.Open();
                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "INSERT inventary (type_id, status, number) VALUES ('"+currentType()+"', '1', '"+num.Content.ToString()+"') " ;
                command.ExecuteNonQuery();

            }  
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            { 
                connection.Close();
                reload_invNum(currentType());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                winpers = new Window1(currentPost(1), connectToOther);
                winpers.Owner = this;
                winpers.Title = "Новый " + currentPost(0);
                winpers.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
    }
}
