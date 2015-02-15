namespace Yahtzee.Framework
{
	public interface IDie
	{
		DieState State { get; set; }
		int Value { get; }
		int Roll();
	}
}