using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class VehiculoTarificador : Tarificador
    {
        public Vehiculo Vehiculo { get; set; }
        public Cliente Cliente { get; set; }

        public VehiculoTarificador()
        {
            Init();
        }

        public VehiculoTarificador(Vehiculo varVehiculo,Cliente cliente)
        {
            Init();
            Vehiculo = varVehiculo;
            Cliente = cliente;
        }

        private void Init()
        {
            Vehiculo = new Vehiculo();
            Cliente = new Cliente();
        }

        public override double CalcularPrimaBase(double Base)
        {
            double Prima = -1;

            int edad = 0;
            if (Base >= 0.0d && Base <= 20.0d)
            {
                if (ValidarEdad())
                {
                    Prima = Base;
                    edad = CalcularEdad();
                    if (edad >= 18 && edad <= 25) Prima += 1.2d;
                    if (edad >= 26 && edad <= 45) Prima += 2.4d;
                    if (edad >= 46 && edad <= 50) Prima += 3.2d;
                    switch (Cliente.IdSexo)
                    {
                        case 1:
                            Prima += 0.8d;
                            break;
                        case 2:
                            Prima += 0.4d;
                            break;
                        default:
                            Prima += 0;
                            break;
                    }

                    int annos = -1;
                    if (DateTime.Today.Year >= Vehiculo.Anho)
                    {
                        annos = DateTime.Today.Year - Vehiculo.Anho;
                    }
                    else
                    {
                        throw new ArgumentException("El año del vehiculo no puede ser mayor al actual "+DateTime.Today.Year);
                    }

                    if (annos == 0) Prima += 1.2d;
                    if (annos != 0 && annos <= 5) Prima+=0.8d;
                    if (annos > 5) Prima += 0.4d;
                    Double aux = (Prima * 10);
                    aux = Math.Truncate(aux) + 0.0d;
                    Prima = (aux / 10.0);

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

            TimeSpan dif = DateTime.Now - Cliente.FechaNacimiento;
            int anios = (int)(Math.Truncate(dif.TotalDays / 365.25));

            return anios;
        }
        #endregion

        #region Validar Edad
        private bool ValidarEdad()
        {
            bool Exito = false;
            int Edad = CalcularEdad();
            if (Edad >= 18 && Edad <= 50)
            {
                Exito = true;
            }
            return Exito;
        }
        #endregion
    }
}
