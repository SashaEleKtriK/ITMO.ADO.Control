using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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
using System.Xml.Linq;
using System.Data.SqlClient;

namespace ITMO.ADO.Control
{
    /// <summary>
    /// Логика взаимодействия для InventaryWindow.xaml
    /// </summary>
    public partial class InventaryWindow : Window
    {

        int textId;
        private int person;
        private bool take;
        public StackPanel curItem;
       
        OleDbConnection connection = new OleDbConnection();
        public InventaryWindow(int text, int personId, string connectionString)
        {
            this.textId = text;
            connection.ConnectionString= connectionString;
            person = personId;
            InitializeComponent();
            but0.IsEnabled = true;
            but1.IsEnabled = false;
            take = true;
            data(textId);
            
         

            
        }
        private void data(int text)
        {
            try
            {
                connection.Open();
                checkBoxInv(text);
                substationListInit();
                fillLog();
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
        private void fillLog()
        {
            
            logBox.Items.Clear();
            StackPanel selected = new StackPanel();
            selected = inventaryList.SelectedItem as StackPanel;
            Label tag = selected.Children[1] as Label;
            string typefilter = tag.Tag.ToString();    
            OleDbCommand command = connection.CreateCommand();


            command.CommandText = "SELECT * FROM invent_log ORDER BY take_time DESC ";



            OleDbDataReader reader = command.ExecuteReader();



            while (reader.Read())
            {

                string invId = reader["inventary_id"].ToString();
                string persId = reader["person_take"].ToString();
                string takeTime = reader["take_time"].ToString();
                string subNum = reader["substation"].ToString();
                string log_id = reader["log_id"].ToString();
                string persIdBring = reader["person_bring"].ToString();
                string timeBring = reader["bring_time"].ToString();
                string number = "";
                string name = "";
                string FirstName = "";
                string FatherName = "";
                string LastName = "";
                string FirstNameBr = "";
                string FatherNameBr = "";
                string LastNameBr = "";
                string typeId = "";
                OleDbCommand commandd = connection.CreateCommand();
                commandd.CommandText = "SELECT * FROM inventary WHERE inventary_id = " + invId;
                OleDbDataReader dreader = commandd.ExecuteReader();
                while (dreader.Read())
                {
                    typeId = dreader["type_id"].ToString();
                    number = dreader["number"].ToString();
                    OleDbCommand commanddd = connection.CreateCommand();
                    commanddd.CommandText = "SELECT * FROM types WHERE type_id = " + typeId;
                    OleDbDataReader ddreader = commanddd.ExecuteReader();
                    while (ddreader.Read())
                    {
                        name = ddreader["name"].ToString();
                    }
                    // ddreader.Close();


                }
                //dreader.Close();
                OleDbCommand commandddd = connection.CreateCommand();
                commandddd.CommandText = "SELECT * FROM persons WHERE person_id = " + persId;
                OleDbDataReader dddreader = commandddd.ExecuteReader();
                while (dddreader.Read())
                {
                    FirstName = dddreader["first_name"].ToString();
                    LastName = dddreader["last_name"].ToString();
                    FatherName = dddreader["father_name"].ToString();
                }
                ///dddreader.Close();
                if (persIdBring.Length > 0 && timeBring.Length > 0)
                {
                       
                    OleDbCommand commanddddd = connection.CreateCommand();
                    commanddddd.CommandText = "SELECT * FROM persons WHERE person_id = " + persIdBring;
                    OleDbDataReader ddddreader = commanddddd.ExecuteReader();
                    while (ddddreader.Read())
                    {
                        FirstNameBr = ddddreader["first_name"].ToString();
                        LastNameBr = ddddreader["last_name"].ToString();
                        FatherNameBr = ddddreader["father_name"].ToString();
                    }
                        
                }

                else
                {
                    timeBring = ("");
                }
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                Label type = new Label();
                Label num = new Label();
                Label persIdName = new Label();
                Label persIdLName = new Label();
                Label perIdFName = new Label();
                Label logId = new Label();
                Label substNum = new Label();
                Label take_Time = new Label();
                Label persBringName = new Label();
                Label persBringLName = new Label();
                Label persBringFName = new Label();
                Label bringTime = new Label();
                Label Vipisal = new Label();
                Label Sdal = new Label();
                Sdal.Content = "Сдал  ";
                Vipisal.Content = "Выписал  ";
                type.Content = name;
                bringTime.Content = "Сдано  " + timeBring;
                num.Content = "№ " + number;
                persIdName.Content = FirstName;
                persIdLName.Content = LastName;
                perIdFName.Content = FatherName;
                logId.Content = log_id;
                substNum.Content = subNum;
                take_Time.Content = "Выдано  " + takeTime;
                persBringName.Content = FirstNameBr;
                persBringLName.Content = LastNameBr;
                persBringFName.Content = FatherNameBr;
                stackPanel.Children.Add(logId);
                stackPanel.Children.Add(type);
                stackPanel.Children.Add(num);
                StackPanel stackPanel1 = new StackPanel();
                StackPanel stackPanel2 = new StackPanel();
                StackPanel stackPanel3 = new StackPanel();
                StackPanel stackPanel4 = new StackPanel();
                StackPanel stackPanel5 = new StackPanel();
                stackPanel1.Orientation = Orientation.Vertical;
                stackPanel2.Orientation = Orientation.Horizontal;
                stackPanel3.Orientation = Orientation.Horizontal;
                stackPanel4.Orientation = Orientation.Vertical;
                stackPanel5.Orientation = Orientation.Vertical;
                stackPanel1.Children.Add(stackPanel2);
                stackPanel1.Children.Add(stackPanel3);
                stackPanel4.Children.Add(persIdName);
                stackPanel4.Children.Add(perIdFName);
                stackPanel4.Children.Add(persIdLName);
                stackPanel5.Children.Add(persBringName);
                stackPanel5.Children.Add(persBringFName);
                stackPanel5.Children.Add(persBringLName);
                stackPanel2.Children.Add(Vipisal);
                stackPanel3.Children.Add(Sdal);
                stackPanel2.Children.Add(stackPanel4);
                stackPanel3.Children.Add(stackPanel5);
                stackPanel3.Children.Add(bringTime);
                stackPanel2.Children.Add(take_Time);

                stackPanel.Children.Add(stackPanel1);

                stackPanel.Children.Add(substNum);
                if(typeId == typefilter)
                logBox.Items.Add(stackPanel);




                }
                reader.Close();

            
            



        }
   
        private void reconnect ()
        {
            bool connect = false;
            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                    connect = true;
                    substationListInit();
                }
                fillLog();
                reload_CheckBix_invNumber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connect)
                {
                    connection.Close();
                }

            }
        }
        private void checkBoxInv(int invid)
        {
             
            try
            {

                OleDbCommand command = connection.CreateCommand();

                command.CommandText = "SELECT * FROM types";
                OleDbDataReader reader = command.ExecuteReader();
                string typeName;
                int typeId;
                inventaryList.Items.Clear();
                while (reader.Read())
                {
                    
                    StackPanel item;
                    item = new StackPanel();
                    item.Orientation = Orientation.Horizontal;
                    typeName = reader["name"].ToString();
                    typeId = Int32.Parse(reader["type_id"].ToString());
                    Label tName = new Label();
                    tName.Content = typeName;
                    Label tId = new Label();
                    tId.Tag = typeId.ToString();
                    item.Children.Add(tName);
                    item.Children.Add(tId);
                    inventaryList.Items.Add(item);
                    if (typeId == invid)
                    {
                        inventaryList.SelectedItem = item;
                        curItem = new StackPanel();
                        curItem = item;
                    }
                }
                reader.Close();





            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void inventaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            reconnect();
            


        }
        private void substationListInit()
        {
            substationBox.Items.Clear();
            OleDbCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM substation ORDER BY substation_num";
            OleDbDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                Label type = new Label();
                Label num = new Label();
                StackPanel item;
                item = new StackPanel();
                item.Orientation = Orientation.Horizontal;
                type.Content = reader["substation_type"].ToString();
                num.Content = reader["substation_num"].ToString();
                
                item.Children.Add(type);
                item.Children.Add(num);
                substationBox.Items.Add(item);


            }
            reader.Close() ;
            
        }

