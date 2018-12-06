using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class ContratoVehiculo : Contrato
    {
        public string Patente { get; set; }
        public int IdMarca { get; set; }
        public int IdModelo { get; set; }
        public int Anho { get; set; }

        public ContratoVehiculo():base()
        {

        }
        public new bool Create()
        {
            DatosDB.Contrato contrato = new DatosDB.Contrato();
            DatosDB.Vehiculo vehiculo = new DatosDB.Vehiculo();
            try
            {
                LeerClientePlan();
                CommonBC.Syncronize(this, contrato);
                Conexion.Contexto.Contrato.Add(contrato);
                CommonBC.Syncronize(this,vehiculo);
                contrato.Vehiculo.Add(vehiculo);
                Conexion.Contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Conexion.Contexto.Contrato.Remove(contrato);
                Conexion.Contexto.Vehiculo.Remove(vehiculo);
                return false;
            }
        }

        public new bool Read()
        {
            
            try
            {
                DatosDB.Contrato contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);

                CommonBC.Syncronize(contrato, this);
                LeerClientePlan();
                this.Update();
                DatosDB.Vehiculo vehiculo = contrato.Vehiculo.FirstOrDefault();
                CommonBC.Syncronize(vehiculo, this);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            //Leer el cliente y plan asociados a este contrato
            
        }

        private void LeerClientePlan()
        {
            NegocioBL.Cliente RegClie = new NegocioBL.Cliente();
            RegClie.RutCliente = this.RutCliente;
            if (RegClie.Read())
            {
                //NegocioBL.Cliente Sex = new NegocioBL.Sexo();
                //Sex.IdSexo = RegSex.IdSexo;
                //Sex.Descripcion = RegSex.Descripcion;
                //this.Sexo = Sex;
                this.Cliente = RegClie;
            }
            else
            {
                throw new ArgumentException("No puede crear un contrato a un Cliente no registrado!!!");
            }
            NegocioBL.Plan RegPlan = new NegocioBL.Plan();
            RegPlan.IdPlan = this.CodigoPlan;
            if (RegPlan.Read())
            {
                //NegocioBL.EstadoCivil Est = new NegocioBL.EstadoCivil();
                //Est.IdEstadoCivil = RegEst.IdEstadoCivil;
                //Est.Descripcion = RegEst.Descripcion;
                //this.EstadoCivil = Est;
                this.Plan = RegPlan;
            }
            else
            {
                throw new ArgumentException("Debe seleccionar un Plan para crear un contrato!!!");
            }
        }

        public bool Update()
        {
            try
            {
                DatosDB.Contrato contrato = new DatosDB.Contrato();
                contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);
                DatosDB.Vehiculo vehiculo = Conexion.Contexto.Vehiculo.FirstOrDefault();
                CommonBC.Syncronize(this,vehiculo);
                Conexion.Contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                DatosDB.Contrato contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);
                Conexion.Contexto.Contrato.Remove(contrato);
                DatosDB.Vehiculo vehiculo = Conexion.Contexto.Vehiculo.First(v => v.Patente == Patente);
                Conexion.Contexto.Vehiculo.Remove(vehiculo);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


}
