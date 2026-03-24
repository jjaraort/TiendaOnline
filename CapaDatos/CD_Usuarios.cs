using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "SELECT IdUsuario,Nombre,Apellido,Correo,Clave,Reestablecer,Activo FROM USUARIO";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Nombre = dr["Nombre"].ToString(),
                                Apellido = dr["Apellido"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lista;
        }

        // =========================================
        // Método para Registrar un usuario 
        // =========================================
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idUsuario = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 🔹 Validar nulos antes de enviar
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = obj.Nombre ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = obj.Apellido ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = obj.Correo ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 256).Value = obj.Clave ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = obj.Activo;

                    // 🔹 Parámetros de salida
                    cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // 🔹 Manejo seguro de valores de salida
                    Mensaje = cmd.Parameters["@Mensaje"].Value?.ToString() ?? "";

                    bool resultado = false;
                    if (cmd.Parameters["@Resultado"].Value != DBNull.Value)
                        resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                    if (resultado && cmd.Parameters["@IdUsuario"].Value != DBNull.Value)
                        idUsuario = Convert.ToInt32(cmd.Parameters["@IdUsuario"].Value);
                }
            }
            catch (SqlException ex)
            {
                Mensaje = "Error SQL: " + ex.Message;
                idUsuario = 0;
            }
            catch (Exception ex)
            {
                Mensaje = "Error general: " + ex.Message;
                idUsuario = 0;
            }

            return idUsuario;
        }

        // =========================================
        // Método para editar un usuario existente
        // =========================================
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("@Clave", string.IsNullOrEmpty(obj.Clave) ? DBNull.Value : (object)obj.Clave);
                    cmd.Parameters.AddWithValue("@Activo", obj.Activo);

                    // Parámetros de salida
                    cmd.Parameters.Add("@Mensaje", SqlDbType.NVarChar, 200).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                resultado = false;
            }

            return resultado;
        }
    }
}