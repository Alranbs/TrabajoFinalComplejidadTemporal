
using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{

	public class Estrategia
	{
		private int CalcularDistancia(string str1, string str2)
		{
			// using the method
			String[] strlist1 = str1.ToLower().Split(' ');
			String[] strlist2 = str2.ToLower().Split(' ');
			int distance = 1000;
			foreach (String s1 in strlist1)
			{
				foreach (String s2 in strlist2)
				{
					distance = Math.Min(distance, Utils.calculateLevenshteinDistance(s1, s2));
				}
			}

			return distance;
		}

		public String Consulta1(ArbolGeneral<DatoDistancia> arbol)
		{
			string result = "";
			if(arbol.esHoja())
			{
				return arbol.getDatoRaiz().texto + "\n";
			}
			else
			{
				foreach(ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
				{
					result += Consulta1(hijo);
				}
			}
			return result;
		}


		public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
		{
			List<string> lista = new List<string>();
			string result = Consulta2_Auxiliar(arbol,lista);

            return result;
        }

		public string Consulta2_Auxiliar(ArbolGeneral<DatoDistancia> arbol, List<string> lista)
		{
			string result = "";
			lista.Add(arbol.getDatoRaiz().ToString());
			if (arbol.esHoja())
			{
				foreach(var elemento in lista)
				{
					result+= elemento + "-->";
				}
				result += "\n";
			}
			else
			{
				foreach(var hijo in arbol.getHijos())
				{
					result += Consulta2_Auxiliar(hijo, lista);
					lista.RemoveAt(lista.Count - 1);
				}
			}
			return result;
		}

		

		public String Consulta3(ArbolGeneral<DatoDistancia> arbol)
		{
			int flag = 0;
			string result = "0: ";
			int nivel = 0;
			Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();
			cola.encolar(arbol);
			cola.encolar(null);
			while(!cola.esVacia())
			{
				ArbolGeneral<DatoDistancia> temp = cola.desencolar();
				if (temp != null)
				{
					result += temp.getDatoRaiz().texto + " ";
					foreach(ArbolGeneral<DatoDistancia> hijo in temp.getHijos())
					{						
						flag = 0;
						cola.encolar(hijo);
					}					
				}
				if (temp == null && flag == 0)
				{
					nivel += 1;
					result += "\n" + nivel + ":\n ";
					flag = 1;
					cola.encolar(null);
				}
			}
		
			return result;
		}

		public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
		{
			int flag = 0;
			DatoDistancia raiz = arbol.getDatoRaiz();
			string datoRaiz = raiz.texto;
			string datoDato = dato.texto;
			int distancia = CalcularDistancia(datoRaiz, datoDato);
			if(arbol.esHoja())
			{
				DatoDistancia datoAgregado = new DatoDistancia(distancia, datoDato, dato.descripcion);
				ArbolGeneral<DatoDistancia> arbolAgregado = new ArbolGeneral<DatoDistancia>(datoAgregado);
				arbol.agregarHijo(arbolAgregado);
			}
			else
			{
				foreach(ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
					{
						DatoDistancia datoHijo = hijo.getDatoRaiz();
						int distanciaHijo = datoHijo.distancia;
						if (distanciaHijo == distancia)
						{
							flag = 1;
							AgregarDato(hijo,new DatoDistancia(0, datoDato, dato.descripcion));
							break;
						}
					}
				if (flag == 0)
				{
					DatoDistancia nuevodatoHijo = new DatoDistancia(distancia, datoDato, dato.descripcion);
					ArbolGeneral<DatoDistancia> nuevoHijo = new ArbolGeneral<DatoDistancia>(nuevodatoHijo);
					arbol.agregarHijo(nuevoHijo);
				}					
			}


			

		}

		public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
		{
            string datoRaiz = arbol.getDatoRaiz().texto;
			int distancia = CalcularDistancia(datoRaiz, elementoABuscar);
			if (distancia <= umbral)
			{
				collected.Add(arbol.getDatoRaiz());
			}
			if (!arbol.esHoja())
			{
				foreach(ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
				{
						if (hijo.getDatoRaiz().distancia <= umbral)
						{
							Buscar(hijo,elementoABuscar,umbral,collected);
						}

				}
			}	
        }
    }
}