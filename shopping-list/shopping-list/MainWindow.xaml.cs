using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Printing;
using System.IO;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace shopping_list
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //objects 
        SqlConnection con = new SqlConnection(); //alow to create connection to db (access to table-we make connection)
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();// comunicate data in correct format

        private int GroseryId = 0;

        public MainWindow()
        {
            InitializeComponent();
            BindingGrosery();
            GetData();
            ClearMaster();
        }


        //metoda za string konekciju i otvaranje
        public void mycon()
        {
            String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(Conn);
            con.Open();
        }
        //validacija numerickih brojava
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //gumbic za dodavanje na listu
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtInsertItem.Text == null || txtInsertItem.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter grocery item", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtInsertItem.Focus();
                    return;
                }
                else if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtAmount.Focus();
                    return;
                }
                else
                {
                    if (GroseryId > 0)
                    {
                        if (MessageBox.Show($"Update item: {txtInsertItem.Text}", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            //update
                            mycon();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("UPDATE ItemsDb SET Amount = @Amount, Item = @Item WHERE Id = @Id", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", GroseryId);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@Item", txtInsertItem.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Shopping list updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        //add
                        mycon();
                        DataTable dt = new DataTable();
                        cmd = new SqlCommand("INSERT INTO ItemsDb(Item, Amount) VALUES(@Item, @Amount)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Item", txtInsertItem.Text);
                        cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }
                ClearMaster();
                Delete.Visibility = Visibility.Hidden;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ClearMaster()
        {
            try
            {
                txtInsertItem.Text = string.Empty;
                txtAmount.Text = string.Empty;
                btnAdd.Content = "Add item";
                GetData();
                GroseryId = 0;
                BindingGrosery();
                flowDocument.Blocks.Clear();
                txtInsertItem.Focus();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //ispisivanje(povezivanje) svega iz baze u tablicu
        private void BindingGrosery()
        {

            mycon();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SELECT Id, Amount, Item FROM ItemsDb", con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //stvaranje tablice u koju ce se spremiti to sa baze
            DataRow newRow = dt.NewRow();
            newRow["Id"] = 0;
            newRow["Item"] = "--insert item--";
            dt.Rows.InsertAt(newRow, 0);



            con.Close();

        }
        public void GetData()
        {
            mycon();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("SELECT * FROM ItemsDb ", con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt != null && dt.Rows.Count > 0)
            {
                dgvGrosery.ItemsSource = dt.DefaultView;
            }
            else
            {
                dgvGrosery.ItemsSource = null;
            }
            con.Close();
        }


        private void btnDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            DeleteAll();
        }

        private void DeleteAll()
        {
            try
            {
                if (MessageBox.Show("Are you sure you want delete all?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    txtInsertItem.Text = string.Empty;
                    txtAmount.Text = string.Empty;

                    mycon();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("DELETE FROM ItemsDb", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Shopping list deleted successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                    //dgvGrosery.ItemsSource = null;
                    txtInsertItem.Focus();
                    GetData();
                    flowDocument.Blocks.Clear();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void btnCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sbClipboardStringText = new StringBuilder();

            foreach (object dataItem in dgvGrosery.Items)
            {
                //System.Diagnostics.Debug.WriteLine(dataItem.GetType().FullName);

                var drv = dataItem as DataRowView;

                int id = (int)drv["Id"];
                string item = (string)drv["Item"];
                int amount = (int)drv["Amount"];

                string letterNumber = item.Trim();
                if (letterNumber.Length >= 7)
                {
                    sbClipboardStringText.AppendFormat("{0} \t \t {1}\n", item.Trim(), amount);
                }
                else
                {
                    sbClipboardStringText.AppendFormat("{0} \t \t \t {1}\n", item.Trim(), amount);
                }
            }

            string result = sbClipboardStringText.ToString();
            Clipboard.SetData(DataFormats.Text, (object)result);
            Paragraph p = new Paragraph();
            p.Margin = new Thickness(150, 5, 5, 5);
            p.Inlines.Add(result);
            flowDocument.Blocks.Add(p);
            ClearMaster();
        }


        private void dgvGrosery_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                Delete.Visibility = Visibility.Visible;

                DataGrid grd = (DataGrid)sender;
                DataRowView row_selected = grd.CurrentItem as DataRowView;

                if (row_selected != null)
                {
                    if (dgvGrosery.Items.Count > 0)
                    {
                        if (grd.SelectedCells.Count > 0)
                        {
                            GroseryId = Int32.Parse(row_selected["Id"].ToString());
                            if (grd.SelectedCells[0].Column.DisplayIndex == 1 || grd.SelectedCells[0].Column.DisplayIndex == 2)
                            {
                                txtAmount.Text = row_selected["Amount"].ToString();

                                txtInsertItem.Text = row_selected["Item"].ToString();
                                btnAdd.Content = "Update";
                            }

                            if (grd.SelectedCells[0].Column.DisplayIndex == 3)
                            {
                                txtAmount.Text = row_selected["Amount"].ToString();
                                txtInsertItem.Text = row_selected["Item"].ToString();

                                if (MessageBox.Show($"Delete item: {txtInsertItem.Text}", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    mycon();
                                    DataTable dt = new DataTable();

                                    cmd = new SqlCommand("DELETE FROM ItemsDb WHERE Id = @Id", con);
                                    cmd.CommandType = CommandType.Text;

                                    cmd.Parameters.AddWithValue("@Id", GroseryId);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    MessageBox.Show("Item deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClearMaster();
                                    Delete.Visibility = Visibility.Hidden;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sbClipboardStringText = new StringBuilder();
            sbClipboardStringText.AppendLine("Shopping list:\n \b ");

            foreach (object dataItem in dgvGrosery.Items)
            {
                var drv = dataItem as DataRowView;

                int id = (int)drv["Id"];
                string item = (string)drv["Item"];
                int amount = (int)drv["Amount"];

                string letterNumber = item.Trim();

                if (letterNumber.Length >= 9)
                {
                    sbClipboardStringText.AppendFormat("{0} \t \t {1}\n", item.Trim(), amount);
                }
                else
                {
                    sbClipboardStringText.AppendFormat("{0} \t \t \t {1}\n", item.Trim(), amount);
                }
            }

            string result = sbClipboardStringText.ToString();
            //Clipboard.SetData(DataFormats.Text, (object)result);

            Paragraph p = new Paragraph();
            p.Margin = new Thickness(150, 5, 5, 5);
            p.Inlines.Add(result);
            flowDocument.Blocks.Add(p);

            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;

            flowDocument.PageHeight = pd.PrintableAreaHeight;
            flowDocument.PageWidth = pd.PrintableAreaWidth;

            IDocumentPaginatorSource idocument = flowDocument as IDocumentPaginatorSource;

            pd.PrintDocument(idocument.DocumentPaginator, "Printing Flow Document...");

            ClearMaster();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            StringBuilder sbClipboardStringText = new StringBuilder();

            //sbClipboardStringText.AppendLine("Shopping list: ");

            foreach (object dataItem in dgvGrosery.Items)
            {
                var drv = dataItem as DataRowView;

                int id = (int)drv["Id"];
                string item = (string)drv["Item"];
                int amount = (int)drv["Amount"];

                string itemTrim = item.Trim();
                sbClipboardStringText.AppendFormat("{1}\t{0}\n", itemTrim, amount);

            }

            string result = sbClipboardStringText.ToString();
            var savedlg = new SaveFileDialog();
            
            
            savedlg.DefaultExt = "doc";
            savedlg.Filter = "Document file (*.doc)|*.doc|Text file (*.txt)|*.txt| Excel file (*.xls) | *.xls| All files(*.*) | *.* ";
            //savedlg.InitialDirectory = @"C:\Users\Korisnik\Desktop\proba";
            savedlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (savedlg.ShowDialog() == true)
                File.WriteAllText(savedlg.FileName, result);
            
        }
    }
}



