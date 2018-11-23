using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class Plan
    {
        public string IdPlan { get; set; }
        public string Nombre { get; set; }
        public double PrimaBase { get; set; }
        public string PolizaActual { get; set; }

        public Plan()
        {
            IdPlan = string.Empty;
            Nombre = string.Empty;
            PrimaBase = 0.0;
            PolizaActual = string.Empty;
        }

        #region Metodos
        //Metodo para Leer un plan
        public bool Read()
        {
            
            try
            {
                DatosDB.Plan plan = Conexion.Contexto.Plan.First(p => p.IdPlan == IdPlan);
                CommonBC.Syncronize(plan, this);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Metodo para leer todos los planes
        public List<Plan> ReadAll()
        {
            
            try
            {
                List<DatosDB.Plan > ListaPlanesBD = Conexion.Contexto.Plan.ToList<DatosDB.Plan >();
                //Lista salida
                List<Plan> ListaPlanesBC = GenerarListaPlanes(ListaPlanesBD);

                return ListaPlanesBC;
            }
            catch (Exception ex)
            {
                return new List<Plan>();
            }
        }

        private List<Plan> GenerarListaPlanes(List<DatosDB.Plan> listaPlanBD)
        {
            List<Plan> ListaBC = new List<Plan>();

            foreach (DatosDB.Plan Datos in listaPlanBD)
            {
                Plan plan = new Plan();
                CommonBC.Syncronize(Datos, plan);
                ListaBC.Add(plan);
            }

            return ListaBC;
        }
        #endregion
    }
}
