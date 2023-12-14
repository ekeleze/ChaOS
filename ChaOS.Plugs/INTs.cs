using IL2CPU.API.Attribs;
using static Cosmos.Core.INTs;
using static System.ConsoleColor;

[Plug(Target = typeof(Cosmos.Core.INTs))]
internal class INTs
{
    public static void HandleException(uint eip, string desc, string name, ref IRQContext context, uint lastKnownAddressValue = 0)
    {
        ChaOS.Kernel.Screen.IsEnabled = false;

        string Message = "STOP: 0x" + eip.ToString("X") + " " + desc + " at 0x" + lastKnownAddressValue.ToString("X");

        Console.BackgroundColor = Blue; Console.ForegroundColor = White;
        Console.Clear();
        Console.CursorTop = 10; Console.WriteLine("              ChaOS has hit a brick wall and died in the wreckage!\n");
        Console.CursorLeft = 39 - (Message.Length / 2);
        Console.Write(Message + "\n\n");

        Console.Write("                                You can restart. ");
        while (true) Console.ReadKey(true);
    }
}
