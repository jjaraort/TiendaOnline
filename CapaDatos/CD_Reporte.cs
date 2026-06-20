using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("@fechafin", fechafin);
                    cmd.Parameters.AddWithValue("@idtransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reporte()
                            {
                                FechaVenta = dr["FechaVenta"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("en-CO")).ToString(new CultureInfo("en-CO")),
                                Cantidad = dr["Cantidad"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"], new CultureInfo("en-CO")).ToString(new CultureInfo("en-CO")),
                                IdTransaccion = dr["IdTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Reporte>();
                Console.WriteLine(ex.Message);
            }

            return lista;
        }

        public DashBoard ObtenerDashBoard()
        {
            DashBoard dashBoard = new DashBoard();

            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dashBoard = new DashBoard
                            {
                                TotalClientes = Convert.ToInt32(dr["TotalCliente"]),
                                TotalVentas = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProductos = Convert.ToInt32(dr["TotalProducto"])
                            };
                        }

                    }
                }

            }
            catch
            {
                dashBoard = new DashBoard();
            }

            return dashBoard;
        }
    }
}