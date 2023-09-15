using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=LAB1504-28\\SQLEXPRESS;Initial Catalog=Tecsup202DB;User ID=Tecsup;Password=123456";

    static void Main()
    {
        Console.WriteLine("Lista de productos usando DataTable:");
        List<Producto> productosUsingDataTable = GetProductosUsingDataTable();
        foreach (var producto in productosUsingDataTable)
        {
            Console.WriteLine($"ID: {producto.IdProducto}, Nombre: {producto.Nombre}, Categoría: {producto.Categoria}, Precio: {producto.Precio}, Fecha de Vencimiento: {producto.FechaVencimiento}");
        }

        Console.WriteLine("\nLista de productos usando Lista de Objetos:");
        List<Producto> productosUsingList = GetProductosUsingList();
        foreach (var producto in productosUsingList)
        {
            Console.WriteLine($"ID: {producto.IdProducto}, Nombre: {producto.Nombre}, Categoría: {producto.Categoria}, Precio: {producto.Precio}, Fecha de Vencimiento: {producto.FechaVencimiento}");
        }
    }

    static List<Producto> GetProductosUsingDataTable()
    {
        List<Producto> productos = new List<Producto>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM [Productos-1]", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        productos.Add(new Producto
                        {
                            IdProducto = Convert.ToInt32(row["IdProducto"]),
                            Nombre = row["Nombre"].ToString(),
                            Categoria = row["Categoria"].ToString(),
                            Precio = Convert.ToDecimal(row["Precio"]),
                            FechaVencimiento = Convert.ToDateTime(row["FechaVencimiento"])
                        });
                    }
                }
            }
        }
        return productos;
    }

    static List<Producto> GetProductosUsingList()
    {
        List<Producto> productos = new List<Producto>();
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand("SELECT * FROM [Productos-1]", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        productos.Add(new Producto
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
        return productos;
    }
}

class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public decimal Precio { get; set; }
    public DateTime FechaVencimiento { get; set; }
}
