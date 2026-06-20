using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte cd_reporte = new CD_Reporte();

        public List<Reporte> Ventas(string fechainicio, string fechafin, string idtransaccion)
        {
            return cd_reporte.Ventas(fechainicio, fechafin, idtransaccion);
        }

        public DashBoard ObtenerDashBoard()
        {
            return cd_reporte.ObtenerDashBoard();
        }
    }
}
