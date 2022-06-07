using Dao;
using Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Negocio
{
    public class NegocioCarrerasCurso
    {

        private DaoCarrerasCurso dao = new DaoCarrerasCurso();
        private List <string> Carreras_seleccionadas = new List<string>();
        private List <int> Meses_seleccionados = new List<int>();
        private List <int> Anios_seleccionados = new List<int>();

        string TipoPago_seleccionado = "";

        public NegocioCarrerasCurso() {;}

        public void cargarDatosCarrerasCBL(string idusuario,CheckBoxList checkBoxList, string TipoConsulta = "Todo", string TextoBusqueda = "")
        {
            
            checkBoxList.DataSource = dao.listarCarreras(TipoConsulta, TextoBusqueda, idusuario);
            checkBoxList.DataTextField = "cacu_descripcion";
            checkBoxList.DataValueField = "cacu_idcarrcurs";
            checkBoxList.DataBind();
        }

        public void cargarDatosMesesCBL(CheckBoxList checkBoxList)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Numero");
            dt.Columns.Add("Mes");
            dt.Rows.Add("0", "Todos");
            dt.Rows.Add("1", "Enero");
            dt.Rows.Add("2", "Febrero");
            dt.Rows.Add("3", "Marzo");
            dt.Rows.Add("4", "Abril");
            dt.Rows.Add("5", "Mayo");
            dt.Rows.Add("6", "Junio");
            dt.Rows.Add("7", "Julio");
            dt.Rows.Add("8", "Agosto");
            dt.Rows.Add("9", "Septiembre");
            dt.Rows.Add("10", "Octubre");
            dt.Rows.Add("11", "Noviembre");
            dt.Rows.Add("12", "Diciembre");
            checkBoxList.DataSource = dt;
            checkBoxList.DataTextField = "Mes";
            checkBoxList.DataValueField = "Numero";

            checkBoxList.SelectedIndex = 0;

            checkBoxList.DataBind();
         
        }

        public void cargarDatosAñosCBL(CheckBoxList checkBoxList)
        {
            checkBoxList.DataSource = dao.listarFechasCarreras();
            checkBoxList.DataTextField = "Fecha";
            checkBoxList.DataValueField = "IdFecha";

            checkBoxList.SelectedIndex = 0;

            checkBoxList.DataBind();
        }

        public List<DataTable> ControlarBusqueda(CheckBoxList cbl_carrera, CheckBoxList cbl_anio, CheckBoxList cbl_mes, RadioButtonList rbl_tipoPago)
        {
            foreach (ListItem item in cbl_carrera.Items)
            {
                if (item.Selected == true)
                {
                    //Alguna accion al encontrar un check seleccionado

                    Carreras_seleccionadas.Add(item.Value.ToString());
                }
            }
            foreach (ListItem item in cbl_anio.Items)
            {
                if (item.Selected == true)
                {
                    //Alguna accion al encontrar un check seleccionado
                    Anios_seleccionados.Add(Int32.Parse(item.Text));

                }
            }
            foreach (ListItem item in cbl_mes.Items)
            {
                if (item.Selected == true)
                {
                    //Alguna accion al encontrar un check seleccionado
                    
                    if (Int32.Parse(cbl_mes.SelectedItem.Value) == 0)
                    {
                        for (int x = 0; x <= 12; x++) { Meses_seleccionados.Add(x); }
                        break;

                    }
                    else {
                        Meses_seleccionados.Add(Int32.Parse(item.Value));
                    }                  
                }
            }


            if (rbl_tipoPago.SelectedItem != null)
            {
                TipoPago_seleccionado = rbl_tipoPago.SelectedItem.Text;
            }

            else return null;

            if ((!Carreras_seleccionadas.Any()) || (!Anios_seleccionados.Any()) || (!Meses_seleccionados.Any()) )
            {
                return null;
            }
         
            return crearTabla();
        }
        private List<DataTable> crearTabla()
        {
            List<DataTable> tablasCarrerasCursos = new List<DataTable>();
            List<DataTable> tablasCuotas = new List<DataTable>();
            List <DataTable> tablasMatriculas = new List<DataTable>();
            
            List<DataTable> tablasControlPagos = new List<DataTable>();


            // Se busca la cantidad de alumnos en la carrera/curso en un determinado mes y año
            cargarTablaCantidadAlumnos(tablasCarrerasCursos);

            // Metodo que une las tablas de las consultas
            unirTablas(tablasCarrerasCursos, tablasCuotas, tablasMatriculas, tablasControlPagos);



            return tablasControlPagos;
        }


        private void unirTablas(List<DataTable> tablasCarrerasCursos, List<DataTable> tablasCuotas, 
                                List<DataTable> tablasMatriculas, List<DataTable> tablasControlPagos)
        {
           
            // un valor auxiliar para saber la posicion de la tabla cuota/matricula seleccionada
            int valorAuxiliar = 0;
            
            foreach (DataTable registro in tablasCarrerasCursos)
            {
                DataTable controlDePagos = new DataTable();
                controlDePagos = registro.Copy();

                if (TipoPago_seleccionado == "AMBAS")
                 {
                     // Se busca la cantidad de cuotas en un determinado mes y año
                     cargarTablaCantidadCuotas(tablasCuotas);

                     // Se busca la cantidad de matriculas en un determinado mes y año
                     cargarTablaCantidadMatriculas(tablasMatriculas);

                     // Agrego en el data table las matriculas y cuotas
                     agregarCuotasTablaDeControl(controlDePagos, tablasCuotas[valorAuxiliar]);
                     agregarMatriculasTablaDeControl(controlDePagos, tablasMatriculas[valorAuxiliar]);

                 }
                 else if (TipoPago_seleccionado == "CUOTAS")
                 {
                     // Se busca la cantidad de cuotas en un determinado mes y año
                     cargarTablaCantidadCuotas(tablasCuotas);

                     // Agrego en el data table las cuotas
                     agregarCuotasTablaDeControl(controlDePagos, tablasCuotas[valorAuxiliar]);
                }
                 else if (TipoPago_seleccionado == "MATRICULAS")
                 {
                     // Se busca la cantidad de matriculas en un determinado mes y año
                     cargarTablaCantidadMatriculas(tablasMatriculas);

                     // Agrego en el data table las matriculas
                     agregarMatriculasTablaDeControl(controlDePagos, tablasMatriculas[valorAuxiliar]);
                 }

                tablasControlPagos.Add(controlDePagos);
                valorAuxiliar++;
            }
        }

        private void agregarCuotasTablaDeControl(DataTable controlDePagos, DataTable tablaCuotas)
        {

            controlDePagos.Columns.Add("Cuotas - Pago");
            controlDePagos.Columns.Add("Cuotas - Impago");
            controlDePagos.Columns.Add("Cuotas - Parcial");

            foreach (DataRow row in controlDePagos.Rows)
            {


                row["Cuotas - Pago"] = tablaCuotas.Rows[0]["pago_cuotas"];
                row["Cuotas - Impago"] = tablaCuotas.Rows[0]["impago_cuotas"];
                row["Cuotas - Parcial"] = tablaCuotas.Rows[0]["parcial_cuotas"];
            }
        }

        private void agregarMatriculasTablaDeControl(DataTable controlDePagos, DataTable tablaMatriculas)
        {
            controlDePagos.Columns.Add("Matriculas - Pago");
            controlDePagos.Columns.Add("Matriculas - Impago");
            controlDePagos.Columns.Add("Matriculas - Parcial");

            foreach (DataRow row in controlDePagos.Rows)
            {
                row["Matriculas - Pago"] = tablaMatriculas.Rows[0]["pago_matriculas"];
                row["Matriculas - Impago"] = tablaMatriculas.Rows[0]["impago_matriculas"];
                row["Matriculas - Parcial"] = tablaMatriculas.Rows[0]["parcial_matriculas"];
            }
        }

        private void cargarTablaCantidadAlumnos(List<DataTable> tablasCarrerasCursos)
        {
            
            if (Meses_seleccionados.Count() == 0)//SI no hay meses seleccionados chau
            { return; }
            //Si se selecciono TODOS
            else if (Meses_seleccionados.Contains(0)) { 
                foreach (int año in Anios_seleccionados)//por cada anio seleccionado
                { 
                    try
                    {
                    
                         foreach (string carrera in Carreras_seleccionadas)
                         {
                                DataTable resultado = new DataTable();

                                resultado = dao.ObtenerAlumnosCarrCursoxAnio(año, carrera);
                                if (resultado != null)
                                {
                                    //!! 
                                    resultado.Columns.Add("ANIO");
                                    resultado.Columns.Add("MES");

                                    foreach (DataRow drs in resultado.Rows) // Por cada row en la datatable que me devolvieron
                                    {

                                        if (drs[0].ToString() != null) // si la columna no ta vacia
                                        {
                                            drs[2] = año.ToString(); //Anio pedido
                                            drs[3] = "TODOS"; // y meses seleccionados
                                        }
                                     
                                    }


                                    //dr = resultado.NewRow();
                                    //dr["ANIO"] = año.ToString();
                                    //dr["MES"] = "TODOS";
                                    //resultado.Rows.Add(dr);

                                    tablasCarrerasCursos.Add(resultado);
                                }
                         }
                        
                    
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Debug.WriteLine(exc.Message);
                    }
                }
                return;
            }
            foreach (int año in Anios_seleccionados) { 

                foreach (int mes in Meses_seleccionados)
                {
                    foreach (string carrera in Carreras_seleccionadas)
                    {
                        try
                        {
                            DataTable resultado = new DataTable();


                            resultado = dao.ObtenerAlumnosCarrCursoxFecha(mes, año, carrera);

                            if(resultado != null) 
                                { 
                                    resultado.Columns.Add("ANIO");
                                    resultado.Columns.Add("MES");
                                    foreach (DataRow drs in resultado.Rows) // Por cada row en la datatable que me devolvieron
                                    {

                                        if (drs[0].ToString() != null) // si la columna no ta vacia
                                        {
                                            drs[2] = año.ToString(); //Anio pedido
                                            drs[3] = mes.ToString(); // y meses seleccionados
                                        }

                                    }

                                tablasCarrerasCursos.Add(resultado);
                                }   
                        
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }

            }
        }

        private void cargarTablaCantidadCuotas(List<DataTable> tablasCuotas)
        {
            foreach (int año in Anios_seleccionados)
            {
                foreach (int mes in Meses_seleccionados)
                {
                    foreach (string carrera in Carreras_seleccionadas)
                    {
                        try
                        {
                            DataTable resultado = new DataTable();


                            if (mes == 0)
                            {

                                resultado = dao.ObtenerCuotasCarrCursoxAnio(año, carrera);
                            }
                            else
                            {

                                resultado = dao.ObtenerCuotasCarrCursoxFecha(mes, año, carrera);
                            }


                            if (resultado != null)
                            {
                                tablasCuotas.Add(resultado);
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private void cargarTablaCantidadMatriculas(List<DataTable> tablasMatriculas)
        {
            foreach (int año in Anios_seleccionados)
            {
                foreach (int mes in Meses_seleccionados)
                {
                    foreach (string carrera in Carreras_seleccionadas)
                    {
                        try
                        {
                            DataTable resultado = new DataTable();

                            if (mes == 0)
                            {
                                resultado = dao.ObtenerMatriculasCarrCursoxAnio(año, carrera);
                            }

                            else
                            {

                                resultado = dao.ObtenerMatriculasCarrCursoxFecha(mes, año, carrera);
                            }


                            if (resultado != null) tablasMatriculas.Add(resultado);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }


    }
}
