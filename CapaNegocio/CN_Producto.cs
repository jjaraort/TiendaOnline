using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objcd_Producto = new CD_Producto();
        public List<Producto> Listar()
        {
            return objcd_Producto.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones (aquí está la lógica de negocio)
            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede estar vacía";
                return 0;
            }
            else if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del Producto no puede estar vacía";
                return 0;
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "La Marca del Producto no puede estar vacía";
                return 0;
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria del Producto no puede estar vacía";
                return 0;
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "El precio del Producto no puede estar vacía";
                return 0;
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "El Stock del Producto no puede estar vacía";
                return 0;
            }

            return objcd_Producto.Registrar(obj, out Mensaje);
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede estar vacía";
            }
            else if (string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del Producto no puede estar vacía";
            }
            else if (obj.oMarca.IdMarca == 0)
            {
                Mensaje = "La Marca del Producto no puede estar vacía";
            }
            else if (obj.oCategoria.IdCategoria == 0)
            {
                Mensaje = "La Categoria del Producto no puede estar vacía";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "El precio del Producto no puede estar vacía";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "El Stock del Producto no puede estar vacía";
            }

            return objcd_Producto.Editar(obj, out Mensaje);
        }

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            return objcd_Producto.GuardarDatosImagen(oProducto, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objcd_Producto.Eliminar(id, out Mensaje);
        }
    }
}
