using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosDB;

namespace NegocioBL
{
    public class TipoContrato
    {
        public int IdTipoContrato { get; set; }
        public string Descripcion { get; set; }

        public TipoContrato()
        {
                
        }

        public bool Read()
        {
            try
            {
                DatosDB.TipoContrato tipoContrato = Conexion.Contexto.TipoContrato.First(t => t.IdTipoContrato == IdTipoContrato);
                CommonBC.Syncronize(tipoContrato, this);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<TipoContrato> ReadAll()
        {
            try
            {
                List<DatosDB.TipoContrato> TipoContratoListDB = Conexion.Contexto.TipoContrato.ToList<DatosDB.TipoContrato>();
                List<TipoContrato> TipoContratoListBN = GenerarListaTipContra(TipoContratoListDB);
                return TipoContratoListBN;
            }
            catch (Exception ex)
            {
                return new List<TipoContrato>();
            }
        }

        private List<TipoContrato> GenerarListaTipContra(List<DatosDB.TipoContrato> tipoContratoListDB)
        {
            List<TipoContrato> ListaBC = new List<TipoContrato>();

            foreach (DatosDB.TipoContrato Datos in tipoContratoListDB)
            {
                TipoContrato tipoContrato = new TipoContrato();
                CommonBC.Syncronize(Datos, tipoContrato);
                ListaBC.Add(tipoContrato);
            }

            return ListaBC;
        }
    }
}
