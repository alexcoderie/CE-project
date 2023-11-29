using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public struct Memory
    {
        //(16 bit words as signed)
        short[] MemData;

        public void Init()
        {
            MemData = new short[400];

            for(int i = 0; i < 400; i++)
            {
                MemData[i] = 0;
            }

            MemData[0] = 0b0111000100000010;
            MemData[1] = 0b0111000100010010;
            MemData[2] = 0b0111000100100010;
            MemData[3] = 0b0111000100110010;
            MemData[4] = 0b0111000110000010;
        }

        public void ViewMem() //!!! Works, however it needs some "pretty printing" BS...
        {
            for (byte i = 0; i <= 39; i++)
            {
                for (byte j = 0; j <= 9; j++)
                {
                    Console.Write(MemData[((i*10)+j)] + " ");
                    //Console.Write((i * 10) + j + " "); //Testing 40x10 grid style printing
                }
                Console.WriteLine("");
            }
        }

        public short ExtractMem(short PC)
        {
            //Console.WriteLine(PC);
            return MemData[PC];
        }
    }
}
