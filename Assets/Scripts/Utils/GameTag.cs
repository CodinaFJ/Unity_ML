
using System.Collections.Generic;

public static class GameTag
{
	public const string UNTAGGED 		= "Untagged";
	public const string PLAYER			= "Player";
	public const string REWARD			= "Reward";
	public const string PUNISH			= "Punish";
	public const string GROUND			= "Ground";

	private static readonly List<string> gameTags = new ();

	static GameTag()
	{
		gameTags.Add(UNTAGGED);
		gameTags.Add(PLAYER);
		gameTags.Add(REWARD);
		gameTags.Add(PUNISH);
		gameTags.Add(GROUND);
	}

	public static int 	GetGameTagId(string tag)
	{
		return gameTags.IndexOf(tag);
	}
}