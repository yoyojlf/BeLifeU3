using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using NegocioBL;

namespace BeLifeU3
{
    /// <summary>
    /// Lógica de interacción para BeLifeWindows.xaml
    /// </summary>
    public partial class BeLifeWindows : MetroWindow
    {
        int IndexWin=-1;
        string empresa = "Seguros BeLife ";
        string modulo = "";
        public BeLifeWindows()
        {
            InitializeComponent();
            this.Title = empresa;
            CargaComboBoxCliente();
            CargaCbContrato();
            CargaConTipoContrato();
            Limpiar();
           
        }

        

        #region Inicializa CB
        private void CargaComboBoxCliente()
        {
            Sexo sexo = new Sexo();
            CbSexo.ItemsSource = sexo.ReadAll();
            CbSexo.SelectedValuePath = "IdSexo";
            CbSexo.DisplayMemberPath = "Descripcion";

            EstadoCivil estadoCivil = new EstadoCivil();
            CbEstadoCivil.ItemsSource = estadoCivil.ReadAll();
            CbEstadoCivil.SelectedValuePath = "IdEstadoCivil";
            CbEstadoCivil.DisplayMemberPath = "Descripcion";
        }
        #endregion

        #region Ventana Cliente
        private void Limpiar()
        {
            TxtRut.Text = string.Empty;
            TxtNombre.Text = string.Empty;
            TxtApellido.Text = string.Empty;
            //lenado ComboBox
            //CbSexo.ItemsSource = Enum.GetValues(typeof(Sexo));
            Sexo ComboSexo = new Sexo();
            CbSexo.ItemsSource = ComboSexo.ReadAll();
            //CbEstadoCivil.ItemsSource = Enum.GetValues(typeof(EstadoCivil));
            EstadoCivil ComboEstados = new EstadoCivil();
            CbEstadoCivil.ItemsSource = ComboEstados.ReadAll();
            //CbSexo.SelectedValue = Sexo.Femenino;
            CbSexo.SelectedValuePath = "IdSexo";
            CbSexo.DisplayMemberPath = "Descripcion";
            //CbSexo.SelectedIndex = 0;
            CbSexo.SelectedValue = 1;
            //CbEstadoCivil.SelectedValue = EstadoCivil.Soltero;
            CbEstadoCivil.SelectedValuePath = "IdEstadoCivil";
            CbEstadoCivil.DisplayMemberPath = "Descripcion";
            CbEstadoCivil.SelectedValue = 1;
            DpFechaNacimiento.SelectedDate = new DateTime(1990, 1, 1);

            //cargar combo box del filtro en lista de usuarios
            //Sexo
            List<Sexo> ListSexito = new List<Sexo>();
            Sexo sexito = new Sexo();
            ListSexito = ComboSexo.ReadAll();
            sexito.IdSexo = 0;
            sexito.Descripcion = "No Filtrar";
            ListSexito.Add(sexito);
            CbSexoListaCli.ItemsSource = ListSexito;


            CbSexoListaCli.SelectedValuePath = "IdSexo";
            CbSexoListaCli.DisplayMemberPath = "Descripcion";
            CbSexoListaCli.SelectedValue = 0;
            //Estado civil
            List<EstadoCivil> ListEstadito = new List<EstadoCivil>();
            EstadoCivil estadito = new EstadoCivil();
            ListEstadito = ComboEstados.ReadAll();
            estadito.IdEstadoCivil = 0;
            estadito.Descripcion = "No Filtrar";
            ListEstadito.Add(estadito);
            CbEstadoCivilListaCli.ItemsSource = ListEstadito;
            CbEstadoCivilListaCli.SelectedValuePath = "IdEstadoCivil";
            CbEstadoCivilListaCli.DisplayMemberPath = "Descripcion";
            CbEstadoCivilListaCli.SelectedValue = 0;
            MostrarClientes();
        }

