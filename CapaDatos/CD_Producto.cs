using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CapaDatos
{
    public class CD_Producto
    {
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT p.IdProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("m.IdMarca, m.Descripcion AS DesMarca,");
                    sb.AppendLine("c.IdCategoria, c.Descripcion AS DesCategoria,");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo");
                    sb.AppendLine("FROM PRODUCTO p");
                    sb.AppendLine("INNER JOIN MARCA m ON m.IdMarca = p.IdMarca");
                    sb.AppendLine("INNER JOIN CATEGORIA c ON c.IdCategoria = p.IdCategoria");
                    sb.AppendLine("GROUP BY p.IdProducto, p.Nombre, p.Descripcion,");
                    sb.AppendLine("p.Precio, p.Stock, p.RutaImagen, p.NombreImagen, p.Activo,");
                    sb.AppendLine("m.IdMarca, m.Descripcion,");
                    sb.AppendLine("c.IdCategoria, c.Descripcion");

                    using (SqlCommand cmd = new SqlCommand(sb.ToString(), conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        conexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            // 🔥 Diccionario para evitar duplicados por IdProducto
                            Dictionary<int, Producto> productos = new Dictionary<int, Producto>();

                            while (dr.Read())
                            {
                                int id = Convert.ToInt32(dr["IdProducto"]);

                                if (!productos.ContainsKey(id))
                                {
                                    productos[id] = new Producto()
                                    {
                                        IdProducto = id,
                                        Nombre = dr["Nombre"]?.ToString(),
                                        Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : "",
                                        oMarca = new Marca()
                                        {
                                            IdMarca = Convert.ToInt32(dr["IdMarca"]),
                                            Descripcion = dr["DesMarca"]?.ToString()
                                        },
                                        oCategoria = new Categoria()
                                        {
                                            IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                            Descripcion = dr["DesCategoria"]?.ToString()
                                        },
                                        Precio = dr["Precio"] != DBNull.Value ? Convert.ToDecimal(dr["Precio"]) : 0,
                                        Stock = dr["Stock"] != DBNull.Value ? Convert.ToInt32(dr["Stock"]) : 0,
                                        RutaImagen = dr["RutaImagen"] != DBNull.Value ? dr["RutaImagen"].ToString() : "",
                                        NombreImagen = dr["NombreImagen"] != DBNull.Value ? dr["NombreImagen"].ToString() : "",
                                        Activo = dr["Activo"] != DBNull.Value && Convert.ToBoolean(dr["Activo"])
                                    };
                                }
                            }

                            lista = productos.Values.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar productos", ex);
            }

            return lista;
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Derivar parámetros del stored procedure en tiempo de ejecución para evitar desajustes
                        oconexion.Open();
                        System.Data.SqlClient.SqlCommandBuilder.DeriveParameters(cmd);

                        // Asignar valores sólo si el parámetro existe en la definición del SP
                        if (cmd.Parameters.Contains("@Nombre")) cmd.Parameters["@Nombre"].Value = (object)obj.Nombre ?? DBNull.Value;
                        if (cmd.Parameters.Contains("@Descripcion")) cmd.Parameters["@Descripcion"].Value = (object)(obj.Descripcion ?? (object)DBNull.Value);
                        if (cmd.Parameters.Contains("@IdMarca")) cmd.Parameters["@IdMarca"].Value = obj.oMarca?.IdMarca ?? 0;
                        if (cmd.Parameters.Contains("@IdCategoria")) cmd.Parameters["@IdCategoria"].Value = obj.oCategoria?.IdCategoria ?? 0;
                        if (cmd.Parameters.Contains("@Precio")) cmd.Parameters["@Precio"].Value = obj.Precio;
                        if (cmd.Parameters.Contains("@Stock")) cmd.Parameters["@Stock"].Value = obj.Stock;
                        if (cmd.Parameters.Contains("@RutaImagen")) cmd.Parameters["@RutaImagen"].Value = (object)(obj.RutaImagen ?? (object)DBNull.Value);
                        if (cmd.Parameters.Contains("@NombreImagen")) cmd.Parameters["@NombreImagen"].Value = (object)(obj.NombreImagen ?? (object)DBNull.Value);
                        if (cmd.Parameters.Contains("@Activo")) cmd.Parameters["@Activo"].Value = obj.Activo;

                        // Asegurar que todos los parámetros de entrada o inputoutput tengan un valor
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            if ((p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                            {
                                p.Value = DBNull.Value;
                            }
                        }

                        cmd.ExecuteNonQuery();

                        if (cmd.Parameters.Contains("@Resultado") && cmd.Parameters["@Resultado"].Value != DBNull.Value)
                            idautogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);

                        if (cmd.Parameters.Contains("@Mensaje") && cmd.Parameters["@Mensaje"].Value != DBNull.Value)
                            Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return idautogenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("@Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("@Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("@RutaImagen", obj.RutaImagen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);

                    //OUTPUTS
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value) == 1;
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                resultado = false;
            }

            return resultado;
        }

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = "";

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand(@"UPDATE PRODUCTO SET RutaImagen = @RutaImagen,NombreImagen = @NombreImagen WHERE IdProducto = @IdProducto", conexion);

                    cmd.Parameters.AddWithValue("@IdProducto", oProducto.IdProducto);
                    cmd.Parameters.AddWithValue("@RutaImagen", oProducto.RutaImagen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NombreImagen", oProducto.NombreImagen ?? (object)DBNull.Value);

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                resultado = false;
            }

            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = "";

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "DELETE FROM PRODUCTO WHERE IdProducto = @IdProducto";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdProducto", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filas = cmd.ExecuteNonQuery();
                    respuesta = filas > 0;

                    Mensaje = respuesta ? "Producto eliminado correctamente" : "No se encontró el producto";
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }

    }
}