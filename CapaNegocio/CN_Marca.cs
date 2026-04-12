using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objcd_marca = new CD_Marca();

        public List<Marca> Listar()
        {
            return objcd_marca.Listar();
        }

        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones (aquí está la lógica de negocio)
            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía";
                return 0;
            }

            return objcd_marca.Registrar(obj, out Mensaje);
        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la marca no puede estar vacía";
                return false;
            }

            return objcd_marca.Editar(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objcd_marca.Eliminar(id, out Mensaje);
        }
    }
}
