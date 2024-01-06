using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public struct ALU
    {
        public void ADD(ref short a, ref short b) {
            a += b;
        }
        public void SUB(ref short a, ref short b) {
            a -= b;
        }
        public void MUL(ref short a, ref short b) {
            a *= b;
        }
        public void DIV(ref short a, ref short b) {
            a /= b;
        }
        public void MOD(ref short a, ref short b) {
            a %= b;
        }
        public void MOV(ref short a, ref short b)
        {
            a = b;
        }
    }
}


// 0000 000 00000000 0
// OP    R   R/Val   S
