using Nickel;
using System.Collections.Generic;

namespace Flipbop.Cleo;

internal sealed class EventDialogue : BaseDialogue
{
	public EventDialogue() : base(locale => ModEntry.Instance.Package.PackageRoot.GetRelativeFile($"i18n/dialogue-event-{locale}.json").OpenRead())
	{
		var cleoDeck = ModEntry.Instance.CleoDeck.Deck;
		var cleoType = ModEntry.Instance.CleoCharacter.CharacterType;
		var newNodes = new Dictionary<IReadOnlyList<string>, StoryNode>();
		var newHardcodedNodes = new Dictionary<IReadOnlyList<string>, StoryNode>();
		var saySwitchNodes = new Dictionary<IReadOnlyList<string>, Say>();

		ModEntry.Instance.Helper.Events.OnModLoadPhaseFinished += (_, phase) =>
		{
			if (phase != ModLoadPhase.AfterDbInit)
				return;
			InjectStory(newNodes, newHardcodedNodes, saySwitchNodes, NodeType.@event);
		};
		ModEntry.Instance.Helper.Events.OnLoadStringsForLocale += (_, e) => InjectLocalizations(newNodes, newHardcodedNodes, saySwitchNodes, e);

		newNodes[["Shop", "0"]] = new()
		{
			lookup = ["shopBefore"],
			bg = typeof(BGShop).Name,
			allPresent = [cleoType],
			lines = [
				new Say { who = cleoType, loopTag = "neutral" },
				new Jump() { key = "NewShop" }
			],
		};
		newNodes[["Shop", "1"]] = new()
		{
			lookup = ["shopBefore"],
			bg = typeof(BGShop).Name,
			allPresent = [cleoType],
			lines = [
				new Say { who = cleoType, loopTag = "neutral" },
				new Jump() { key = "NewShop" }
			],
		};

		newHardcodedNodes[["LoseCharacterCard_{{CharacterType}}"]] = new()
		{
			oncePerRun = true,
			bg = typeof(BGSupernova).Name,
			allPresent = [cleoType],
			lines = [
				new Say { who = cleoType, loopTag = "neutral" },
			],
		};
		newHardcodedNodes[["CrystallizedFriendEvent_{{CharacterType}}"]] = new()
		{
			oncePerRun = true,
			bg = typeof(BGCrystalizedFriend).Name,
			allPresent = [cleoType],
			lines = [
				new Wait() { secs = 1.5 },
				new Say { who = cleoType, loopTag = "neutral" },
			],
		};
		newHardcodedNodes[["ChoiceCardRewardOfYourColorChoice_{{CharacterType}}"]] = new()
		{
			oncePerRun = true,
			bg = typeof(BGBootSequence).Name,
			allPresent = [cleoType],
			lines = [
				new Say { who = cleoType, loopTag = "squint" },
				new Say { who = "comp", loopTag = "neutral" },
			],
		};

		saySwitchNodes[["GrandmaShop"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
		saySwitchNodes[["LoseCharacterCard"]] = new()
		{
			who = cleoType,
			loopTag = "nervous"
		};
		saySwitchNodes[["CrystallizedFriendEvent"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
		saySwitchNodes[["ShopKeepBattleInsult"]] = new()
		{
			who = cleoType,
			loopTag = "nervous"
		};
		saySwitchNodes[["DraculaTime"]] = new()
		{
			who = cleoType,
			loopTag = "squint"
		};
		saySwitchNodes[["Soggins_Infinite"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
		saySwitchNodes[["Soggins_Missile_Shout_1"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
		saySwitchNodes[["SogginsEscapeIntent_1"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
		saySwitchNodes[["SogginsEscape_1"]] = new()
		{
			who = cleoType,
			loopTag = "neutral"
		};
	}
}
