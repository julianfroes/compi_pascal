using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compi
{
    public class NodoPolish
    {
        private string direccionbrinco;
        private string brinco;
        private string lexema;

        public string Lexema
        {
            get
            {
                return lexema;
            }

            set
            {
                lexema = value;
            }
        }
        public string Direccionbrinco
        {
            get
            {
                return direccionbrinco;
            }

            set
            {
                direccionbrinco = value;
            }
        }
        public string Brinco
        {
            get
            {
                return brinco;
            }

            set
            {
                brinco = value;
            }
        }
    }
}
