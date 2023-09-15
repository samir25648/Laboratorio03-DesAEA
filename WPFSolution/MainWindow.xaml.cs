using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WPFSolution.Views
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=LAB1504-28\\SQLEXPRESS;Initial Catalog=Tecsup202DB;User ID=Tecsup;Password=123456";
        private List<Producto> productoList;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            productoList = new List<Producto>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM [Productos-1]", connection)) // Usar [Productos-1] para manejar el nombre de la tabla con guiones
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productoList.Add(new Producto
                            {
                                IdProducto = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Categoria = reader.GetString(2),
                                Precio = reader.GetDecimal(3),
                                FechaVencimiento = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
            }

            productoDataGrid.ItemsSource = productoList;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                List<Producto> filteredList = productoList.FindAll(producto =>
                    producto.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    producto.Categoria.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                productoDataGrid.ItemsSource = filteredList;
            }
            else
            {
                productoDataGrid.ItemsSource = productoList;
            }
        }
    }

    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
