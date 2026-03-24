using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class SwaggerController : Controller
    {
        // Página con Swagger UI (usa CDN)
        public ActionResult Index()
        {
            return View();
        }

        // Devuelve un Swagger 2.0 (OpenAPI) mínimo en JSON para las rutas existentes
        public ActionResult Docs()
        {
            string json = @"{
  \"swagger\": \"2.0\",
  \"info\": {
    \"version\": \"v1\",
    \"title\": \"API TiendaOnline\"
  },
  \"basePath\": \"/\",
  \"schemes\": [\"http\",\"https\"],
  \"paths\": {
    \"/Home/ListarUsuarios\": {
      \"get\": {
        \"tags\": [\"Home\"],
        \"summary\": \"Listar usuarios\",
        \"produces\": [\"application/json\"],
        \"responses\": {
          \"200\": {
            \"description\": \"OK\",
            \"schema\": {
              \"type\": \"object\",
              \"properties\": {
                \"data\": {
                  \"type\": \"array\",
                  \"items\": { \"$ref\": \"#/definitions/Usuario\" }
                }
              }
            }
          }
        }
      }
    },
    \"/Home/GuardarUsuario\": {
      \"post\": {
        \"tags\": [\"Home\"],
        \"summary\": \"Guardar usuario (Registrar/Editar)\",
        \"consumes\": [\"application/json\"],
        \"parameters\": [
          { \"in\": \"body\", \"name\": \"objeto\", \"schema\": { \"$ref\": \"#/definitions/Usuario\" } }
        ],
        \"responses\": { \"200\": { \"description\": \"OK\" } }
      }
    },
    \"/Home/EliminarUsuario\": {
      \"post\": {
        \"tags\": [\"Home\"],
        \"summary\": \"Eliminar usuario\",
        \"consumes\": [\"application/json\"],
        \"parameters\": [
          { \"in\": \"body\", \"name\": \"id\", \"schema\": { \"type\": \"object\", \"properties\": { \"id\": { \"type\": \"integer\" } } } }
        ],
        \"responses\": { \"200\": { \"description\": \"OK\" } }
      }
    }
  },
  \"definitions\": {
    \"Usuario\": {
      \"type\": \"object\",
      \"properties\": {
        \"IdUsuario\": { \"type\": \"integer\", \"format\": \"int32\" },
        \"Nombre\": { \"type\": \"string\" },
        \"Apellido\": { \"type\": \"string\" },
        \"Correo\": { \"type\": \"string\" },
        \"Clave\": { \"type\": \"string\" },
        \"Reestablecer\": { \"type\": \"boolean\" },
        \"Activo\": { \"type\": \"boolean\" }
      }
    }
  }
}";

            return Content(json, "application/json");
        }
    }
}
