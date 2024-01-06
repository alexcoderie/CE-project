namespace Project
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            //Initialize the memory
            Memory mem = new Memory();
            mem.Init();
            //mem.ViewMem();


            //Initialize the CPU
            CPU cpu = new CPU(ref mem);
            //cpu.ViewCPU();
            //cpu.ViewMem(mem);

            cpu.RUN(mem);
            //cpu.ViewMem(mem);
            //cpu.ViewCPU();

            /*
            Console.CursorVisible = false;
            Pong pong = new Pong(60, 20);
            pong.Run(0, mem);
            Console.ReadKey();
            */
            
        }
    }
}