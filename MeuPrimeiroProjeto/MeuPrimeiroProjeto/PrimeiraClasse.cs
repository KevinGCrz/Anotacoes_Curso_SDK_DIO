﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuPrimeiroProjeto
{
    public class PrimeiraClasse
    {
        public void HelloWorld()
        {
            Console.WriteLine("Digite seu nome: ");
            string nome = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("=========================================");
            Console.WriteLine($"Bem vindo(a) {nome}!");
            Console.WriteLine("...Pressione qualquer tecla para continuar...");
            Console.WriteLine("=========================================");
            Console.ReadKey();

        }
    }
}
