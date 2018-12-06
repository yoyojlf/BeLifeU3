using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class ViviendaTarificador : Tarificador
    {
        public Vivienda Vivienda { get; set; }

        private void Init()
        {
            Vivienda = new Vivienda();
        }
        public ViviendaTarificador()
        {
            Init();
        }

        public ViviendaTarificador(Vivienda vivienda)
        {
            Init();
            Vivienda = vivienda;
        }

        public override double CalcularPrimaBase(double Base)
        {
            double Prima = -1;

            if(DateTime.Today.Year >= Vivienda.Anho)
            {
                int annos = DateTime.Today.Year - Vivienda.Anho;
                if (annos <= 5) Prima += 1.0d;
                if (annos > 5 && annos <= 10) Prima += 0.8d;
                if (annos > 10 && annos <= 30) Prima += 0.4d;
                if (annos > 30) Prima += 0.2d;

                Region region = new Region();
                region.IdRegion = Vivienda.IdRegion;
                region.Read();

                switch (region.NombreRegion.ToLower())
                {
                    case "metropolitana de santiago":
                        Prima += 3.2d;
                        break;
                    default:
                        Prima += 2.8d;
                        break;
                }

                Prima += Vivienda.ValorInmueble * 0.03;
                Prima += Vivienda.ValorContenido * 0.07;

                Double aux = (Prima * 10);
                aux = Math.Truncate(aux) + 0.0d;
                Prima = (aux / 10.0);
            }


            return Prima;
        }
    }
}
