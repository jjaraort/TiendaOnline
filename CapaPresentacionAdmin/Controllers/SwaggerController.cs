using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class SwaggerController : Controller
    {
        // Página Swagger UI
        public ActionResult Index()
        {
            return View();
        }

        // Documento Swagger JSON
        public ActionResult Docs()
        {
            string json = @"{
              ""swagger"": ""2.0"",
              ""info"": {
                ""version"": ""v1"",
                ""title"": ""API TiendaOnline""
              },
              ""basePath"": ""/"",
              ""schemes"": [""http"", ""https""],
              ""paths"": {

                ""/Home/ListarUsuarios"": {
                  ""get"": {
                    ""tags"": [""Usuarios""],
                    ""summary"": ""Listar todos los usuarios"",
                    ""produces"": [""application/json""],
                    ""responses"": {
                      ""200"": {
                        ""description"": ""Lista de usuarios"",
                        ""schema"": {
                          ""type"": ""array"",
                          ""items"": { ""$ref"": ""#/definitions/Usuario"" }
                        }
                      }
                    }
                  }
                },

                ""/Home/BuscarUsuario/{id}"": {
                  ""get"": {
                    ""tags"": [""Usuarios""],
                    ""summary"": ""Obtener usuario por ID"",
                    ""parameters"": [
                      {
                        ""name"": ""id"",
                        ""in"": ""path"",
                        ""required"": true,
                        ""type"": ""integer""
                      }
                    ],
                    ""responses"": {
                      ""200"": {
                        ""description"": ""Usuario encontrado"",
                        ""schema"": { ""$ref"": ""#/definitions/Usuario"" }
                      },
                      ""404"": { ""description"": ""No encontrado"" }
                    }
                  }
                },

                ""/Home/GuardarUsuario"": {
                  ""post"": {
                    ""tags"": [""Usuarios""],
                    ""summary"": ""Crear usuario"",
                    ""consumes"": [""application/json""],
                    ""parameters"": [
                      {
                        ""in"": ""body"",
                        ""name"": ""usuario"",
                        ""required"": true,
                        ""schema"": { ""$ref"": ""#/definitions/Usuario"" }
                      }
                    ],
                    ""responses"": {
                      ""201"": { ""description"": ""Usuario creado"" },
                      ""400"": { ""description"": ""Datos inválidos"" }
                    }
                  }
                },

                ""/Home/ActualizarUsuario/{id}"": {
                  ""put"": {
                    ""tags"": [""Usuarios""],
                    ""summary"": ""Actualizar usuario"",
                    ""consumes"": [""application/json""],
                    ""parameters"": [
                      {
                        ""name"": ""id"",
                        ""in"": ""path"",
                        ""required"": true,
                        ""type"": ""integer""
                      },
                      {
                        ""in"": ""body"",
                        ""name"": ""usuario"",
                        ""required"": true,
                        ""schema"": { ""$ref"": ""#/definitions/Usuario"" }
                      }
                    ],
                    ""responses"": {
                      ""200"": { ""description"": ""Usuario actualizado"" },
                      ""404"": { ""description"": ""No encontrado"" }
                    }
                  }
                },

                ""/Home/EliminarUsuario/{id}"": {
                  ""delete"": {
                    ""tags"": [""Usuarios""],
                    ""summary"": ""Eliminar usuario"",
                    ""parameters"": [
                      {
                        ""name"": ""id"",
                        ""in"": ""path"",
                        ""required"": true,
                        ""type"": ""integer""
                      }
                    ],
                    ""responses"": {
                      ""200"": { ""description"": ""Usuario eliminado"" },
                      ""404"": { ""description"": ""No encontrado"" }
                    }
                  }
                }

              },

              ""definitions"": {
                ""Usuario"": {
                  ""type"": ""object"",
                  ""properties"": {
                    ""IdUsuario"": { ""type"": ""integer"", ""format"": ""int32"" },
                    ""Nombre"": { ""type"": ""string"" },
                    ""Apellido"": { ""type"": ""string"" },
                    ""Correo"": { ""type"": ""string"" },
                    ""Clave"": { ""type"": ""string"" },
                    ""Reestablecer"": { ""type"": ""boolean"" },
                    ""Activo"": { ""type"": ""boolean"" }
                  }
                }
              }
            }";

            return Content(json, "application/json");
        }
    }
}