        //verifica si cliente existe
        private int Buscar()
        {
            int index = -1;
            int count = 0;
            List<Cliente> clientes = new List<Cliente>();
            Cliente clien = new Cliente();
            clientes = clien.ReadAll();
            foreach (Cliente x in clientes)
            {
                if (x.RutCliente.Equals(TxtRut.Text))
                {
                    index = count;
                    break;
                }
                count++;
            }
            return index;
        }

        //retorna el cliente que buscas
        private Cliente ConsultaCliente()
        {
            Cliente clie = new Cliente();
            
            
                if (!TxtRut.Text.Equals(string.Empty))
                {
                    clie.RutCliente = TxtRut.Text;
                    clie.Read();
                    return clie;
                }
                else
                {
                    throw new ArgumentException("Debe ingresar un rut para consultar!!!");
                }

            
               
            
            

        }

        //cargar datos de cliente clie
        private void CargarCliente(Cliente Cli)
        {
            if (!Cli.RutCliente.Equals(string.Empty))
            {
                TxtRut.Text = Cli.RutCliente;
                TxtNombre.Text = Cli.Nombres;
                TxtApellido.Text = Cli.Apellidos;
                DpFechaNacimiento.SelectedDate = Cli.FechaNacimiento;
                CbSexo.SelectedValue = Cli.IdSexo;
                CbEstadoCivil.SelectedValue = Cli.IdEstadoCivil;
            }
            else
            {
                Limpiar();
            }
        }

