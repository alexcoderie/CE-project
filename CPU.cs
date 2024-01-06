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

/*
 * --- ANATOMY OF AN INSTRUCTION ---
 * From MSB to LSB:
 *      - 4 bits for the instruction OP code
 *      - 3 bits for the register you want to do the operation in (we have registers from 0 to 7 so only 3 bits are necessary)
 *      - 8 bits for to represent a given value or a value stored in a register
 *      - 1 bit as a selector. 
 *          If this bit is 1 the operation will be performed between the value stored in the register given in those 3 bits and the value stored in the register given in those 8 bits.
 *          If this bit is 0 the operation will be performed between the value stored in the register given in those 3 bits and the value given those 8 bits. 
 * --- INSTRUCTION OP CODES ---
 * INPUT:  0b0000
 * OUTPUT: 0b0001
 * ADD:    0b0010
 * SUB:    0b0011
 * MUL:    0b0100
 * DIV:    0b0101
 * MOD:    0b0110 
 * MOV:    0b0111
 * 
 * --- SPECIFIC INSTRUCTIONS FOR THE GAME ---
 * These are combination of the previous instructions. Basically, when the CPU encounters these instructions the ALU performs multiple low level instructions
 *
 * Left Player Scores :  0b1010000000000000
 * Right Player Scores : 0b1011000000000000
 * Move Up-Left:         0b1100000000000000
 * Move Up-Right:        0b1101000000000000
 * Move Down-Left:       0b1110000000000000
 * Move Down-Right:      0b1111000000000000
 *
 * R[0] is used to store the position of the left paddle
 * R[1] is used to store the position of the right paddle
 * R[2] is used to store the position of the ball on the X axis 
 * R[3] is used to store the position of the ball on the Y axis 
 * R[4] is used to store the score of the left player
 * R[5] is used to store the score of the right player
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public struct CPU
    {
        ALU alu { get; set; }
        Pong pong = new Pong(60, 20);

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
            if(PC == 399)
            {
                PC = 0;
            }
            else
            {
                PC++;
            }
            return fmem;
        }

        public void Execute(Memory mem)
        {
            short Instruction = Fetch(mem);
            short Rindex1;
            short Rindex2;
            short Op;

            //INPUT
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0000)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                INPUT(ref R[Rindex1]);
            }

            //OUTPUT
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0001)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                OUTPUT(R[Rindex1]);
            }

            //ADD
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0010)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);

                if((short)(Instruction & 0b1) == 1)
                {
                    alu.ADD(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.ADD(ref R[Rindex1], ref Rindex2);
                }
            }
            
            //SUB
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0011)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);
                if((short)(Instruction & 0b1) == 1)
                {
                    alu.SUB(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.SUB(ref R[Rindex1], ref Rindex2);
                }
            }

            //MUL
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0100)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);
                if((short)(Instruction & 0b1) == 1)
                {
                    alu.MUL(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.MUL(ref R[Rindex1], ref Rindex2);
                }    
            }

            //DIV
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0101)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);
                if((short)(Instruction & 0b1) == 1)
                {
                    alu.DIV(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.DIV(ref R[Rindex1], ref Rindex2);
                }
            }

            //MOD
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0110)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);
                if((short)(Instruction & 0b1) == 1)
                {
                    alu.MOD(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.MOD(ref R[Rindex1], ref Rindex2);
                }
            }

            //MOV
            if((((short)(Instruction >> 12)) & 0b1111) == 0b0111)
            {
                Rindex1 = (short)((Instruction >> 9) & 0b111);
                Rindex2 = (short)((Instruction >> 1) & 0b11111111);
                if((short)(Instruction & 0b1) == 1)
                {
                    alu.MOV(ref R[Rindex1], ref R[Rindex2]);
                }
                else
                {
                    alu.MOV(ref R[Rindex1], ref Rindex2);
                }
            }

            //Move Up-Left:    0b1100
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1100)
            {
                short val = (short)(0b1);
                alu.SUB(ref R[2], ref val);
                alu.SUB(ref R[3], ref val);
            }
            //Move Up-Right:    0b1101
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1101)
            {
                short val = (short)(0b1);
                alu.ADD(ref R[2], ref val);
                alu.SUB(ref R[3], ref val);
            }
            //Move Down-Left:    0b1110
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1110)
            {
                short val = (short)(0b1);
                alu.SUB(ref R[2], ref val);
                alu.ADD(ref R[3], ref val);
            }
            //Move Down-Right:    0b1111
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1111)
            {
                short val = (short)(0b1);
                alu.ADD(ref R[2], ref val);
                alu.ADD(ref R[3], ref val);
            }

            //Left Player Scores :  0b1010000000000000
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1010)
            {
                short val = (short)(0b1);
                short centerX = (short)(0b11110);
                short centerY = (short)(0b1010);
                alu.ADD(ref R[4], ref val);
                alu.MOV(ref R[2], ref centerX);
                alu.MOV(ref R[3], ref centerY);
            }

            //Right Player Scores :  0b1011000000000000
            if((((short)(Instruction >> 12)) & 0b1111) == 0b1011)
            {
                short val = (short)(0b1);
                short centerX = (short)(0b11110);
                short centerY = (short)(0b1010);
                alu.ADD(ref R[5], ref val);
                alu.MOV(ref R[2], ref centerX);
                alu.MOV(ref R[3], ref centerY);
            }
            Count++;
        }

        public void RUN(Memory mem)
        {
            pong.Initialization(ref PC, ref mem, ref R);
            mem.ExtractMem(PC);
            Execute(mem);
            mem.ExtractMem(PC);
            Execute(mem);

            while(true)
            {
                pong.Run(ref PC, ref mem, ref R);
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
