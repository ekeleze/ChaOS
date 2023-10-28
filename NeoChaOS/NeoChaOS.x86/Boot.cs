using Mosa.Kernel.BareMetal;

namespace NeoChaOS.x86
{
	public static class Boot
	{
		public static void Main()
		{
			Debug.WriteLine("Boot::Main()");
			Debug.WriteLine("NeoChaOS Kernel");

			Program.EntryPoint();
		}
	}
}
