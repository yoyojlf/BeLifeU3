using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NegocioBL
{
    public class Contrato
    {
        private DateTime fechaInicioVigencia;
        public string Numero { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string RutCliente { get; set; }
        public string CodigoPlan { get; set; }
        public DateTime FechaInicioVigencia {
            get
            {
                return fechaInicioVigencia;
            }
            set
            {
                if (value >= FechaCreacion && value <= FechaCreacion.AddMonths(1))
                {
                    fechaInicioVigencia = value;
                    
                }
                else
                {
                    throw new ArgumentException("La fecha de inicio de vigencia no puede ser menor a la fecha de creacion ni mayor a un mes despues de esta!!!");
                }
            }
        }
        public DateTime FechaFinVigencia { get; set; }
        public bool Vigente { get; set; }
        public bool DeclaracionSalud { get; set; }
        public double PrimaAnual { get; set; }
        public double PrimaMensual { get; set; }
        public string Observaciones { get; set; }

        public Cliente Cliente { get; set; }
        public Plan Plan { get; set; }

        #region Constructor
        public Contrato()
        {
            Init();
        }

        #region Iniciador
        private void Init()
        {
            Numero = this.ObtNumContrato();
            FechaCreacion = new DateTime();
            FechaCreacion = DateTime.Today;
            CodigoPlan = String.Empty;
            fechaInicioVigencia = new DateTime();
            FechaInicioVigencia = DateTime.Today;
            FechaFinVigencia = new DateTime();
            Vigente = false;
            DeclaracionSalud = false;
            PrimaAnual = 0f;
            PrimaMensual = 0f;
            Observaciones = string.Empty;
        }
        #endregion

        #endregion

        #region Metodos
        //cargar fecha fin vigencia
        public void FinVigencia()
        {
            FechaFinVigencia = FechaInicioVigencia.AddYears(1);
        }
        //creacion de numero contrato
        private string ObtNumContrato()
        {

            int mes = DateTime.Now.Month;
            int dia = DateTime.Now.Day;
            int hora = DateTime.Now.Hour;
            int minu = DateTime.Now.Minute;
            int seg = DateTime.Now.Second;

            string anno = DateTime.Now.Year + "";
            string mess, dias, horr, minus, segu;

            if (mes < 10) { mess = "0" + mes; } else { mess = mes + ""; }
            if (dia < 10) { dias = "0" + dia; } else { dias = dia + ""; }
            if (hora < 10) { horr = "0" + hora; } else { horr = hora + ""; }
            if (minu < 10) { minus = "0" + minu; } else { minus = minu + ""; }
            if (seg < 10) { segu = "0" + seg; } else { segu = seg + ""; }


            return anno + mess + dias + horr + minus + segu;
        }
        //Leer contrato
        public bool Read()
        {
            
            try
            {
                DatosDB.Contrato contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);
                CommonBC.Syncronize(contrato, this);
                this.LeerClientePlan();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Leer el cliente y plan asociados a este contrato
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
        }
        //Crear un nuevo contrato
        public bool Create()
        {
            
            DatosDB.Contrato contrato = new DatosDB.Contrato();
            try
            {
                LeerClientePlan();
                CommonBC.Syncronize(this, contrato);
                Conexion.Contexto.Contrato.Add(contrato);
                Conexion.Contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Conexion.Contexto.Contrato.Remove(contrato);

                return false;
            }
        }
        //Borrar contrato
        public bool Delete()
        {
            
            try
            {
                DatosDB.Contrato contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);
                Conexion.Contexto.Contrato.Remove(contrato);
                Conexion.Contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //Leer todos los contratos
        public List<Contrato> ReadAll()
        {
            
            try
            {
                List<DatosDB.Contrato> ListaBD = Conexion.Contexto.Contrato.ToList<DatosDB.Contrato>();
                //Lista de Salida
                List<Contrato> ListaBiblioteca = GenerarListaContratos(ListaBD);

                return ListaBiblioteca;
            }
            catch (Exception ex)
            {
                return new List<Contrato>();
            }
        }
        //genera lista de contratos
        private List<Contrato> GenerarListaContratos(List<DatosDB.Contrato> listaBD)
        {
            List<Contrato> ListaContrato = new List<Contrato>();
            foreach (DatosDB.Contrato Dato in listaBD)
            {
                Contrato contrato = new Contrato();
                CommonBC.Syncronize(Dato, contrato);
                contrato.LeerClientePlan();
                ListaContrato.Add(contrato);
            }
            return ListaContrato;
        }
        //Actualiza contratos
        public bool Update()
        {
            
            try
            {
                DatosDB.Contrato contrato = Conexion.Contexto.Contrato.First(c => c.Numero == Numero);
                LeerClientePlan();
                CommonBC.Syncronize(this, contrato);

                Conexion.Contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //ReadAllByNumeroContrato es lo musmo que el Read()
        //ReadAllByRut 
        public List<Contrato> ReadAllByRut()
        {
            List<NegocioBL.Contrato> contratos = new List<Contrato>();
            var consulta = this.ReadAll().Where(c => c.RutCliente == RutCliente);
            foreach(Contrato con in consulta)
            {
                Contrato contrato = new Contrato();
                CommonBC.Syncronize(con, contrato);
                contratos.Add(contrato);
            }
            return contratos;
        }
        //ReadAllByPoliza
        public List<Contrato> ReadAllByPoliza()
        {
            List<NegocioBL.Contrato> contratos = new List<Contrato>();
            var consulta = this.ReadAll().Where(c => c.CodigoPlan == CodigoPlan);
            foreach (Contrato con in consulta)
            {
                Contrato contrato = new Contrato();
                CommonBC.Syncronize(con, contrato);
                contratos.Add(contrato);
            }
            return contratos;
        }
        #endregion
    }
}
