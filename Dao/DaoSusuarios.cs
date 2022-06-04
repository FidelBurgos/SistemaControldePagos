﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Dao
{
    public class DaoSusuarios
    {

        private AccesoDatos accesoDatos = new AccesoDatos();


        public DaoSusuarios(){; }

        public Boolean verificarUseryPass(Susuarios usuario)
        {
            DataTable tabla = new DataTable();
            tabla = obtenerTodosLosUsuarios();
            foreach(DataRow fila in tabla.Rows)
            {

                
                if (usuario.User == fila["usua_user"].ToString() && usuario.Clave == fila["usua_clave"].ToString() && 0== Int32.Parse(fila["usua_baja"].ToString()))
                {
                    return true;
                }
                
                /*
                /// EL USUARIO EXISTE Y ESTA DE ALTA

                if (usuario.User == fila["usua_user"].ToString() && usuario.Clave == fila["usua_clave"].ToString() && 0 == Int32.Parse(fila["usua_baja"].ToString()))
                {
                   
                    return 2;
                }
                /// EL USIUARIO EXISTE PERO ESTA DE BAJA
                else if (usuario.User == fila["usua_user"].ToString() && usuario.Clave == fila["usua_clave"].ToString() && 1 == Int32.Parse(fila["usua_baja"].ToString()))
                {
                    return 3;
                }
                /// EL USUARIO EXISTE PERO NO COINCIDEN USER NI PASS
                else if (usuario.User != fila["usua_user"].ToString() || usuario.Clave != fila["usua_clave"].ToString() && 0 == Int32.Parse(fila["usua_baja"].ToString()))
                {
                    return 4;
                }
                else { return 5; }
                */
            }
            return false;
            /// EL USUARIO NO EXISTE NI COINCIDE 
        }

        public DataTable obtenerTodosLosUsuarios()
        {
            String consulta = "SELECT * FROM susuarios";
            return accesoDatos.ObtenerTabla("susuarios", consulta);
        }

        /*
        public Boolean verificarUseryPassBaja(Susuarios usuario)
        {
            DataTable tabla = new DataTable();
            tabla = obtenerTodosLosUsuarios();
            foreach (DataRow fila in tabla.Rows)
            {
                if (usuario.User == fila["usua_user"].ToString() && usuario.Clave == fila["usua_clave"].ToString() && 0 == Int32.Parse(fila["usua_baja"].ToString()))
                    {
                    return true;
                    }
                else
                {

                }
            }
        }
        */

    }
}