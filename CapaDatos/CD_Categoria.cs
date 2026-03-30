using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"SELECT IdCategoria, Descripcion, Activo, FechaRegistro FROM CATEGORIA";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        conexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Categoria()
                                {
                                    IdCategoria = dr["IdCategoria"] != DBNull.Value ? Convert.ToInt32(dr["IdCategoria"]) : 0,
                                    Descripcion = dr["Descripcion"]?.ToString(),
                                    Activo = dr["Activo"] != DBNull.Value && Convert.ToBoolean(dr["Activo"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ideal: loggear en archivo o sistema (no solo consola)
                throw new Exception("Error al listar categorías", ex);
            }

            return lista;
        }
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idCategoria = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("INSERT INTO CATEGORIA(Descripcion, Activo, FechaRegistro) VALUES(@Descripcion, @Activo, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion))
                {
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        idCategoria = Convert.ToInt32(result);

                    Mensaje = "Categoria registrada correctamente";
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                idCategoria = 0;
            }

            return idCategoria;
        }
        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("UPDATE CATEGORIA SET Descripcion = @Descripcion, Activo = @Activo WHERE IdCategoria = @IdCategoria", conexion))
                {
                    cmd.Parameters.AddWithValue("@IdCategoria", obj.IdCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    resultado = filas > 0;

                    Mensaje = resultado ? "Categoria actualizada correctamente" : "No se encontró la categoría";
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
                    string query = "DELETE FROM CATEGORIA WHERE IdCategoria = @IdCategoria";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdCategoria", id);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filas = cmd.ExecuteNonQuery();

                    respuesta = filas > 0;
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
