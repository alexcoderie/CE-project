/*
//UNSIGNED INT
System.Console.WriteLine("byte:   " + sizeof(byte)   + " bytes; " + sizeof(byte)*8   + " bits"); //1,8
System.Console.WriteLine("ushort: " + sizeof(ushort) + " bytes; " + sizeof(ushort)*8 + " bits"); //2,16

//SIGNED INT
System.Console.WriteLine("sbyte:  " + sizeof(sbyte)  + " bytes; " + sizeof(sbyte)*8  + " bits"); //1,8
System.Console.WriteLine("short:  " + sizeof(short)  + " bytes; " + sizeof(short)*8  + " bits"); //2,16

//CHAR
System.Console.WriteLine("char:   " + sizeof(char)   + " bytes; " + sizeof(char)*8   + " bits"); //2,16
*/

//byte   --- unsigned int,  8 bits
//ushort --- unsigned int, 16 bits
//short  ---   signed int, 16 bits


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public struct CPU
    {
        short PC; //Program Counter
        short LR; //Link Register
        short SP; //Stack Pointer

        //Registers
        //[-32768, 32767] => short
        short[] R = new short[8];

        //Status Flags
        byte N; //Negative
        byte Z; //Zero
        byte C; //Carry
        byte V; //Overflow

        //Count
        int Count;

        public CPU(ref Memory mem)
        {
            mem.Init();

            PC = 0x0000;
            LR = 0x0000;
            SP = 0x0190; //400

            R[7] = 0;
            R[6] = 0;
            R[5] = 0;
            R[4] = 0;
            R[3] = 0;
            R[2] = 0;
            R[1] = 0;
            R[0] = 0;

            N = 0;
            Z = 0;
            C = 0;
            V = 0;

            Count = 0;

            //Testing functions...
            //INPUT(ref R[1]);
            //Increment(ref R[2]);
        }

        public void ViewCPU()
        {
            //Increment(ref R1); // Testing Increment()

            Console.WriteLine("PC    = " + PC);
            Console.WriteLine("LR    = " + LR);
            Console.WriteLine("SP    = " + SP);

            Console.WriteLine("R7    = " + R[7]);
            Console.WriteLine("R6    = " + R[6]);
            Console.WriteLine("R5    = " + R[5]);
            Console.WriteLine("R4    = " + R[4]);
            Console.WriteLine("R3    = " + R[3]);
            Console.WriteLine("R2    = " + R[2]);
            Console.WriteLine("R1    = " + R[1]);
            Console.WriteLine("R0    = " + R[0]);

            Console.WriteLine("N     = " + N);
            Console.WriteLine("Z     = " + Z);
            Console.WriteLine("C     = " + C);
            Console.WriteLine("V     = " + V);

            Console.WriteLine("Count = " + Count);
        }

        public void ViewMem(Memory mem)
        {
            for (short i = 0; i <= 399; i++)
            {
                Console.Write(mem.ExtractMem(i) + " ");
                if (((i + 1) % 10) == 0)
                {
                    Console.Write("\n");
                }
            }
        }

        public short Increment(ref short R)
        {
            R++;
            return R;
        }

        public short INPUT(ref short R)
        {
            Console.WriteLine("INPUT NEEDED!");
            string s;
            do
            {
                s = Console.ReadLine();
            } while (!(short.TryParse(s, out R)));
            //Console.WriteLine("Input = " + R);
            return R;
        }

        public void OUTPUT(short R)
        {
            Console.WriteLine("Output: " + R);
        }

        public short Fetch(Memory mem)
        {
            short fmem = mem.ExtractMem(PC);
            //Console.WriteLine(PC + " --- " + fmem);
            PC++;
            return fmem;
        }

        public void Execute(Memory mem)
        {
            short Instruction = Fetch(mem);
            short Rindex1;
            short Rindex2;
            short Op;

            //INPUT
            if((((short)(Instruction >> 7)) & 0b111111111) == 0b011100010)
            {
                Rindex1 = (short)((Instruction >> 4) & 0b111);
                Op = (short)(Instruction & 0b111);
                INPUT(ref R[Rindex1]);
            }

            //OUTPUT
            if((((short)(Instruction >> 7)) & 0b111111111) == 0b011100011)
            {
                Rindex1 = (short)((Instruction >> 4) & 0b111);
                Op = (short)(Instruction & 0b111);
                OUTPUT(R[Rindex1]);
            }

            Count++;
        }

        public void RUN(Memory mem)
        {
            while(PC < 400)
            {
                if (mem.ExtractMem(PC) == 0)
                {
                    break;
                }
                else
                {
                    Execute(mem);
                }
            }
        }
    }
}
