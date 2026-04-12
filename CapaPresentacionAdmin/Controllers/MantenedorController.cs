using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        //VISTAS
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }

        #region Categoria
        //LISTAR
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            try
            {
                List<Categoria> oLista = new CD_Categoria().Listar();
                return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Categoria>(), mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //GUARDAR (INSERT / UPDATE)
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            int resultado = 0;
            string mensaje = "";

            try
            {
                //VALIDACIONES
                if (objeto == null)
                {
                    return Json(new { resultado = 0, mensaje = "Datos inválidos" });
                }

                if (string.IsNullOrWhiteSpace(objeto.Descripcion))
                {
                    return Json(new { resultado = 0, mensaje = "La descripción es obligatoria" });
                }

                if (objeto.IdCategoria == 0)
                {
                    // NUEVO
                    resultado = new CD_Categoria().Registrar(objeto, out mensaje);
                }
                else
                {
                    // EDITAR
                    bool respuesta = new CD_Categoria().Editar(objeto, out mensaje);
                    resultado = respuesta ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                resultado = 0;
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //ELIMINAR
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = "";

            try
            {
                if (id <= 0)
                {
                    return Json(new { resultado = false, mensaje = "Id inválido" });
                }

                respuesta = new CD_Categoria().Eliminar(id, out mensaje);
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                respuesta = false;
            }

            return Json(new { resultado = respuesta, mensaje = mensaje });
        }
<<<<<<< HEAD
        #endregion

        #region Marca
=======
        #region Categoria
>>>>>>> f70d8af6a7ef25e4f18a56b8884698b80fcf50b2
        [HttpGet]
        public JsonResult ListarMarca()
        {
            try
            {
                List<Marca> oLista = new CD_Marca().Listar();
                return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Marca>(), mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            int resultado = 0;
            string mensaje = "";

            try
            {
                // VALIDACIONES
                if (objeto == null)
                {
                    return Json(new { resultado = 0, mensaje = "Datos inválidos" });
                }

                if (string.IsNullOrWhiteSpace(objeto.Descripcion))
                {
                    return Json(new { resultado = 0, mensaje = "La descripción de la marca es obligatoria" });
                }

                if (objeto.IdMarca == 0)
                {
                    // NUEVO
                    resultado = new CD_Marca().Registrar(objeto, out mensaje);
                }
                else
                {
                    // EDITAR
                    bool respuesta = new CD_Marca().Editar(objeto, out mensaje);
                    resultado = respuesta ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                resultado = 0;
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = "";

            try
            {
                if (id <= 0)
                {
                    return Json(new { resultado = false, mensaje = "Id inválido" });
                }

                respuesta = new CD_Marca().Eliminar(id, out mensaje);
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                respuesta = false;
            }

            return Json(new { resultado = respuesta, mensaje = mensaje });
        }
        #endregion
<<<<<<< HEAD

        #region Producto
        [HttpGet]
        public JsonResult ListarProducto()
        {
            try
            {
                List<Producto> oLista = new CD_Producto().Listar();
                return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Producto>(), mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase archivoImagen)
        {
            string mensaje = "";
            bool imagenGuardada = true;

            Producto oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            //GUARDAR PRODUCTO (SOLO BD)
            int idProducto = new CN_Producto().Registrar(oProducto, out mensaje);

            if (idProducto == 0)
            {
                return Json(new
                {
                    productoCreado = false,
                    mensaje = mensaje
                });
            }

            oProducto.IdProducto = idProducto;

            //GUARDAR IMAGEN (INDEPENDIENTE)
            if (archivoImagen != null && archivoImagen.ContentLength > 0)
            {
                try
                {
                    string ruta = ConfigurationManager.AppSettings["ServidorFotos"];

                    if (!Directory.Exists(ruta))
                        Directory.CreateDirectory(ruta);

                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombreImagen = $"{idProducto}{extension}";

                    archivoImagen.SaveAs(Path.Combine(ruta, nombreImagen));

                    oProducto.RutaImagen = ruta;
                    oProducto.NombreImagen = nombreImagen;

                    new CN_Producto().GuardarDatosImagen(oProducto, out _);
                }
                catch (Exception ex)
                {
                    imagenGuardada = false;
                    mensaje += " | Error imagen: " + ex.Message;
                }
            }

            //RESPUESTA CLARA
            return Json(new
            {
                productoCreado = true,
                imagenGuardada = imagenGuardada,
                idGenerado = idProducto,
                mensaje = mensaje
            });
        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto oproducto = new CD_Producto().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oproducto.RutaImagen, oproducto.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oproducto.NombreImagen)
            },
            JsonRequestBehavior.AllowGet
            );


        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = "";

            try
            {
                if (id <= 0)
                {
                    return Json(new { resultado = false, mensaje = "Id inválido" });
                }

                respuesta = new CD_Producto().Eliminar(id, out mensaje);
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
                respuesta = false;
            }

            return Json(new { resultado = respuesta, mensaje = mensaje });
        }

        #endregion

=======
>>>>>>> f70d8af6a7ef25e4f18a56b8884698b80fcf50b2
    }
}