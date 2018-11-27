using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosDB;

namespace NegocioBL
{
    public static class Conexion
    {
        private static BeLifeEntities contexto;

       

        public static BeLifeEntities Contexto
        {
            get
            {
                if(contexto == null)
                {
                    contexto = new BeLifeEntities();
                }
                return contexto;
            }
        }
    }
}