        private void radio_Checked(object sender, RoutedEventArgs e)
        {
            
            Button btn = e.Source as Button;
            
            if (btn.Tag.ToString()=="1")
            {
                but0.IsEnabled = true;
                but1.IsEnabled = false;
                substationBox.IsEnabled = true;

                take = true;
            }
            if (btn.Tag.ToString() == "0")
            {
                but0.IsEnabled = false;
                but1.IsEnabled = true;
                substationBox.IsEnabled = false;
                take = false;
            }
            reconnect();
            if (btn.Tag.ToString() == "0")
            {

                substationBox.Items.Clear();

            }


        }
        private void reload_CheckBix_invNumber()
        {
            invNum.Items.Clear();
            StackPanel selected = new StackPanel();
            selected = inventaryList.SelectedItem as StackPanel;
            Label tag = selected.Children[1] as Label;
            string typeId = tag.Tag.ToString();
            

            OleDbCommand command = connection.CreateCommand();
            if (take)
            {
                command.CommandText = "SELECT * FROM inventary WHERE type_id = " + typeId + "AND status = 1 ORDER BY number";
                confirmBtn.Content = "Выписать";
            }
            else
            {
                command.CommandText = "SELECT * FROM inventary WHERE type_id = " + typeId + "AND status = 0 ORDER BY number";
                confirmBtn.Content = "Списать";
            }
            OleDbDataReader invread = command.ExecuteReader();
            string numberInv;
            string invId;
            while (invread.Read())
            {
                StackPanel numbers = new StackPanel();
                numberInv = invread["number"].ToString();
                invId = invread["inventary_id"].ToString();
                Label id = new Label();
                id.Content = numberInv;
                id.Tag = invId;
                numbers.Children.Add(id);
                invNum.Items.Add(numbers);
                
            }
            invread.Close();    

        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                connection.Open();
                OleDbTransaction OleTran = connection.BeginTransaction();
                OleDbCommand command = connection.CreateCommand();
                command.Transaction = OleTran;

                try
                {
                    Button btn = e.Source as Button;
                    string text = btn.Content.ToString();
                    StackPanel stackPanel = new StackPanel();               
                    stackPanel = invNum.SelectedItem as StackPanel;
                    Label id = stackPanel.Children[0] as Label;
                    string invId = id.Tag.ToString();
                    StackPanel stackPanel1 = new StackPanel();
                    stackPanel1 = substationBox.SelectedItem as StackPanel;
                    Label sub = stackPanel1.Children[1] as Label;
                    string subst = sub.Content.ToString();


                    if (text == "Выписать")
                    {

                        command.CommandText = "INSERT INTO invent_log (inventary_id, person_take, take_time, substation) VALUES('" + invId + "', '" + person.ToString() + "', '" +
                            DateTime.Now.ToString() + "', '" + subst + "')";
                        

                        command.ExecuteNonQuery();

                        command.CommandText = "update inventary set status = 0 where inventary_id = "+ invId;

                        command.ExecuteNonQuery();



                        OleTran.Commit();

                        


                    }
                    else
                    {
                        command.CommandText = "update invent_log set person_bring = " + person.ToString() + ", bring_time =  '"
                            + DateTime.Now.ToString() + "' WHERE inventary_id = " + invId;

                        
                        command.ExecuteNonQuery();

                        command.CommandText = "update inventary set status = 1 where inventary_id = " + invId;

                        command.ExecuteNonQuery();



                        OleTran.Commit();

                        
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
            }
            catch(Exception err)
            { MessageBox.Show(err.Message); }
            
            finally
            {
                connection.Close();
                reconnect();


            }

            

        }

        private void invNum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (confirmBtn.Content.ToString() == "Списать" && invNum.Items.Count > 0)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel = invNum.SelectedItem as StackPanel;
                Label id = stackPanel.Children[0] as Label;
                string invId = id.Tag.ToString();
                try
                {
                    connection.Open();
                    OleDbCommand command = connection.CreateCommand();
                    command.CommandText = "SELECT substation FROM invent_log WHERE inventary_id = " + invId;
                    OleDbDataReader invread = command.ExecuteReader();
                    string subNum = "";
                    while (invread.Read())
                    {
                        subNum = invread["substation"].ToString();
                    }
                    invread.Close();
                    command.CommandText = "SELECT * FROM substation WHERE substation_num = " + subNum;
                    OleDbDataReader read = command.ExecuteReader();
                    while (read.Read())
                    {
                        Label type = new Label();
                        Label num = new Label();
                        StackPanel item;
                        item = new StackPanel();
                        item.Orientation = Orientation.Horizontal;
                        type.Content = read["substation_type"].ToString();
                        num.Content = read["substation_num"].ToString();

                        item.Children.Add(type);
                        item.Children.Add(num);
                        substationBox.Items.Add(item);
                        substationBox.SelectedItem = item;
                        
                    }
                    read.Close();



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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
