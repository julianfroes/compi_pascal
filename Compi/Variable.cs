using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi
{
    
    public class Variable
    {
        private string nombre;
        private int tipoVariable;

        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public int TipoVariable
        {
            get
            {
                return tipoVariable;
            }

            set
            {
                tipoVariable = value;
            }
        }

    }
}
