using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatosDB;

namespace NegocioBL
{
    public class Cliente
    {

        #region Parametros
        string rut;
        string nombre;
        string apellido;
        DateTime fechaNacimiento;
        #endregion

        #region Propiedades
        public string RutCliente
        {
            set
            {
                if(value != string.Empty)
                {
                    rut = value;
                }
                else
                {
                    throw new ArgumentException("El rut no puede estar vacio!!!!");
                }
            }
            get
            {
                return rut;
            }
        }
        
        public string Nombres
        {
            get
            {
                return nombre;
            }
            set
            {
                if(value != string.Empty)
                {
                    nombre = value;
                }
                else
                {
                    throw new ArgumentException("El nombre no puede estar vacio!!!");
                }
            }
        }
        
        public string Apellidos
        {
            get
            {
                return apellido;
            }
            set
            {
                if(value != string.Empty)
                {
                    apellido = value;
                }
                else
                {
                    throw new ArgumentException("El apellido no puede estar vacio!!!");
                }
            }
        }
        
        public DateTime FechaNacimiento
        {
            get
            {
                return fechaNacimiento;
            }
            set
            {
                TimeSpan a = DateTime.Today.Subtract(value); //estima el tiempo que ha trascurrido desde nacimiento
                double b = a.TotalDays / 365.25;
                int edad = Convert.ToInt32(b);
                DateTime fechamenos = DateTime.Today.AddYears(-18); //estimo la fecha de hoy hace 18 años atras
                TimeSpan diasMedad = DateTime.Today.Subtract(fechamenos); //estimo lapsus de tiempo que seria de mayor de edad
                                                                          //int anios = (int)(a.TotalDays/365);
                #region calculaEdad
                TimeSpan dif = DateTime.Now - value;
                int anios = (int)(Math.Truncate(dif.TotalDays/365.25));
                #endregion
                //if(edad >= 18)
                if (a.Days >= diasMedad.Days) //verifico que el tiempo transcurrido desde su nacimiento sea mayor o igual a 18 años, comparando la cantidad de dias 
                {
                    fechaNacimiento = value;
                    //throw new ArgumentException("la edad del wey es de: " + edad);
                    //throw new ArgumentException("La edad del wey es: "+anios);
                    //throw new ArgumentException("edad: " + Math.Truncate(b));
                    //throw new ArgumentException(anios + "");
                }
                else
                {
                    //throw new ArgumentException("La edad del cliente tiene que ser mayor o igual a 18 años!!! "); //+edad);
                    //throw new ArgumentException("la edad del wey es menor, edad: " + anios+" b: "+Math.Truncate(b));
                    throw new ArgumentException("NO REGISTRADO!!, El cliente tiene que ser mayor de 18 años!!!\nEdad: "+anios);
                }
            }
        }

        public int IdSexo { get; set; }
        public int IdEstadoCivil { get; set; }

        public Sexo Sexo { get; set; }

        public EstadoCivil EstadoCivil { get; set; }
        #endregion

        #region Constructor
        public Cliente()
        {
            this.Init();
        }
        #endregion

        #region inicializador
        private void Init()
        {
            nombre = string.Empty;
            apellido = string.Empty;
            rut = string.Empty;
            fechaNacimiento = new DateTime(1990, 1, 1);
            //Sexo = Sexo.Femenino;
            //EstadoCivil = EstadoCivil.Soltero;
        }
        #endregion

        #region Metodos
        public bool Read()
        {
            
            try
            {
                DatosDB.Cliente Clien = Conexion.Contexto.Cliente.First(b => b.RutCliente == RutCliente);
                CommonBC.Syncronize(Clien, this);
                this.LeerDescripcion();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void LeerDescripcion()
        {
            NegocioBL.Sexo RegSex = new NegocioBL.Sexo();
            RegSex.IdSexo = this.IdSexo;
            if (RegSex.Read())
            {
                NegocioBL.Sexo Sex = new NegocioBL.Sexo();
                Sex.IdSexo = RegSex.IdSexo;
                Sex.Descripcion = RegSex.Descripcion;
                this.Sexo = Sex;
            }
            NegocioBL.EstadoCivil RegEst = new NegocioBL.EstadoCivil();
            RegEst.IdEstadoCivil = this.IdEstadoCivil;
            if (RegEst.Read())
            {
                NegocioBL.EstadoCivil Est = new NegocioBL.EstadoCivil();
                Est.IdEstadoCivil = RegEst.IdEstadoCivil;
                Est.Descripcion = RegEst.Descripcion;
                this.EstadoCivil = Est;
            }
        }

        public bool Create()
        {
            
            DatosDB.Cliente Clien = new DatosDB.Cliente();
            try
            {
                CommonBC.Syncronize(this, Clien);
                Conexion.Contexto.Cliente.Add(Clien);
                Conexion.Contexto.SaveChanges();

                return true;
            }catch(Exception ex)
            {
                Conexion.Contexto.Cliente.Remove(Clien);

                return false;
            }
        }

        public bool Delete()
        {
            
            try
            {
                DatosDB.Cliente Clien = Conexion.Contexto.Cliente.First(b => b.RutCliente == RutCliente);
                Conexion.Contexto.Cliente.Remove(Clien);
                Conexion.Contexto.SaveChanges();

                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public List<Cliente> ReadAll()
        {
            
            try
            {
                List<DatosDB.Cliente> ListaBD = Conexion.Contexto.Cliente.ToList<DatosDB.Cliente>();
                //Lista de Salida
                List<Cliente> ListaBiblioteca = GenerarListaClientes(ListaBD);

                return ListaBiblioteca;
            }catch(Exception ex)
            {
                return new List<Cliente>();
            }
        }

        private List<Cliente> GenerarListaClientes(List<DatosDB.Cliente> listaBD)
        {
            List<Cliente> ListaClie = new List<Cliente>();
            foreach(DatosDB.Cliente Dato in listaBD)
            {
                Cliente Clien = new Cliente();
                CommonBC.Syncronize(Dato, Clien);
                Clien.LeerDescripcion();
                ListaClie.Add(Clien);
            }
            return ListaClie;
        }

        public bool Update()
        {
            
            try
            {
                DatosDB.Cliente Clien = Conexion.Contexto.Cliente.First(b => b.RutCliente == RutCliente);
                CommonBC.Syncronize(this, Clien);

                Conexion.Contexto.SaveChanges();

                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
        //ReadAllBySexo
        public List<Cliente> ReadAllBySexo()
        {
            List<NegocioBL.Cliente> clientes = new List<Cliente>();
            var consulta = this.ReadAll().Where(s => s.IdSexo == IdSexo);
            foreach (Cliente con in consulta)
            {
                Cliente cliente = new Cliente();
                CommonBC.Syncronize(con, cliente);
                clientes.Add(cliente);
            }
            return clientes;
        }
        //ReadAllByEstadoCivil
        public List<Cliente> ReadAllByEstadoCivil()
        {
            List<NegocioBL.Cliente> clientes = new List<Cliente>();
            var consulta = this.ReadAll().Where(e => e.IdEstadoCivil == IdEstadoCivil);
            foreach (Cliente con in consulta)
            {
                Cliente cliente = new Cliente();
                CommonBC.Syncronize(con, cliente);
                clientes.Add(cliente);
            }
            return clientes;
        }
        #endregion

    }
}
