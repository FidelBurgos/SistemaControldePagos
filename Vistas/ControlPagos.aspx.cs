using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;
namespace Vistas
{
    public partial class ControlPagos : System.Web.UI.Page
    {
        NegocioCarrerasCurso negocioCarrerasCurso = new NegocioCarrerasCurso();
        List<DataTable> tablasCarrerasCurso = new List<DataTable>();
        DataTable dtPruebas = new DataTable();
        DataView view;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idusuario = "";
            if (Session["IdUsuario"] != null)
            {
                idusuario=Session["IdUsuario"].ToString();
            }

            if (!IsPostBack)
            {
                negocioCarrerasCurso.cargarDatosCarrerasCBL(idusuario,cblCarrera);
                negocioCarrerasCurso.cargarDatosMesesCBL(cblMes);
                negocioCarrerasCurso.cargarDatosAñosCBL(cblAnio);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            String advertencia;

            if (!verificarCBLMarcado(cblCarrera))
            {
                advertencia = "Debe marcar todos los datos";
                lblAdvertencia.Text = advertencia;
            }
            else
            {
                try
                {

                    tablasCarrerasCurso = negocioCarrerasCurso.ControlarBusqueda(cblCarrera, cblAnio, cblMes, rblTipoPago);

                    //Si devuelve algún null le pide que ingrese bien los datos
                    if (tablasCarrerasCurso != null)
                    {

                        lblAdvertencia.Text = "";
                        foreach (DataTable tablaCarreraCurso in tablasCarrerasCurso)
                        {

                            try
                            {
                                dtPruebas.Merge(tablaCarreraCurso);
                                dtPruebas.AcceptChanges();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine(ex);
                            }
                        }
                        view = new DataView(dtPruebas);
                        grdBuscado.DataSource = view;
                        grdBuscado.DataBind();
                        grdBuscado.CssClass = "tablaResultados";


                    }
                    else
                    {
                        advertencia = "No se encontraron registros para la(s) consulta(s) de:\n";

                        String aniosSeleccionados = String.Join(", ", cblAnio.Items.Cast<ListItem>().Where(i => i.Selected));
                        String mesesSeleccionados = String.Join(", ", cblMes.Items.Cast<ListItem>().Where(i => i.Selected));

                        foreach (ListItem item in cblCarrera.Items)
                        {
                            if (item.Selected)
                            {
                                advertencia += $"Carrera/Curso: {item.Text}, Año(s): {aniosSeleccionados}, Mes(es): {mesesSeleccionados}\n";
                            }
                        }


                        lblAdvertencia.Text = advertencia;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
           
            HttpResponse response = Response;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            HtmlForm form = new HtmlForm();


            try
            {
                if (grdBuscado.Rows.Count > 0)
                {
                    response.Clear();
                    response.AddHeader("Content-Disposition", "attachment; filename= ControlPagos" + ".xls");
                    response.ContentType = "application/vnd.ms";
                    grdBuscado.RenderControl(htw);
                    response.Write(sw.ToString());
                    response.End();
                }
                else {
                    lblAdvertencia.Text = "Debe haber cursos para generar el excel!";
                }
            }
            catch (Exception ex)
            {
                lblAdvertencia.Text = "No pudo generarse correctamente el excel";
            }


        }

        protected void rblistSeleccionCarrCurso_SelectedIndexChanged(object sender, EventArgs e)
        {
            string busqueda = tbxBusquedaTexto.Text.Trim().ToUpper();
            string idusuario = "";
            if (Session["IdUsuario"] != null)
            {
                idusuario = Session["IdUsuario"].ToString();
            }
            negocioCarrerasCurso.cargarDatosCarrerasCBL(idusuario,cblCarrera, rblistSeleccionCarrCurso.SelectedValue, busqueda);
        }

        protected void tbxBusquedaTexto_TextChanged(object sender, EventArgs e)
        {
            string busqueda = tbxBusquedaTexto.Text.Trim().ToUpper();
            string idusuario = "";
            if (Session["IdUsuario"] != null)
            {
                idusuario = Session["IdUsuario"].ToString();
            }
            negocioCarrerasCurso.cargarDatosCarrerasCBL(idusuario,cblCarrera, rblistSeleccionCarrCurso.SelectedValue, busqueda);
        }

        protected bool verificarCBLMarcado(CheckBoxList cbl)
        {

            foreach(ListItem li in cbl.Items)
            {
                if (li.Selected) return true;
            }

            return false;
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
           
        }

    }
}