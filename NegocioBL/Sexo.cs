using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosDB;

namespace NegocioBL
{
    public class Sexo
    {
        #region Propiedades
        public int IdSexo { get; set; }
        public string Descripcion { get; set; }
        #endregion

        #region Metodos
        public bool Read()
        {
            
            try
            {
                DatosDB.Sexo sexo = Conexion.Contexto.Sexo.First(b => b.IdSexo == IdSexo);
                CommonBC.Syncronize(sexo, this);

                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public List<Sexo> ReadAll()
        {
            
            try
            {
                List<DatosDB.Sexo> ListaSexBD = Conexion.Contexto.Sexo.ToList<DatosDB.Sexo>();
                //Lista salida
                List<Sexo> ListaSexBB = GenerarListaSexos(ListaSexBD);

                return ListaSexBB;
            }catch(Exception ex)
            {
                return new List<Sexo>();
            }
        }

        private List<Sexo> GenerarListaSexos(List<DatosDB.Sexo> listaSexBD)
        {
            List<Sexo> ListaBB = new List<Sexo>();

            foreach(DatosDB.Sexo Datos in listaSexBD)
            {
                Sexo Sex = new Sexo();
                CommonBC.Syncronize(Datos, Sex);
                ListaBB.Add(Sex);
            }

            return ListaBB;
        }
        #endregion
    }
}
