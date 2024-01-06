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

        public void StoreInstruction(short instruction, short PC)
        {
            MemData[PC] = instruction;
        }
    }
}