        //busca cliente
        private async void BtnConsultarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CargarCliente(ConsultaCliente());
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR!!!",ex.Message,MessageDialogStyle.Affirmative);
            }
        }


        //Agrega cliente
        private async void BtnRegistrarCliente_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            Sexo Sex = new Sexo();
            EstadoCivil Est = new EstadoCivil();

            if (Buscar() == -1)
            {
                try
                {
                    cliente.RutCliente = TxtRut.Text;
                    cliente.Nombres = TxtNombre.Text;
                    cliente.Apellidos = TxtApellido.Text;
                    cliente.FechaNacimiento = (DateTime)DpFechaNacimiento.SelectedDate;
                    Sex.IdSexo = (int)CbSexo.SelectedValue;
                    Sex.Descripcion = CbSexo.Text;
                    cliente.IdSexo = (int)CbSexo.SelectedValue;
                    cliente.Sexo = Sex;
                    Est.IdEstadoCivil = (int)CbEstadoCivil.SelectedValue;
                    Est.Descripcion = CbEstadoCivil.Text;
                    cliente.IdEstadoCivil = (int)CbEstadoCivil.SelectedValue;
                    cliente.EstadoCivil = Est;
                    //agregar cliente
                    cliente.Create();
                    await this.ShowMessageAsync("Registro Exito!", "Se ha registrado exitosamente!! ");
                    //MessageBox.Show("Se ha registrado exitosamente!! ", "Registro Exito!", MessageBoxButton.OK, MessageBoxImage.None);
                }
                catch (ArgumentException ex)
                {
                    await this.ShowMessageAsync("ERROR!!!",ex.Message);
                    //MessageBox.Show(ex.Message + "", "ERROR!!");
                }

            }
            else
            {
                await this.ShowMessageAsync("ERROR!!!", "NO Se ha registrado exitosamente!!\nCliente Ya Existe!!");
                //MessageBox.Show("NO Se ha registrado exitosamente!!\nCliente Ya Existe!!", "Sin Exito!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //actualizar cliente
        private async void BtnActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            Sexo Sex = new Sexo();
            EstadoCivil Est = new EstadoCivil();
            //Commit de prueba sdhbchbcshb
            try
            {
                cliente.RutCliente = TxtRut.Text;
                cliente.Nombres = TxtNombre.Text;
                cliente.Apellidos = TxtApellido.Text;
                cliente.FechaNacimiento = (DateTime)DpFechaNacimiento.SelectedDate;
                Sex.IdSexo = (int)CbSexo.SelectedValue;
                Sex.Descripcion = CbSexo.Text;
                cliente.IdSexo = (int)CbSexo.SelectedValue;
                cliente.Sexo = Sex;
                Est.IdEstadoCivil = (int)CbEstadoCivil.SelectedValue;
                Est.Descripcion = CbEstadoCivil.Text;
                cliente.IdEstadoCivil = (int)CbEstadoCivil.SelectedValue;
                cliente.EstadoCivil = Est;
                cliente.Update();
                await this.ShowMessageAsync("Exito!!!", "cliente rut: " + cliente.RutCliente + " actualizado correctamente!!!");
                //MessageBox.Show("cliente rut: " + cliente.RutCliente + " actualizado correctamente!!!");
            }
            catch (ArgumentException ex)
            {
                await this.ShowMessageAsync("ERROR!!!", "Error: "+ex.Message);
                //MessageBox.Show(ex.Message + "");
            }
        }

        //Eliminar cliente
        private async void BtnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            Cliente cliente = new Cliente();
            try
            {
                cliente.RutCliente = TxtRut.Text;
                if (!cliente.Delete()) throw new ArgumentException("Cliente no Existe!!!");
                await this.ShowMessageAsync("Exito!!!", "Cliente eliminado correctamente!!!");
                //MessageBox.Show("cliente eliminado correctamente!!!");
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR!!!", "cliente no eliminado correctamente!!! \nError: "+ex.Message);
                //MessageBox.Show("cliente no eliminado correctamente!!!");
            }

        }
        #endregion

        #region Listado Clientes
        //Metodo para cargar la lista de Clientes
        private void MostrarClientes()
        {
            Cliente Clie = new Cliente();
            DgClientes.ItemsSource = Clie.ReadAll();
            DgClientes.Items.Refresh();
            //CargaRut();
        }

        //Logica del filtro
        private async void UltimateFilter()
        {
            Cliente FiltroClie = new Cliente();
            try
            {
                if (!TxtRutList.Text.Equals(string.Empty))
                {
                    DgClientes.ItemsSource = FiltroClie.ReadAll().Where(r => r.RutCliente == TxtRutList.Text);
                   
                    if (DgClientes.Items.Count == 0)
                    {
                        await this.ShowMessageAsync("Error","no existe cliente con este rut " + TxtRutList.Text);
                        DgClientes.ItemsSource = FiltroClie.ReadAll();
                    }
                }
                else
                {
                    if ((int)CbSexoListaCli.SelectedIndex > -1 && (int)CbSexoListaCli.SelectedValue != 0 && (int)CbEstadoCivilListaCli.SelectedIndex > -1 && (int)CbEstadoCivilListaCli.SelectedValue != 0)
                    {
                        //si ambos combo box estan alterados se asigna el valor de la id de sexo y estado civil
                        //a nuestro ojeto cliente (FiltroClie)
                        FiltroClie.IdSexo = (int)CbSexoListaCli.SelectedValue;
                        FiltroClie.IdEstadoCivil = (int)CbEstadoCivilListaCli.SelectedValue;
                        /*una vez asignados se procede a usar los elementos ReadAllBySexo y ReadAllByEstadoCivil*/
                        //La siguiente linea no funciona
                        //DgClientes.ItemsSource = FiltroClie.ReadAllBySexo().Intersect(FiltroClie.ReadAllByEstadoCivil() );
                        /* a continuacion el link donde encontramos la solucion para el filtro
                         * http://www.qualityinfosolutions.com/comparar-listas-de-objetos-utilizando-linq-c/ */
                        DgClientes.ItemsSource = (from s in FiltroClie.ReadAllBySexo()
                                                  where (from e in FiltroClie.ReadAllByEstadoCivil()
                                                         select e.RutCliente).Contains(s.RutCliente)
                                                  select s).Distinct().ToList();
                        //intentare un metodo nuevo
                        // lruits1.Where(product => !fruits2Names.Contains(product.Name));
                        //DgClientes.ItemsSource = FiltroClie.ReadAllBySexo().Where(a =>  FiltroClie.ReadAllByEstadoCivil().Contains(a));
                        //DgClientes.ItemsSource = FiltroClie.ReadAllBySexo().Join(FiltroClie.ReadAllByEstadoCivil(), a = > a.RutCliente = RutCliente);
                        

                    }
                    else
                    {
                        if (((int)CbEstadoCivilListaCli.SelectedIndex > -1 && (int)CbEstadoCivilListaCli.SelectedValue != 0) || ((int)CbSexoListaCli.SelectedIndex > -1 && (int)CbSexoListaCli.SelectedValue != 0))
                        {
                            if (((int)CbEstadoCivilListaCli.SelectedIndex > -1 && (int)CbEstadoCivilListaCli.SelectedValue != 0))
                            {

                                FiltroClie.IdEstadoCivil = (int)CbEstadoCivilListaCli.SelectedValue;
                                DgClientes.ItemsSource = FiltroClie.ReadAllByEstadoCivil();
                               
                            }
                            if (((int)CbSexoListaCli.SelectedIndex > -1 && (int)CbSexoListaCli.SelectedValue != 0))
                            {
                                FiltroClie.IdSexo = (int)CbSexoListaCli.SelectedValue;

                                DgClientes.ItemsSource = FiltroClie.ReadAllBySexo();
                                
                            }
                        }
                        else
                        {
                            DgClientes.ItemsSource = FiltroClie.ReadAll();
                            await this.ShowMessageAsync("","sin Filtro");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR","Durante el filtrado se produjo una excepsion: Detalle --> " + ex);
            }
        }

        //cuando seleccione un cliente en La lista se importara a el mantenedor para ser modificado o eliminado
        private void DgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(IndexWin != -1)
            {
                int index = 0;
                index = DgClientes.SelectedIndex;
                if (index != -1)
                {
                    DgClientes.Items[index].ToString();
                    Cliente Clie = new Cliente();
                    Clie = (Cliente)DgClientes.Items[index];
                    this.Limpiar();
                    TxtContratoRut.Text = Clie.RutCliente;
                    TbBeLife.SelectedIndex = IndexWin;
                    //TxtRut.Text = Clie.RutCliente;
                    //TxtNombre.Text = Clie.Nombres;
                    //TxtApellido.Text = Clie.Apellidos;
                    //CbSexo.SelectedValue = Clie.Sexo.IdSexo;
                    //CbEstadoCivil.SelectedValue = Clie.EstadoCivil.IdEstadoCivil;
                    //DpFechaNacimiento.SelectedDate = Clie.FechaNacimiento;
                }
            }
            else
            {
                int index = 0;
                index = DgClientes.SelectedIndex;
                if (index != -1)
                {
                    DgClientes.Items[index].ToString();
                    Cliente Clie = new Cliente();
                    Clie = (Cliente)DgClientes.Items[index];
                    this.Limpiar();
                    CargarCliente(Clie);
                    TbBeLife.SelectedIndex = 1;
                    //TxtRut.Text = Clie.RutCliente;
                    //TxtNombre.Text = Clie.Nombres;
                    //TxtApellido.Text = Clie.Apellidos;
                    //CbSexo.SelectedValue = Clie.Sexo.IdSexo;
                    //CbEstadoCivil.SelectedValue = Clie.EstadoCivil.IdEstadoCivil;
                    //DpFechaNacimiento.SelectedDate = Clie.FechaNacimiento;
                }
            }
        }

        //boton que inicia la accin para filtrar lista cliente
        private void BtnFiltrarClientes_Click(object sender, RoutedEventArgs e)
        {
            UltimateFilter();
        }

        #endregion

        #region Ventana Contrato
        //carga contrato
        private void CargaContrato(Contrato contrato)
        {
            TxtContratoRut.Text = contrato.RutCliente;
            CbContratoPlanes.SelectedValue = contrato.CodigoPlan;
            CargarClienteContrato(ObtenerCliente(TxtContratoRut.Text));
            DpContratoInicio.SelectedDate = contrato.FechaInicioVigencia;
            ChBContratoEstaVigente.IsChecked = contrato.Vigente;
            ChBContratoSalud.IsChecked = contrato.DeclaracionSalud;
            CargarResumenPlan();
            TxtContratoObserva.Text = contrato.Observaciones;
            TxtNumeroContrato.Text = contrato.Numero;
        }

        //carga combobox contrato
        private void CargaCbContrato()
        {
            Plan plan = new Plan();
            CbContratoPlanes.ItemsSource = plan.ReadAll();
            CbContratoPlanes.SelectedValuePath = "IdPlan";
            CbContratoPlanes.DisplayMemberPath = "Nombre";
            CbContratoPlanes.SelectedIndex = 0;
            CbContratoPlanes.Items.Refresh();

            List<Plan> ListPlancito = new List<Plan>();
            Plan PlanPoli = new Plan();
            ListPlancito = plan.ReadAll();
            PlanPoli.IdPlan = "0";
            PlanPoli.PolizaActual = "No Filtrar";
            ListPlancito.Add(PlanPoli);
            CbPlanPoliza.ItemsSource = ListPlancito;
            CbPlanPoliza.SelectedValuePath = "IdPlan";
            CbPlanPoliza.DisplayMemberPath = "PolizaActual";
            CbPlanPoliza.SelectedValue = "0";

        }

        //metodo para cargar datos de cliente en contrato
        private void CargarClienteContrato(Cliente clie)
        {
            TxtContratoNombre.Text = clie.Nombres;
            TxtContratoApellido.Text = clie.Apellidos;
        }

        //Obtener Cliente
        private Cliente ObtenerCliente(string rut)
        {
            Cliente cliente = new Cliente();
            if (!rut.Equals(string.Empty))
            {
                cliente.RutCliente = rut;
                if (cliente.Read())
                {
                    return cliente;
                }
                else
                {
                    throw new ArgumentException("El cliente que ingreso no es valido, ¡¡¡No existe!!!");
                }
            }
            return cliente;
        }

        //Obtener Prima
        private double ObtenerPrima()
        {
            Tarificador tarificador;
            double PrimaBase;
            double Primaanual=-1;
            Plan plan = new Plan();
            int i=0;
            try
            {
                i = (int)CbContraTipoContra.SelectedValue;
                try
                {
                    if (CbContratoPlanes.SelectedIndex != -1)
                    {
                        plan.IdPlan = CbContratoPlanes.SelectedValue.ToString();
                        plan.Read();
                        PrimaBase = plan.PrimaBase;

                        LbContratoTipoPlan.Content = plan.Nombre;
                        LbPoliza.Content = plan.PolizaActual;
                    }
                    else
                    {
                        throw new ArgumentException("Tiene que seleccionar un Plan antes de calcular la Prima!!!");
                    }
                    switch (i)
                    {
                        case 10:
                            tarificador = new VidaTarificador();
                            if(tarificador is VidaTarificador)
                            {
                                VidaTarificador vidaTarificador = (VidaTarificador)tarificador;
                                vidaTarificador.cliente = this.ObtenerCliente(TxtContratoRut.Text);
                                Primaanual = vidaTarificador.CalcularPrimaBase(PrimaBase);
                            }
                            break;
                        case 20:
                            tarificador = new VehiculoTarificador();
                            if(tarificador is VehiculoTarificador)
                            {
                                VehiculoTarificador vehiculoTarificador = (VehiculoTarificador)tarificador;
                                vehiculoTarificador.Cliente = this.ObtenerCliente(TxtContratoRut.Text);
                                vehiculoTarificador.Vehiculo = this.ObtenerVehiculo();
                                Primaanual = vehiculoTarificador.CalcularPrimaBase(PrimaBase);
                            }
                            break;
                        case 30:
                            tarificador = new ViviendaTarificador();
                            if (tarificador is ViviendaTarificador)
                            {
                                ViviendaTarificador viviendaTarificador = (ViviendaTarificador)tarificador;
                                viviendaTarificador.Vivienda = this.ObtenerVivienda();
                                Primaanual = viviendaTarificador.CalcularPrimaBase(PrimaBase);
                            }
                            break;

                    }

                }
                catch (Exception ex)
                {

                }
            }catch(Exception ex)
            {
                throw new ArgumentException("tiene que seleccionar un tipo de contrato para calcular prima");
            }

            return Primaanual;
        }

        //Cargar Rssumen
        private void CargarResumenPlan()
        {
            double PrimaAnual = 0.0d, PrimaMensual = 0.0d;
            PrimaAnual = ObtenerPrima(ObtenerCliente(TxtContratoRut.Text));
            PrimaMensual = (double)((Math.Truncate(PrimaAnual * 10.0)) / 10.0) / 12;
            LbPrimaAnual.Content = PrimaAnual.ToString();
            LbPrimaMensual.Content = PrimaMensual.ToString();
        }

        //metodo para validar los datos del contrato actual retorna un objeto cliente
        private Contrato ObtenerContrato()
        {
            Contrato contrato = new Contrato();
            contrato.RutCliente = TxtContratoRut.Text;
            contrato.IdTipoContrato = (int)CbContraTipoContra.SelectedValue;
            contrato.CodigoPlan = CbContratoPlanes.SelectedValue.ToString();
            contrato.DeclaracionSalud = ChBContratoSalud.IsChecked.Value;
            contrato.FechaInicioVigencia = DpContratoInicio.SelectedDate.Value;
            contrato.DeclaracionSalud = ChBContratoSalud.IsChecked.Value;
            contrato.Observaciones = TxtContratoObserva.Text;
            return contrato;
        }

        //buscar contrato numero
        private Contrato BuscarContratoNr(string numero)
        {
            Contrato contrato = new Contrato();
            if (!TxtNumeroContrato.Equals(string.Empty))
            {
                contrato.Numero = TxtNumeroContrato.Text;
                if (contrato.Read())
                {
                    return contrato;
                }
                else
                {
                    throw new ArgumentException("El numero de contrato no coincide!!!, no existe!");
                }
            }
            else
            {
                throw new ArgumentException("No puede estar vacio el numero de contrato!!, al buscar!");
            }
        }

        //boton para buscar contrato
        private async void BtnBuscarContrato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CargaContrato(BuscarContratoNr(TxtNumeroContrato.Text));
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR", "Error: " + ex.Message);
            }
        }

        //boton para abrir la ventana lista de contratos
       

        //busca al cliente rut 
        private async void TxtContratoRut_TextChanged(object sender, TextChangedEventArgs e)
        {
            Cliente clie = new Cliente();
            try
            {
                if (!TxtContratoRut.Text.Equals(string.Empty))
                {
                    clie.RutCliente = TxtContratoRut.Text; 
                    if (clie.Read())
                    {
                        CargarClienteContrato(clie);
                    }
                    else
                    {
                        if (TxtContratoRut.Text.Length == 10)
                        {
                            throw new ArgumentException("El cliente Rut: " + TxtContratoRut.Text + " no esta registrado!!!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("Info","Error: "+ex.Message);
            }
        }

       

        private void CbContratoPlanes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //boton para agregar contrato
        private async void BtnAgregarContrato_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato;
            try
            {
                contrato = ObtenerContrato();
                switch ((int)CbContraTipoContra.SelectedValue)
                {
                    case 10:
                        contrato = new Contrato();
                        break;
                    case 20:
                        contrato = new ContratoVehiculo();
                        break;
                    case 30:
                        contrato = new ContratoVivienda();
                        break;
                }
                contrato = ObtenerContrato();

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR!","Error: "+ ex.Message);
            }
        }

        //boton para actualizar contrato
        private async void BtnActualizarContrato_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato = new Contrato();
            try
            {


                if (contrato.Sincroniza(ObtenerContrato()))
                {
                    contrato.Numero = TxtNumeroContrato.Text;
                    if (contrato.Update())
                    {
                        TxtNumeroContrato.Text = contrato.Numero;
                        await this.ShowMessageAsync("Correcto!", "Contraro Numero: " + contrato.Numero + " actualizado correctamente!!");
                    }
                    else
                    {
                        await this.ShowMessageAsync("ERROR!", "Contraro Numero: " + contrato.Numero + " no pudo ser actualizado!!\nVerifique que esté vigente!!");
                    }
                }
                else
                {
                    await this.ShowMessageAsync("ERROR!", "Contraro Numero: " + contrato.Numero + " no sincronizado!!");
                }

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR!", "Error: " + ex.Message);
            }
        }

        //boton para terminar contrato
        private async void BtnEliminarContrato_Click(object sender, RoutedEventArgs e)
        {
            Contrato contrato = new Contrato();
            try
            {
                contrato = BuscarContratoNr(TxtNumeroContrato.Text);
                contrato.FechaFinVigencia = DateTime.Today;
                if (contrato.Update())
                {
                    await this.ShowMessageAsync("Terminado","Contrato numero: "+contrato.Numero+" a sido terminado");
                }
                else
                {
                    await this.ShowMessageAsync("No Terminado", "Contrato numero: " + contrato.Numero + " no se pudo terminar el contrato.\nPuede que el contrato ya haya sido terminado o no exista!!");
                }
                CargaContrato(contrato);
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR!", "Error: " + ex.Message);
            }
        }

        private void ChBContratoSalud_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ChBContratoEstaVigente_Checked(object sender, RoutedEventArgs e)
        {

        }

        //click calcular resumen contrato
        private async void BtnCalcularResumeContrato_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                CargarResumenPlan();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("Error!","Error: "+ex.Message);
            }
            
        }

        //cargar combo tipo contrato
        private void CargaConTipoContrato()
        {
            TipoContrato tipoContrato = new TipoContrato();
            CbContraTipoContra.ItemsSource = tipoContrato.ReadAll();
            CbContraTipoContra.SelectedValuePath = "IdTipoContrato";
            CbContraTipoContra.DisplayMemberPath = "Descripcion";
            CbContraTipoContra.Items.Refresh();
        }

        //cambio combo tipo contrato
        private void CbContraTipoContra_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TipoContrato tipo = new TipoContrato();
            
            try
            {
                if (CbContraTipoContra.SelectedIndex != -1)
                {
                    
                    tipo.IdTipoContrato = (int)CbContraTipoContra.SelectedValue;
                    Plan plan = new Plan();
                    plan.IdTipoContrato = tipo.IdTipoContrato;
                    CbContratoPlanes.ItemsSource = plan.ReadAllByTipCon();
                    CbContratoPlanes.SelectedValuePath = "IdPlan";
                    CbContratoPlanes.DisplayMemberPath = "Nombre";
                    CbContratoPlanes.SelectedIndex = 0;
                    
                }
            }catch(Exception ex)
            {
                
            }
            
        }
        #endregion

        #region Lista Contratos
        //carga del listado de contratos
        private void CargarListaContratos()
        {
            Contrato contrato = new Contrato();
            DgContratos.ItemsSource = contrato.ReadAll();
            DgContratos.Items.Refresh();
        }

        //llevar contrato seleccionado al mantenedor de contratos
        private void DgContratos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = 0;
            index = DgContratos.SelectedIndex;
            if (index != -1)
            {
                DgContratos.Items[index].ToString();
                Contrato contrato = new Contrato();
                contrato = (Contrato)DgContratos.Items[index];
                this.Limpiar();
                CargaContrato(contrato);
                TbBeLife.SelectedIndex = 3;
                //TxtRut.Text = Clie.RutCliente;
                //TxtNombre.Text = Clie.Nombres;
                //TxtApellido.Text = Clie.Apellidos;
                //CbSexo.SelectedValue = Clie.Sexo.IdSexo;
                //CbEstadoCivil.SelectedValue = Clie.EstadoCivil.IdEstadoCivil;
                //DpFechaNacimiento.SelectedDate = Clie.FechaNacimiento;
            }
        }

        #endregion

        private void ListClientesWin_Click(object sender, RoutedEventArgs e)
        {
            if(TbBeLife.SelectedIndex == 3)
            {
                IndexWin = 3;
            }
            else
            {
                IndexWin = -1;
            }
            modulo = "Lista Clientes";
            TbBeLife.SelectedIndex = 2;
            this.Title = empresa+modulo;
        }

        private void ManClienteWin_Click(object sender, RoutedEventArgs e)
        {
            modulo = "Mantenedor Cliente";
            TbBeLife.SelectedIndex = 1;
            this.Title = empresa + modulo;
        }

        private void LisContratoWin_Click(object sender, RoutedEventArgs e)
        {
            modulo = "Lista Contratos";
            TbBeLife.SelectedIndex = 4;
            CargarListaContratos();
            this.Title = empresa + modulo;
        }

        private void ManContratoWin_Click(object sender, RoutedEventArgs e)
        {
            modulo = "Mantenedor Contrato";
            TbBeLife.SelectedIndex = 3;
            this.Title = empresa + modulo;
        }

        //Logica del filtro
        private async void UltimateFilterContrato()
        {
            Contrato FiltroContra = new Contrato();
            try
            {
                if (!TxtRutListCon.Text.Equals(string.Empty))
                {
                    DgContratos.ItemsSource = FiltroContra.ReadAll().Where(r => r.RutCliente == TxtRutList.Text);

                    if (DgClientes.Items.Count == 0)
                    {
                        await this.ShowMessageAsync("Error", "no existe contratos asociados con este rut " + TxtRutList.Text);
                        DgClientes.ItemsSource = FiltroContra.ReadAll();
                    }
                }
                else
                {
                    if (!TxtNContratoList.Text.Equals(string.Empty))
                    {
                        DgContratos.ItemsSource = FiltroContra.ReadAll().Where(r => r.Numero == TxtNContratoList.Text);

                        if (DgClientes.Items.Count == 0)
                        {
                            await this.ShowMessageAsync("Error", "no existe contratos asociados con este numero " + TxtNContratoList.Text);
                            DgClientes.ItemsSource = FiltroContra.ReadAll();
                        }

                    }
                    else
                    {
                        if (CbPlanPoliza.SelectedIndex > -1 && CbPlanPoliza.SelectedValue.ToString() !="0")
                        {
                            DgContratos.ItemsSource = FiltroContra.ReadAll().Where(r => r.CodigoPlan == CbPlanPoliza.SelectedValue.ToString());

                            if (DgClientes.Items.Count == 0)
                            {
                                await this.ShowMessageAsync("Error", "no existe contratos asociados con esa poliza " );
                                DgClientes.ItemsSource = FiltroContra.ReadAll();
                            }
                        }
                        else
                        {
                            DgContratos.ItemsSource = FiltroContra.ReadAll();
                            await this.ShowMessageAsync("", "sin Filtro");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR", "Durante el filtrado se produjo una excepsion: Detalle --> " + ex);
            }
        }


        private void BtnFiltrarContratos_Click(object sender, RoutedEventArgs e)
        {
            UltimateFilterContrato();
        }

        private void TbBeLife_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IndexWin = TbBeLife.SelectedIndex;
            if(IndexWin == 0)
            {
                modulo = "Home";
                this.Title = empresa + modulo;
            }
        }

        
    }
}
