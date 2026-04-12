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
    public class CD_Marca
    {
        public List<Marca> Listar()
        {
            List<Marca> lista = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = @"SELECT IdMarca,Descripcion,Activo FROM MARCA";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.CommandType = CommandType.Text;
                        conexion.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                lista.Add(new Marca()
                                {
                                    IdMarca = dr["IdMarca"] != DBNull.Value ? Convert.ToInt32(dr["IdMarca"]) : 0,
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
                throw new Exception("Error al listar marca", ex);
            }

            return lista;
        }
        public int Registrar(Marca obj, out string Mensaje)
        {
            int IdMarca = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("INSERT INTO MARCA(Descripcion, Activo, FechaRegistro) VALUES(@Descripcion, @Activo, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS INT);", conexion))
                {
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        IdMarca = Convert.ToInt32(result);

                    Mensaje = "Marca registrada correctamente";
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                IdMarca = 0;
            }

            return IdMarca;
        }
        public bool Editar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("UPDATE MARCA SET Descripcion = @Descripcion, Activo = @Activo WHERE IdMarca = @IdMarca", conexion))
                {
                    cmd.Parameters.AddWithValue("@IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.Descripcion ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    int filas = cmd.ExecuteNonQuery();
                    resultado = filas > 0;

                    Mensaje = resultado ? "Marca actualizada correctamente" : "No se encontró la Marca";
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
                    string query = "DELETE FROM MARCA WHERE IdMarca = @IdMarca";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdMarca", id);
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
