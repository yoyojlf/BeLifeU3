using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class ContratoVivienda  : Contrato
    {
        public int CodigoPostal { get; set; }
        public int Anho { get; set; }
        public string Direccion { get; set; }
        public double ValorInmueble { get; set; }
        public double ValorContenido { get; set; }
        public int IdRegion { get; set; }
        public int IdComuna { get; set; }

        public ContratoVivienda():base()
        {

        }

        public new bool Create()
        {
            DatosDB.Contrato contrato = new DatosDB.Contrato();
            DatosDB.Vivienda vivienda = new DatosDB.Vivienda();
            try
            {
                LeerClientePlan();
                CommonBC.Syncronize(this, contrato);
                Conexion.Contexto.Contrato.Add(contrato);
                CommonBC.Syncronize(this, vivienda);
                contrato.Vivienda.Add(vivienda);
                Conexion.Contexto.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Conexion.Contexto.Contrato.Remove(contrato);
                Conexion.Contexto.Vivienda.Remove(vivienda);
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
                DatosDB.Vivienda vivienda = contrato.Vivienda.FirstOrDefault();
                CommonBC.Syncronize(vivienda, this);

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
                CommonBC.Syncronize(this, vehiculo);
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
                DatosDB.Vivienda vivienda = Conexion.Contexto.Vivienda.First(v => v.CodigoPostal == CodigoPostal);
                Conexion.Contexto.Vivienda.Remove(vivienda);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
