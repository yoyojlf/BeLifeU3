using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class Tarificador
    {
        #region Campos propiedades
        public Cliente cliente { get; set; }
        #endregion

        

        #region Constructor
        public Tarificador(Cliente clie)
        {
            Init();
            cliente = clie;
        }

        public Tarificador()
        {
            Init();
        }

        private void Init()
        {
            cliente = new Cliente();
        }
        #endregion

        #region Metodos
        public double CalcularPrimaBase(double Base)
        {
            double Prima = -1;
            
            int edad = 0;
            if (Base >= 0.0d && Base <= 20.0d)
            {
                if (ValidarEdad())
                {
                    Prima = Base;
                    edad = CalcularEdad();
                    if (edad >= 18 && edad <= 25) Prima += 0.3d;
                    if (edad >= 26 && edad <= 45) Prima += 0.2d;
                    if (edad >= 46 && edad <= 50) Prima += 0.5d;
                    switch (cliente.IdSexo)
                    {
                        case 1:
                            Prima += 0.2d;
                            break;
                        case 2:
                            Prima += 0.1d;
                            break;
                        default:
                            Prima += 0;
                            break;
                    }
                    switch (cliente.IdEstadoCivil)
                    {
                        case 1:
                            Prima += 0.4d;
                            break;
                        case 2:
                            Prima += 0.2d;
                            break;
                        default:
                            Prima += 0.3d;
                            break;
                    }
                }
                else
                {
                    throw new ArgumentException("La edad esta fuera de rango.\nSolo se admiten valores ente 18 y 50.");
                }
            }
            else
            {
                throw new ArgumentException("El Valor de la prima Base esta Fuera de rango.\nSolo se haceptan valores entre 0 y 20.");
            }
            return Prima;
        }

        #region Calcular Edad
        private int CalcularEdad()
        {
           
            TimeSpan dif = DateTime.Now - cliente.FechaNacimiento;
            int anios = (int)(Math.Truncate(dif.TotalDays / 365.25));

            return anios;
        }
        #endregion

        #region Validar Edad
        private bool ValidarEdad()
        {
            bool Exito = false;
            int Edad = CalcularEdad();
            if(Edad >= 18 && Edad <= 50)
            {
                Exito = true;
            }
            return Exito;
        }
        #endregion

        #endregion
    }
}
