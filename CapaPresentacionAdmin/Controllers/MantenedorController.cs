using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
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
    }
}