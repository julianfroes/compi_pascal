using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Compi
{

    class Sintatico
    {
        public int variableTipo = 0;
        public string nombreVariable;


        int p = 0,iteradorIF,iteradorWhile,iteradorVtemporales;
        bool errorsintactico = false,saltopendienteW=false, saltopendienteIF = false,direccionpendiente=false;
        string direccionAux, lexemaoperador;
        public string textoASMFinal,textoensamblador, titulo,variablesAsm;
        public Stack<string> variablesPolish;
        public Stack<string> variablesTemporalesPolish;
        NodoPolish tokenAux = new NodoPolish();
        public List<Token> listaToken;
        public List<NodoPolish> listaPolish;
        public List<Error> listaError;
        public List<Variable> listaVar;
        public List<Variable> listAuxVar;
        public List<int> listaPostfix;
        public List<int> listaAuxPostfix;




        public Sintatico(List<Error> errorlis, List<Token> tokenlis)
        {
            listaToken = tokenlis;  // inicializar
            listaError = errorlis;  // inicializar
            textoASMFinal = "";

            listaVar = new List<Variable>();
            listAuxVar = new List<Variable>();
            listaPostfix = new List<int>();
            listaAuxPostfix = new List<int>();
            listaPolish = new List<NodoPolish>();
            variablesPolish = new Stack<string>();
            variablesTemporalesPolish = new Stack<string>();


            Stack stack = new Stack();

        } //constructor
        public void incrementarpunteroSin()
        {
            if (!errorsintactico) { p++; }
        }

        public int[,] sumaM = { 
         //      int  || real || string ||  bool
            {    203  ,   204  ,   500  ,   500,},
            {    204  ,   204  ,   500  ,   500,},
            {    500  ,   500  ,   202  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] restaM = { 
         //      int  || real || string ||  bool
            {    203  ,   204  ,   500  ,   500,},
            {    204  ,   204  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] multiplicacionM = { 
         //      int  || real || string ||  bool
            {    203  ,   204  ,   500  ,   500,},
            {    204  ,   204  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] divisionM = { 
         //      int  || real || string ||  bool
            {    204  ,   204  ,   500  ,   500,},
            {    204  ,   204  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] asignacionM = { 
         //      int  || real || string ||  bool
            {    203  ,   204  ,   500  ,   500,},
            {    204  ,   204  ,   500  ,   500,},
            {    500  ,   500  ,   202  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] diferenteM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   205  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] mayorQueM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] menorQueM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] mayorIgualM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] menorIgualM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] igualM = { 
         //      int  || real || string ||  bool
            {    205  ,   205  ,   500  ,   500,},
            {    205  ,   205  ,   500  ,   500,},
            {    500  ,   500  ,   205  ,   500,},
            {    500  ,   500  ,   500  ,   500,}
        };

        public int[,] ANDM = { 
         //      int  || real || string ||  bool
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   205,}
        };

        public int[,] ORM = { 
         //      int  || real || string ||  bool
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   500,},
            {    500  ,   500  ,   500  ,   205,}
        };

        public int[,] NOTM = { 
         //      int  || real || string ||  bool
            {    500  ,   500  ,   500  ,   205,},

        };


        public void ejecSintactico()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 200) //program
            {
                p++;
                if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
                {

                    p++;
                    if (listaToken.ElementAt<Token>(p).ValorToken == 114) //;
                    {
                        p++;
                        block();
                    }
                    else if(listaToken.ElementAt<Token>(p).ValorToken != 115) //.
                    {
                        listaError.Add(ManejoErrores(507, listaToken.ElementAt<Token>(p).Linea));
                    }
                }
            }
            else
            {
                listaError.Add(ManejoErrores(506, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        public void block()
        {
            variableDeclarationPart();
            if (!errorsintactico) { statementPart(); }
            
        }

        public void variableDeclarationPart()
        {

            
            if (listaToken.ElementAt<Token>(p).ValorToken == 201) //var
            {
                p++;
                variableDeclaration();
                if (listaToken.ElementAt<Token>(p).ValorToken == 114)//;
                {
                    while (listaToken.ElementAt<Token>(p).ValorToken == 114) //;
                    {
                        p++;
                        variableDeclaration();
                        if (listaToken.ElementAt<Token>(p).ValorToken != 114)//;
                        {
                            if(listaToken.ElementAt<Token>(p).ValorToken == 206 && listaToken.ElementAt<Token>(p-1).ValorToken == 114) { break; } //begin y ;
                            listaError.Add(ManejoErrores(520, listaToken.ElementAt<Token>(p - 1).Linea)); 
                            errorsintactico = true;
                            break;
                        }
                        


                    }
                }
                else
                {
                    listaError.Add(ManejoErrores(5201, listaToken.ElementAt<Token>(p-1).Linea)); errorsintactico = true;

                }
                

            }
            else
            {
                listaError.Add(ManejoErrores(528, listaToken.ElementAt<Token>(p).Linea)); errorsintactico = true;// se espera var    
            }
        } 

        private void variableDeclaration()
        {
            //p++;
            if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
            {
                agregarVariablePilaAux(listaToken.ElementAt<Token>(p).Lexema);
                p++;

                while (listaToken.ElementAt<Token>(p).ValorToken == 117) //,
                {
                    p++;
                    if (listaToken.ElementAt<Token>(p).ValorToken == 100)//id
                    {
                        agregarVariablePilaAux(listaToken.ElementAt<Token>(p).Lexema);
                        p++;
                    }
                    else
                    {
                        listaError.Add(ManejoErrores(504, listaToken.ElementAt<Token>(p).Linea));//corregir error
                    }
                }
                int ma = listaToken.ElementAt<Token>(p).ValorToken;
                if (listaToken.ElementAt<Token>(p).ValorToken == 116)//:
                {
                    type();
                }

            }
            else if(listaToken.ElementAt<Token>(p).ValorToken != 206)
            {
                listaError.Add(ManejoErrores(509, listaToken.ElementAt<Token>(p).Linea));
            }
        } 

        private void type()
        {
            p++;
            if (listaToken.ElementAt<Token>(p).ValorToken == 202 ||
                listaToken.ElementAt<Token>(p).ValorToken == 203 ||
                listaToken.ElementAt<Token>(p).ValorToken == 204 ||
                listaToken.ElementAt<Token>(p).ValorToken == 205)
            {
                if (listaToken.ElementAt<Token>(p).ValorToken == 202)//string
                {
                    definirVariable(202);
                    p++;
                }
                else if (listaToken.ElementAt<Token>(p).ValorToken == 203)//integer
                {
                    definirVariable(203);
                    p++;
                }
                else if (listaToken.ElementAt<Token>(p).ValorToken == 204)//real
                {
                    definirVariable(204);
                    p++;
                }
                else if (listaToken.ElementAt<Token>(p).ValorToken == 205)//boolean
                {
                    definirVariable(205);
                    p++;
                }
                listAuxVar.Clear();
                
            }
            else
            {
                listaError.Add(ManejoErrores(505, listaToken.ElementAt<Token>(p).Linea));
            }
        } 

        public void statementPart()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 206)// begin
            {
                p++;
                statement();
                while (listaToken.ElementAt<Token>(p).ValorToken == 114) //;
                {
                    p++;
                    
                    if (listaToken.ElementAt<Token>(p).ValorToken == 207) //end
                    {
                        break;
                    }
                    statement();
                }
                if (listaToken.ElementAt<Token>(p).ValorToken == 207) //end
                {
                    p++;
                    if (listaToken.ElementAt<Token>(p).ValorToken == 114) //;
                    {
                        p++;
                    }
                    else if (listaToken.ElementAt<Token>(p).ValorToken == 115) // .final
                    {
                        MandarTaPolish("fin Programa");
                        leerListaPolish();
                        MessageBox.Show("ANALISIS SINTACTICO Y SEMANTICO TERMINADO");
                    }
                    else 
                    {
                        listaError.Add(ManejoErrores(508, listaToken.ElementAt<Token>(p).Linea));//corregir error
                    }
                }
                else
                {
                    listaError.Add(ManejoErrores(529, listaToken.ElementAt<Token>(p).Linea));
                }
            }
            else
            {
                listaError.Add(ManejoErrores(509, listaToken.ElementAt<Token>(p-1).Linea));
            }

        }

        private void statement()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 100||//id
                listaToken.ElementAt<Token>(p).ValorToken == 208||//read
                listaToken.ElementAt<Token>(p).ValorToken == 209)//write
            {

                simpleStatement();
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 210||//if
                     listaToken.ElementAt<Token>(p).ValorToken == 213)//while
            {
                structuredStatement();
            }
            else
            {
                 listaError.Add(ManejoErrores(510, listaToken.ElementAt<Token>(p).Linea));//end
            }
        }

        private void simpleStatement()
        {
            
            if (listaToken.ElementAt<Token>(p).ValorToken == 100)// id
            {
                assignmentStatement();
                vaciarAuxPostfijo();
                if (!evaluarPostfijo(listaPostfix)) { listaError.Add(ManejoErrores(532, listaToken.ElementAt<Token>(p).Linea)); }
                listaAuxPostfix.Clear();listaPostfix.Clear();
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 208)//read
            {

                MandarTaPolishAuxiliar(listaToken.ElementAt<Token>(p));
                p++;
                readStatement();
                listaAuxPostfix.Clear(); listaPostfix.Clear();
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 209)//write
            {
                MandarTaPolishAuxiliar(listaToken.ElementAt<Token>(p));
                writeStatement();
                listaAuxPostfix.Clear(); listaPostfix.Clear();
            }
            else
            {
                listaError.Add(ManejoErrores(511, listaToken.ElementAt<Token>(p).Linea));
            }
            
        }

        private void assignmentStatement() 
        {
            variable();
            if (listaToken.ElementAt<Token>(p).ValorToken == 113) // :=
            { 
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
                expression();

            }
            else
            {
                listaError.Add(ManejoErrores(510, listaToken.ElementAt<Token>(p).Linea)); // error de asignaciones if write else while
            }
        }

        private void variable()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 100)//id
            {
                // p++; pormientras xq no se que chucha son estos metodos
                entireVariable();
            }
            else if(listaToken.ElementAt<Token>(p).ValorToken == 120 || listaToken.ElementAt<Token>(p).ValorToken == 101 || listaToken.ElementAt<Token>(p).ValorToken == 102)//valores no definidos
            { unsignedConstant(); }
            else
            {
                listaError.Add(ManejoErrores(504, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void entireVariable()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
            {
                variableIdentifier();
            }
            else
            {
                listaError.Add(ManejoErrores(504, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void variableIdentifier()//
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
            {
                variableNoDefinida(listaToken.ElementAt<Token>(p).Lexema);
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(504, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void readStatement()
        {
            
            if (listaToken.ElementAt<Token>(p).ValorToken == 118) // (
            {
                p++;
                variable();
                while (listaToken.ElementAt<Token>(p).ValorToken == 117) // ,
                    {          
                        p++;
                        variable();

                    }
                    listaAuxPostfix.Clear();
                if(listaToken.ElementAt<Token>(p).ValorToken == 119)// ))
                {
                    p++;
                    MandarTaPolish(tokenAux.Lexema);
                }
                else
                {
                    listaError.Add(ManejoErrores(524, listaToken.ElementAt<Token>(p).Linea));
                }
            }
            else
            {
                listaError.Add(ManejoErrores(526, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void writeStatement()
        {
            //p++;
            if (listaToken.ElementAt<Token>(p).ValorToken == 209) // write
            {
                p++;
                if(listaToken.ElementAt<Token>(p).ValorToken == 118) // (
                {
                    p++;
                    variable();
                    while (listaToken.ElementAt<Token>(p).ValorToken == 117) //,
                    {
                        p++;
                        variable();
                    }
                }
                else
                {
                    listaError.Add(ManejoErrores(526, listaToken.ElementAt<Token>(p).Linea));
                }
                if (listaToken.ElementAt<Token>(p).ValorToken == 119) // )
                {
                    p++;
                    MandarTaPolish(tokenAux.Lexema);
                }
                else
                {
                    listaError.Add(ManejoErrores(524, listaToken.ElementAt<Token>(p).Linea));
                }
            }
            else
            {
                listaError.Add(ManejoErrores(500, listaToken.ElementAt<Token>(p).Linea)); //arreglar errores al final
            }
        }

        private void structuredStatement() 
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 210) //if
            {
                conditionalStatement();  
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 213) //while
            {
                repetitiveStatement();
            }
            else
            {
                listaError.Add(ManejoErrores(512, listaToken.ElementAt<Token>(p).Linea));
            }

        }

        private void conditionalStatement()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 210)//if
            {
                ifStatement();
            }
            else
            {
                listaError.Add(ManejoErrores(516, listaToken.ElementAt<Token>(p).Linea));
            }
            
        }

        private void ifStatement()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 210)//if
            {
                p++;
                iteradorIF++;
                expression();
                vaciarAuxPostfijo();
                if (!evaluarPostfijo(listaPostfix)) { listaError.Add(ManejoErrores(532, listaToken.ElementAt<Token>(p).Linea)); }
                listaAuxPostfix.Clear(); listaPostfix.Clear();
                if (listaToken.ElementAt<Token>(p).ValorToken == 211)//then
                {
                    añadirSalto("BRF","A"+iteradorIF);
                    p++;
                    do
                    {

                        statement();
                        if (listaToken.ElementAt<Token>(p).ValorToken == 114) { p++; }
                        else { if (listaToken.ElementAt<Token>(p).ValorToken != 212) { listaError.Add(ManejoErrores(508, listaToken.ElementAt<Token>(p).Linea)); } break; }
                    } while (listaToken.ElementAt<Token>(p).ValorToken == 100 ||//id
                        listaToken.ElementAt<Token>(p).ValorToken == 208 ||//read
                        listaToken.ElementAt<Token>(p).ValorToken == 209 ||//write
                        listaToken.ElementAt<Token>(p).ValorToken == 210 ||//if
                        listaToken.ElementAt<Token>(p).ValorToken == 213);//while
                    añadirSalto("BRI", "B"+iteradorIF);
                    if (listaToken.ElementAt<Token>(p).ValorToken == 212)//else
                    {

                        p++;
                        añadirDireccion("A" + iteradorIF);
                        do
                        {

                            statement();
                            if (listaToken.ElementAt<Token>(p).ValorToken == 114) { p++; }
                            else { 
                                if (listaToken.ElementAt<Token>(p).ValorToken != 218){ listaError.Add(ManejoErrores(508, listaToken.ElementAt<Token>(p).Linea)); }
                                break;
                            }
                        } while (listaToken.ElementAt<Token>(p).ValorToken == 100 ||//id
                            listaToken.ElementAt<Token>(p).ValorToken == 208 ||//read
                            listaToken.ElementAt<Token>(p).ValorToken == 209 ||//write
                            listaToken.ElementAt<Token>(p).ValorToken == 210 ||//if
                            listaToken.ElementAt<Token>(p).ValorToken == 213);//while
                                                                             
                    }
                    if (listaToken.ElementAt<Token>(p).ValorToken == 218)//endif
                    {
                        p++;
                        iteradorIF--;
                        saltopendienteIF = true;
                    }
                    else { listaError.Add(ManejoErrores(533, listaToken.ElementAt<Token>(p).Linea)); }
                }
                else
                {
                    
                     listaError.Add(ManejoErrores(515, listaToken.ElementAt<Token>(p).Linea)); 
                        
                }
            }
            else
            {
                listaError.Add(ManejoErrores(516, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void repetitiveStatement()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 213)//while
            {
                whileStatement();
            }
            else
            {
                listaError.Add(ManejoErrores(514, listaToken.ElementAt<Token>(p).Linea));
            }

        }

        private void whileStatement()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 213)//while
            {
                p++;
                iteradorWhile++;
                añadirDireccion( "W" + iteradorWhile);
                expression();
                vaciarAuxPostfijo();
                if (!evaluarPostfijo(listaPostfix)) { listaError.Add(ManejoErrores(532, listaToken.ElementAt<Token>(p).Linea)); }
                listaAuxPostfix.Clear(); listaPostfix.Clear();
                añadirSalto("BRF", "E"+iteradorWhile);
                if (listaToken.ElementAt<Token>(p).ValorToken == 214)//do
                {
                    p++;
                    if (listaToken.ElementAt<Token>(p).ValorToken == 206)//begin
                    {
                        p++;
                        do
                        {
                            statement();
                            if (listaToken.ElementAt<Token>(p - 1).ValorToken == 218) {
                                statement(); p++; }
                            if (listaToken.ElementAt<Token>(p).ValorToken == 114) {
                                p++;
                                
                            }
                            
                            else  {
                                    if (listaToken.ElementAt<Token>(p).ValorToken != 207) { 
                                    listaError.Add(ManejoErrores(508, listaToken.ElementAt<Token>(p).Linea)); } 
                                    break;
                            }
                        } while(listaToken.ElementAt<Token>(p).ValorToken != 207);
                        añadirSalto("BRI", "W" + iteradorWhile);
                        if (listaToken.ElementAt<Token>(p).ValorToken == 207 && listaToken.ElementAt<Token>(p+1).ValorToken == 114)//end;
                        {
                            p++;
                            iteradorWhile--;
                            saltopendienteW = true;
                        }
                        else { listaError.Add(ManejoErrores(534, listaToken.ElementAt<Token>(p).Linea)); }
                    }
                }
                else
                {
                    listaError.Add(ManejoErrores(513, listaToken.ElementAt<Token>(p).Linea));
                }
            }
            else
            {
                listaError.Add(ManejoErrores(514, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void expression()
        {
            simpleExpression();
            if (listaToken.ElementAt<Token>(p).ValorToken == 107 || //Operadores relacionales
                listaToken.ElementAt<Token>(p).ValorToken == 108 ||
                listaToken.ElementAt<Token>(p).ValorToken == 109 ||
                listaToken.ElementAt<Token>(p).ValorToken == 110 ||
                listaToken.ElementAt<Token>(p).ValorToken == 111 ||
                listaToken.ElementAt<Token>(p).ValorToken == 112)
            {
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
                simpleExpression();
            }
        }

        private void relationalOperator()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 107||//Operadores relacionales
                listaToken.ElementAt<Token>(p).ValorToken == 108||
                listaToken.ElementAt<Token>(p).ValorToken == 109||
                listaToken.ElementAt<Token>(p).ValorToken == 110||
                listaToken.ElementAt<Token>(p).ValorToken == 111||
                listaToken.ElementAt<Token>(p).ValorToken == 112)
            {
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(517, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void simpleExpression()
        {
            if(listaToken.ElementAt<Token>(p).ValorToken == 118 || //(
                listaToken.ElementAt<Token>(p).ValorToken == 100 || // id
                listaToken.ElementAt<Token>(p).ValorToken == 217 || //not
                listaToken.ElementAt<Token>(p).ValorToken == 101 ||//entero
                listaToken.ElementAt<Token>(p).ValorToken == 102 ||//real
                listaToken.ElementAt<Token>(p).ValorToken == 120) //string
            {
                term();
                if(listaToken.ElementAt<Token>(p).ValorToken == 103||// +
                    listaToken.ElementAt<Token>(p).ValorToken == 104||// -
                    listaToken.ElementAt<Token>(p).ValorToken == 216)// or
                {
                    addingOperator();
                    term();
                }
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 103 ||//+
                listaToken.ElementAt<Token>(p).ValorToken == 104)//-
            {
                p++;
                sign();
                term();
                if (listaToken.ElementAt<Token>(p).ValorToken == 103 ||// +
                    listaToken.ElementAt<Token>(p).ValorToken == 104 ||// -
                    listaToken.ElementAt<Token>(p).ValorToken == 216)// or
                {
                    addingOperator();
                    term();
                }
            }
            else { listaError.Add(ManejoErrores(525, listaToken.ElementAt<Token>(p).Linea)); }
            
        }

        private void addingOperator()
        {
            agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
            if (listaToken.ElementAt<Token>(p).ValorToken == 103)//+
            {
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 104)//-
            {
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 216)//or
            {
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(518, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void term()
        {
            factor();
            if (listaToken.ElementAt<Token>(p).ValorToken == 105 ||// *
                    listaToken.ElementAt<Token>(p).ValorToken == 106 ||// /
                    listaToken.ElementAt<Token>(p).ValorToken == 125)// and
            {
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
                term();
            }
            

        }

        private void multiplyingOperator()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 105)//*
            {
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 106)// /
            {
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 215)//and
            {
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(519, listaToken.ElementAt<Token>(p).Linea));
            }
        }
        
        private void factor()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
            {
                variable();
            }
            else if(listaToken.ElementAt<Token>(p).ValorToken == 101 ||//entero
                    listaToken.ElementAt<Token>(p).ValorToken == 102 ||//real
                    listaToken.ElementAt<Token>(p).ValorToken == 120)//string

                {
                unsignedConstant();
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 118)//(
            {
                p++;
                expression();
                if (listaToken.ElementAt<Token>(p).ValorToken == 119)//)
                {
                    p++;
                }
                else { listaError.Add(ManejoErrores(524, listaToken.ElementAt<Token>(p).Linea)); }
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 217) //not
            {
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
                factor();
            }
            else
            {
                listaError.Add(ManejoErrores(501, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void unsignedConstant()
        {
            int valortoken = listaToken.ElementAt<Token>(p).ValorToken;
            if (listaToken.ElementAt<Token>(p).ValorToken == 101 ||//entero

                listaToken.ElementAt<Token>(p).ValorToken == 102)//real
            {
                unsignedNumber();
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 120)//string
            {
                agregarAuxPostfijo(202);
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 100) //id
            {
                p++;
            }
            
            else if(listaToken.ElementAt<Token>(p).ValorToken != 211) //else
            {
                listaError.Add(ManejoErrores(503, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void unsignedInteger()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 101)//int
            {
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(502, listaToken.ElementAt<Token>(p).Linea));
            }
        }

        private void unsignedNumber()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 101) //int
            {
                
                agregarAuxPostfijo(203);
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 102) //real
            {
                
                agregarAuxPostfijo(204);
                p++;
            }
            
            else
            {
                listaError.Add(ManejoErrores(501, listaToken.ElementAt<Token>(p).Linea));
            }
            
        }

        private void sign()
        {
            if (listaToken.ElementAt<Token>(p).ValorToken == 103)//+
            {
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
            }
            else if (listaToken.ElementAt<Token>(p).ValorToken == 104)//-
            {
                agregarAuxPostfijo(listaToken.ElementAt<Token>(p).ValorToken);
                p++;
            }
            else
            {
                listaError.Add(ManejoErrores(500, listaToken.ElementAt<Token>(p).Linea));
            }
        }
        //______________________________________SEMANTICA
        public void definirVariable(int tipo)
        {
            for (int i = 0; i < listAuxVar.Count; i++)
            {
                if (listaVar.Count <= 0)
                {
                    listAuxVar.ElementAt<Variable>(0).TipoVariable = tipo;
                    listaVar.Add(listAuxVar.ElementAt<Variable>(0));
                }
                else
                {
                    if (!variableDefinida(listAuxVar.ElementAt<Variable>(i).Nombre))
                    {
                        listAuxVar.ElementAt<Variable>(i).TipoVariable = tipo;
                        listaVar.Add(listAuxVar.ElementAt<Variable>(i));
                    }
                    else
                    {
                        listaError.Add(ManejoErrores(530, listaToken.ElementAt<Token>(p).Linea));
                        break;
                    }
                }
            }
        }

        private bool variableDefinida(string nombreVar)
        {

            for (int i = 0; i < listaVar.Count; i++)
            {
                if (nombreVar == listaVar.ElementAt<Variable>(i).Nombre)
                {
                    return true;
                }             
            }
            return false;

        }

        private bool variableNoDefinida(string nombreVar)
        {
            for (int i = 0; i < listaVar.Count; i++)
            {
                if (nombreVar == listaVar.ElementAt<Variable>(i).Nombre)
                {
                    agregarAuxPostfijo(listaVar.ElementAt<Variable>(i).TipoVariable);
                    return true;
                }
            }
            listaError.Add(ManejoErrores(531, listaToken.ElementAt<Token>(p).Linea));
            return false;
        }

        public void agregarVariablePilaAux(string nombreVar)
        {
            Variable nuevaVariable = new Variable() { Nombre = nombreVariable };
            nuevaVariable.Nombre = nombreVar;

            listAuxVar.Add(nuevaVariable);
        }
        
        public void vaciarAuxPostfijo()
        {
            while (listaAuxPostfix.Count > 0)
            {
                listaPostfix.Add(listaAuxPostfix.Last());
                switch (listaAuxPostfix.Last())
                {
                    case 103: lexemaoperador = "+"; break;
                    case 104: lexemaoperador = "-"; break;
                    case 105: lexemaoperador = "*"; break;
                    case 106: lexemaoperador = "/"; break;
                    case 107: lexemaoperador = ">"; break;
                    case 108: lexemaoperador = ">="; break;
                    case 109: lexemaoperador = "<"; break;
                    case 110: lexemaoperador = "<="; break;
                    case 111: lexemaoperador = "=="; break;
                    case 112: lexemaoperador = "<>"; break;
                    case 113: lexemaoperador = ":="; break;
                    case 215: lexemaoperador = "and"; break;
                    case 216: lexemaoperador = "or"; break;
                    case 217: lexemaoperador = "Not"; break;

                }
                MandarTaPolish(lexemaoperador);
                if(listaAuxPostfix.Count > 0) listaAuxPostfix.RemoveAt(listaAuxPostfix.Count - 1);
            }
        }
        public void agregarAuxPostfijo(int valortoken)
        {
            if (valortoken == 202 || valortoken == 203 || valortoken == 204 || valortoken == 205 )//operando
            {

                listaPostfix.Add(valortoken);
                if (listaToken.ElementAt(p).ValorToken == 211) { MandarTaPolish(listaToken.ElementAt(p-1).Lexema); }
                else { MandarTaPolish(listaToken.ElementAt(p).Lexema); }
            }

            else//operador
            {
                if(listaAuxPostfix.Count == 0) { listaAuxPostfix.Add(valortoken); }
                else
                {
                    int ultimoperdadorlsitaAux = listaAuxPostfix.ElementAt(listaAuxPostfix.Count - 1);
                    switch (valortoken)
                    {
                        case 105://   *
                        case 106://   /
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 0)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 103://   +
                        case 104://   -
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 1)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 107://   >
                        case 108://   >=
                        case 109://   <
                        case 110://   <=
                        case 111://   ==
                        case 112://   <>
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 2)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 217://   Not
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 3)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 215://   and
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 4)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 216://   or
                            if (consultarPrioridad(ultimoperdadorlsitaAux) == 5)
                            {
                                listaPostfix.Add(ultimoperdadorlsitaAux);
                                listaAuxPostfix.Remove(ultimoperdadorlsitaAux); listaAuxPostfix.Add(valortoken);
                            }
                            else { listaAuxPostfix.Add(valortoken); }
                            break;
                        case 113:// :=
                            listaAuxPostfix.Add(valortoken);
                            break;

                    }
                }
                
            }
                
        }
        
        public int consultarPrioridad(int op1)
        {
            if (op1 == 106 || op1 == 105) { op1 = 0; } //  /,*
            else if(op1 == 103 || op1 == 104) { op1 = 1; } // +/-
            else if( op1 == 107 || op1 == 108 || op1 == 109 || op1 == 110 || op1 == 111 || op1 == 112) { op1 = 2; } //>,>=,<,<=,==,<> 
            else if (op1 == 217) { op1 = 3; }//not
            else if (op1 == 215) { op1 = 4; }//and
            else if (op1 == 216) { op1 = 5; }//or
            return op1;
        }

        public int verificarTipos(int operando1,int operando2,int operador)
        {
            int tipo = 0;
            switch (operando1)
            {
                case 203:
                    operando1 = 0;
                    break;
                case 204:
                    operando1 = 1;
                    break;
                case 202:
                    operando1 = 2;
                    break;
                case 205:
                    operando1 = 3;
                    break;
            }
            switch (operando2)
            {
                case 203:
                    operando2 = 0;
                    break;
                case 204:
                    operando2 = 1;
                    break;
                case 202:
                    operando2 = 2;
                    break;
                case 205:
                    operando2 = 3;
                    break;
            }


            switch (operador)
            {
                case 103:
                    tipo = sumaM[operando1, operando2];
                    break;
                case 104:
                    tipo = restaM[operando1, operando2];
                    break;
                case 105:
                    tipo = multiplicacionM[operando1, operando2];
                    break;
                case 106:
                    tipo = divisionM[operando1, operando2];
                    break;
                case 107:
                    tipo = mayorQueM[operando1, operando2];
                    break;
                case 108:
                    tipo = mayorIgualM[operando1, operando2];
                    break;
                case 109:
                    tipo = menorQueM[operando1, operando2];
                    break;
                case 110:
                    tipo = menorIgualM[operando1, operando2];
                    break;
                case 111:
                    tipo = igualM[operando1, operando2];
                    break;
                case 112:
                    tipo = diferenteM[operando1, operando2];
                    break;
                case 113:
                    tipo = asignacionM[operando1, operando2];
                    break;
                case 215:
                    tipo = ANDM[operando1, operando2];
                    break;
                case 216:
                    tipo = ORM[operando1, operando2];
                    break;
                case 217:
                    tipo = NOTM[operando1, operando2];
                    break;

            }
            return tipo;
        }
        public bool evaluarPostfijo(List<int> listapostfija)
        {
            int i = 0, tiporegreso;
            do
            {
                if (listaAuxPostfix.Count == 0) {
                    listaAuxPostfix.Add(listapostfija.ElementAt(0)); listapostfija.RemoveAt(0);
                    listaAuxPostfix.Add(listapostfija.ElementAt(0)); listapostfija.RemoveAt(0);

                }
                else if (listapostfija.ElementAt(0) == 202 || listapostfija.ElementAt(0) == 203 ||
                    listapostfija.ElementAt(i) == 204 || listapostfija.ElementAt(i) == 205)
                {
                    listaAuxPostfix.Add(listapostfija.ElementAt(0)); listapostfija.RemoveAt(0);

                }
                else
                {
                    tiporegreso = verificarTipos(listaAuxPostfix.ElementAt(listaAuxPostfix.Count - 1),
                        listaAuxPostfix.ElementAt(listaAuxPostfix.Count - 2),
                        listapostfija.ElementAt(0));
                    if (tiporegreso == 500)
                    {
                        return false;
                    }
                    else
                    {
                        listaAuxPostfix.RemoveAt(listaAuxPostfix.Count-1); 
                        listaAuxPostfix.RemoveAt(listaAuxPostfix.Count - 1);
                        listaAuxPostfix.Add(tiporegreso);
                    }


                }

            }
            while (listaAuxPostfix.Count > 1);
                  return true;
        }
        //------------------------------POLISH------------------------------------
        public void MandarTaPolish(string lexema)
        {
            NodoPolish nodoPolishnuevo = new NodoPolish();
            nodoPolishnuevo.Lexema = lexema;
            if (saltopendienteIF)
            {
                nodoPolishnuevo.Direccionbrinco = "B"+(iteradorIF+1);
                saltopendienteIF = false;
            }
            if (saltopendienteW)
            {
                nodoPolishnuevo.Direccionbrinco = "E" + (iteradorWhile + 1);
                saltopendienteW = false;
            }
            if (direccionpendiente)
            {
                nodoPolishnuevo.Direccionbrinco = direccionAux;
                direccionpendiente = false;
            }
            listaPolish.Add(nodoPolishnuevo);
        }
        public void MandarTaPolishAuxiliar(Token token)
        {
            tokenAux.Lexema = token.Lexema;

        }
        public void añadirSalto(string lexema, string brinco)
        {
            NodoPolish nodoPolishnuevo = new NodoPolish();
            nodoPolishnuevo.Lexema = lexema;
            nodoPolishnuevo.Brinco = brinco;
            listaPolish.Add(nodoPolishnuevo);

        }
        public void añadirDireccion(string direccion)
        {
            direccionAux = direccion;
            direccionpendiente = true;

        }
        //----------------ENSAMBLADOR-----------------------
        public void leerListaPolish()
        {
            titulo += "INCLUDE MACROS.MAC\r\nDOSSEG\r\n.MODEL SMALL\r\nSTACK 100h\r\n.DATA\r\n";
            
            string nodoPolActual;
            for (int i = 0; i < listaPolish.Count(); i++)
            {
                nodoPolActual = listaPolish.ElementAt(i).Lexema;
                if(listaPolish.ElementAt(i).Direccionbrinco != null)
                {
                    textoensamblador += "\t"+ listaPolish.ElementAt(i).Direccionbrinco+ ":\r\n";
                }
                switch (nodoPolActual)
                {
                    case "write":
                        textoensamblador += "\t\tWRITE " + variablesPolish.Pop()+ "\r\n";
                        break;
            //----------------Read no funcional--------------------
                    case "read":
                        textoensamblador += "\t\tREAD "  + variablesPolish.Pop() + "\r\n";
                        break;
            //-----------------------Operaciones simples------------------------------------------
                    case "+":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales); iteradorVtemporales++;
                        textoensamblador += "\t\tSUMAR " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "-":
                        variablesTemporalesPolish.Push("t"+ iteradorVtemporales); iteradorVtemporales++;
                        textoensamblador += "\t\tRESTA " + variablesPolish.Pop() + " , "+ variablesPolish.Pop() + " , "
                        +variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "/":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tDIVIDE " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "*":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tMULTI " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                //---------------------Condicionales--------------------------------------------
                    case "<":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tI_MENOR " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "<=":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tI_MENORIGUAL " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case ">":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tI_MAYOR " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case ">=":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tI_MAYORIGUAL " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "==":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\tI_IGUAL " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
                    case "<>":
                        variablesTemporalesPolish.Push("t" + iteradorVtemporales);iteradorVtemporales++;
                        textoensamblador += "\t\t\tI_DIFERENTES " + variablesPolish.Pop() + " , " + variablesPolish.Pop() + " , "
                        + variablesTemporalesPolish.Peek() + "\r\n";
                        variablesPolish.Push(variablesTemporalesPolish.Peek());
                        break;
            //----------------------------Asignacion--------------------------------------
                    case ":=":
                        textoensamblador += "\t\tI_ASIGNAR " + variablesPolish.ElementAt(1) + " , " + variablesPolish.Pop() +"\r\n";
                        variablesPolish.Pop();
                        break;
            //-----------------------------Saltos-------------------------------------
                    case "BRI":
                        textoensamblador += "\t\tJMP " + listaPolish.ElementAt(i).Brinco+ "\r\n";
                        break;
                    case "BRF":
                        textoensamblador += "\t\tJF " + variablesTemporalesPolish.Peek() + " , " + listaPolish.ElementAt(i).Brinco+ "\r\n";
                        break;
                    default:
                        variablesPolish.Push(nodoPolActual);
                        break;
                        
                }


            }
            //-----------------------Definicion de Variables------------------------------
            for (int i = 0; i < listaVar.Count(); i++)
            {
                variablesAsm += "\t\t\t" + listaVar.ElementAt(i).Nombre + " DW ' ', '$'\r\n";
            }
            for (int i = 0; i < variablesTemporalesPolish.Count; i++)
            {
                variablesAsm += "\t\t\t" + variablesTemporalesPolish.ElementAt(i) + " DW , ?\r\n";
            }
            variablesAsm += ".CODE\r\n.386\r\nBEGIN:\r\n\t\t\t MOV AX, @DATA\r\n\t\t\tMOV DS, AX\r\n" +
                "CALL COMPI\r\n\t\t\t MOV AX, 4C00H\r\n\t\t\tINT 21H\r\nCOMPI PROC\r\n\r\n";

            textoensamblador += "\t\t\tret\r\n\r\nCOMPI ENDP\r\nEND BEGIN";
            textoASMFinal += titulo+ "\r\n" + variablesAsm + textoensamblador;
        }
        private Error ManejoErrores(int estado, int linea)
        {
            string mensajeError;

            switch (estado)
            {
                case 500:
                    mensajeError = "Error se esperaba un '+' o '-'"; //sign()
                    break;
                case 501:
                    mensajeError = "Error se esperaba un entero o un real"; //unsignedNumber()
                    break;
                case 502:
                    mensajeError = "Error se esperaba un entero";//unsignedInteger()
                    break;
                case 503:
                    mensajeError = "Error se esperaba una variable de valor entero | real | string | identificador";//unsignedConstant()
                    break;
                case 504:
                    mensajeError = "Error se esperaba un token 100 identificador"; //variableDeclaration();
                    break;
                case 505:
                    mensajeError = "Error se esperabna un valor string|bool|integer|real";//type()
                    break;
                case 506:
                    mensajeError = "Error se esperaba la palabra program";//ejectSintact()
                    break;
                case 507:
                    mensajeError = "Error se esperaba un punto . "; //ejectSintact()
                    break;
                case 508:
                    mensajeError = "Error se esperaba un ;"; //statmentPart()
                    break;
                case 509:
                    mensajeError = "Error se esperaba begin";//statmentPart()
                    break;
                case 510:
                    mensajeError = "Error se esperaba :=|read|write|if|while";//statment()
                    break;
                case 511:
                    mensajeError = "Error se esperaba :=|read|write"; //simpleStatment()
                    break;
                case 512:
                    mensajeError = "Error se esperaba if|while"; //structuredStatment()
                    break;
                case 513:
                    mensajeError = "Error se esperaba do";//whileStatment()
                    break;
                case 514:
                    mensajeError = "Error se esperaba while";//whileStatment()
                    break;
                case 515:
                    mensajeError = "Error se esperaba then";//ifStatment()
                    break;
                case 516:
                    mensajeError = "Error se esperaba if"; //ifStatment()
                    break;
                case 517:
                    mensajeError = "Error se esperabna un operador relacional";//relationalOperator();
                    break;
                case 518:
                    mensajeError = "Error se esperaba +|-|or";//addingOperator()
                    break;
                case 519:
                    mensajeError = "Error se esperaba *|/|and"; //multiplyingOperator()
                    break;
                case 520:
                    mensajeError = "Error se esperaba un ;"; 
                    break;
                case 527:
                    mensajeError = "Error se esperaba un ; despues del tipo";
                    break;
                case 521:
                    mensajeError = "Error se esperaba begin";
                    break;
                case 522:
                    mensajeError = "Error se esperaba :=|read|write|if|while";
                    break;
                case 523:
                    mensajeError = "Error se esperaba :=|read|write"; 
                    break;
                case 524:
                    mensajeError = "Error se esperaba )";//readstatement()
                    break;
                case 525:
                    mensajeError = "Error se esperaba un factor";
                    break;
                case 526:
                    mensajeError = "Error se esperaba (";//readstatement()
                    break;
                case 528:
                    mensajeError = "se esperaba var";
                    break;
                case 529: // ; de mas
                    mensajeError = "Error en declaracion de statements con ;";
                    break;
                    /*ERRORES DE SEMANTICA*/
                case 530: 
                    mensajeError = "Error variable ya definida";
                    break;
                case 531:
                    mensajeError = "Error variable no definida";
                    break;
                case 532:
                    mensajeError = "Error de tipos";
                    break;
                case 533:
                    mensajeError = "Se esperaba endif al acabar el if";
                    break;
                case 534:
                    mensajeError = "Se esperaba end; al acabar el while";
                    break;
                default:
                    mensajeError = "Error inesperado";
                    break;
                    
            }
            if(estado >= 530) { return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Semantico, Linea = linea }; }
            return new Error() { Codigo = estado, MensajeError = mensajeError, Tipo = tipoError.Sintatico, Linea = linea };


        }

    }
}
