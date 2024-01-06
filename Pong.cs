using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Pong
    {
        int width;
        short width_short;
        short height_short;
        int height;
        int paddleLength;
        bool isBallGoingDown;
        bool isBallGoingRight;
        int scoreboardX;
        int scoreboardY;

        const char paddleTile = '|';
        const char ballTile = 'O';
        Board board;
        public Pong(int width, int height)
        {
            this.width = width;
            this.height = height;

            paddleLength = height / 4;
            board = new Board(width, height);

            isBallGoingDown = true;
            isBallGoingRight = true;

            scoreboardX = width / 3;
            scoreboardY = height + 2;
        }

        public void Initialization(ref short PC, ref Memory mem, ref short[] R)
        {
            width_short = (short)(width & 0b11111111);
            height_short = (short)(height & 0b11111111);

            //store the initial position on X of the ball
            mem.StoreInstruction((short)((0b0111 << 12) | (0b010 << 9) | width_short | 0b0), 0);
            //store the initial position on Y of the ball
            mem.StoreInstruction((short)((0b0111 << 12) | (0b011 << 9) | height_short | 0b0), 1);
        }


        public void Run(ref short PC, ref Memory mem, ref short[] R)
        {
            board.Write();
            for (int i = 0; i < paddleLength; i++)
            {
                Console.SetCursorPosition(3, i + 1 + R[0]);
                Console.WriteLine(paddleTile);
                Console.SetCursorPosition(width - 1, i + 1 + R[1]);
                Console.WriteLine(paddleTile);
            }

            while(!Console.KeyAvailable)
            {
                Console.SetCursorPosition(R[2], R[3]);
                Console.WriteLine(ballTile);
                Thread.Sleep(100);

                Console.SetCursorPosition(R[2], R[3]);
                Console.WriteLine(" ");

                Console.SetCursorPosition(scoreboardX, scoreboardY);
                Console.WriteLine($"{R[4]} | {R[5]}");

                if(!isBallGoingDown && !isBallGoingRight)
                { 
                    mem.StoreInstruction(unchecked((short)0b1100000000000000), PC);
                }

                if(!isBallGoingDown && isBallGoingRight)
                {
                    mem.StoreInstruction(unchecked((short)0b1101000000000000), PC);
                }

                if(isBallGoingDown && !isBallGoingRight)
                {
                    mem.StoreInstruction(unchecked((short)0b1110000000000000), PC);
                }

                if(isBallGoingDown && isBallGoingRight)
                {
                    mem.StoreInstruction(unchecked((short)0b1111000000000000), PC);
                }

                if (R[3] == 2)
                {
                    isBallGoingDown = true;
                }
                else if (R[3] == height - 2)
                {
                    isBallGoingDown = false;
                }

                if (R[2] == 5)
                {
                    if (R[3] >= R[0] + 1 && R[3] <= R[0] + paddleLength) 
                    {
                        isBallGoingRight = true;
                    }
                    else
                    {
                        mem.StoreInstruction(unchecked((short)0b1011000000000000), PC);
                        if (R[5] == 2)
                        {
                            goto outer;
                        }
                    }
                }

                if (R[2] == width - 3)
                {
                    if (R[3] >= R[1] + 1 && R[3] <= R[1] + paddleLength)
                    {
                        isBallGoingRight = false;
                    }
                    else
                    {
                        mem.StoreInstruction(unchecked((short)0b1010000000000000), PC);
                        if (R[4] == 2)
                        {
                            goto outer;
                        }
                    }
                }
                return;

                outer:;
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);

                    if (R[5] + 1 == 3)
                    {
                        Console.WriteLine("Right player won!");
                        mem.StoreInstruction(0b0000000000000000, PC);
                    }
                    else if (R[4] + 1 == 3)
                    {
                        Console.WriteLine("Left player won!");
                        mem.StoreInstruction(0b0000000000000000, PC);
                    }
            }

            switch(Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    if (R[0] > 0)
                        mem.StoreInstruction(0b0011000000000010, PC);
                    else
                        mem.StoreInstruction(0b0011000000000000, PC);
                    break;
                case ConsoleKey.S:
                    if (R[0] < height - paddleLength - 1)
                        mem.StoreInstruction(0b0010000000000010, PC);
                    else
                        mem.StoreInstruction(0b0010000000000000, PC);
                    break;
                case ConsoleKey.UpArrow:
                    if (R[1] > 0 )
                        mem.StoreInstruction(0b0011001000000010, PC);
                    else
                        mem.StoreInstruction(0b0011001000000000, PC);
                    break;
                case ConsoleKey.DownArrow:
                    if (R[1] < height - paddleLength - 1)
                        mem.StoreInstruction(0b0010001000000010, PC);
                    else
                        mem.StoreInstruction(0b0010001000000000, PC);
                    break;
                case ConsoleKey.Escape:
                    mem.StoreInstruction(0b0000000000000000, PC);
                    break;
            }

            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(3, i);
                Console.WriteLine(" ");
                Console.SetCursorPosition(width - 1, i);
                Console.WriteLine(" ");
            }

        }

        public class Board
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public Board()
            {
                Height = 20;
                Width = 60;
            }

            public Board(int width, int height)
            {
                Height = height;
                Width = width;
            }

            public void Write()
            {
                for (int i = 1; i <= Width; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("─");
                }

                for (int i = 1; i <= Width; i++)
                {
                    Console.SetCursorPosition(i, (Height + 1));
                    Console.Write("─");
                }

                for (int i = 1; i <= Height; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("│");
                }

                for (int i = 1; i <= Height; i++)
                {
                    Console.SetCursorPosition((Width + 1), i);
                    Console.Write("│");
                }

                Console.SetCursorPosition(0, 0);
                Console.Write("┌");
                Console.SetCursorPosition(Width + 1, 0);
                Console.Write("┐");
                Console.SetCursorPosition(0, Height + 1);
                Console.Write("└");
                Console.SetCursorPosition(Width + 1, Height + 1);
                Console.Write("┘");
            }

        }
    }
}
