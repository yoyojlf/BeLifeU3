using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosDB;

namespace NegocioBL
{
    public class Region
    {
        public int IdRegion { get; set; }
        public string NombreRegion { get; set; }

        public Region()
        {

        }

        public bool Read()
        {
            try
            {
                DatosDB.Region region = Conexion.Contexto.Region.First(r => r.IdRegion == IdRegion);
                CommonBC.Syncronize(region, this);
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public List<Region> ReadAll()
        {
            try
            {
                List<DatosDB.Region> RegionListDB = Conexion.Contexto.Region.ToList<DatosDB.Region>();
                List<Region> RegionListBN = GenerarListaRegiones(RegionListDB);
                return RegionListBN;
            }catch(Exception ex)
            {
                return new List<Region>();
            }
        }

        private List<Region> GenerarListaRegiones(List<DatosDB.Region> regionListDB)
        {
            List<Region> ListaBC = new List<Region>();

            foreach (DatosDB.Region Datos in regionListDB)
            {
                Region region = new Region();
                CommonBC.Syncronize(Datos, region);
                ListaBC.Add(region);
            }

            return ListaBC;
        }
    }
}
