using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Entidades;

namespace Vistas
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciarSesion(object sender, EventArgs e)
        {
            NegocioSusuarios negusuario = new NegocioSusuarios();
            Susuarios usuario = new Susuarios(txtUser.Text.Trim(), txtPass.Text.Trim());
             //int validacion=negusuario.verificarUseryPass(usuario);
            
            if (negusuario.verificarUseryPass(usuario))
            {
                if (Session["usuario"] == null)
                {
                    Session["usuario"] = usuario;
                }
                Response.Redirect("Inicio.aspx");
            }
            /*
            
            if (validacion==2)
            {
                Response.Redirect("Inicio.aspx");

            }
            else if(validacion==3)
            {
                Console.WriteLine("Console text");
            }
            else if (validacion == 4)
            {
                Console.WriteLine("Console text");
            }
            */
        }
    }
}