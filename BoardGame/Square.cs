using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.BoardGame
{
    public abstract class Square
    {
        private bool[,] traps;
        private int size = 21; // Tamaño del tablero
        private Random random = new Random();

        public Square()
        {
            traps = new bool[size, size];
            PlaceTraps();
        }

        private void PlaceTraps()
        {
            int numberOfTraps = (size * size) / 10; // Coloca trampas en el 10% de las casillas

            for (int i = 0; i < numberOfTraps; i++)
            {
                int x;
                int y;
                x = random.Next(size);
                y = random.Next(size);
                while (traps[x, y]) 
                { 
                    x = random.Next(size); 
                    y = random.Next(size);
                }
                traps[x, y] = true;
            }
        }

        public bool IsTrap(int x, int y)
        {
            return traps[x, y]; // Método para verificar si una casilla tiene una trampa
        }
        public void PrintTraps() 
        { 
            for (int y = 0; y < size; y++) 
            { 
                for (int x = 0; x < size; x++) 
                { 
                    if (traps[x, y]) 
                    { 
                        Console.Write("T "); 
                    } 
                } 
                Console.WriteLine(); 
            } 
        }
    }
}

