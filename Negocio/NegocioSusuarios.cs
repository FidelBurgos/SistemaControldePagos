using Dao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NegocioSusuarios
    {

        DaoSusuarios dao_usu = new DaoSusuarios();
        public Boolean verificarUseryPass(Susuarios user)
        {
            return dao_usu.verificarUseryPass(user);
        }

       public String obtenerIdUsuario(Susuarios usuario)
        {
            return dao_usu.obtenerIdUsuario(usuario);
        }

    }
}
