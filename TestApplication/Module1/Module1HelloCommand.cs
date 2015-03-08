namespace TestApplication.Module1
{
	public class Module1HelloCommand : ShellHelloCommand
	{

		protected override string GetMessagePrefix()
		{
			return "Hello from Module-1";
		}
	}
}
