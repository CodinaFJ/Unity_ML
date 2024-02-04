
using System.Collections.Generic;

public static class GameTag
{
	public const string UNTAGGED 		= "Untagged";
	public const string PLAYER			= "Player";
	public const string REWARD			= "Reward";
	public const string PUNISH			= "Punish";
	public const string GROUND			= "Ground";
	public const string SPAWN			= "Spawn";
	public const string END			= "End";

	private static readonly List<string> gameTags = new(){
		UNTAGGED,
		PLAYER,
		REWARD,
		PUNISH,
		GROUND,
		SPAWN,
		END
	};

	public static int 	GetGameTagId(string tag)
	{
		return gameTags.IndexOf(tag);
	}
}

public enum Tag
{
	Untagged,
	Player,
	Reward,
	Punish,
	Ground,
	Spawn,
	End
}