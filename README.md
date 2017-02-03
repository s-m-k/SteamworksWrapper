# SteamworksWrapper
A very simple and specialized (but expandable) Steamworks API wrapper for Unity I'm gonna use for BUTCHER.

If you want to use it, use at your own risk. No support provided.

The only purpose of this wrapper is to write a simple and robust library that's no bullshit on the C# side.

Basic principles of the library:
- Keep it simple and safe - simple C-like API (on the native-managed boundary), no weird memory hacks etc.
- No bullshit - leave the SDK bullshit mess on the C++ side (e.g. no call results spaghetti in C# code, all of it is done in the native side)
- Expand it incrementally - do only what I actually need for my game, don't waste time on functionality I don't want to use
- Don't necessarily map Steam API directly to C# - I don't like some Valve decisions regarding some names (might break SDK docs compatibility, but it's for my own comfort not yours)
- Static C# classes - because Steam API is super singletonish and thinks it's a god surveiling your game, not gonna fight it

Leaderboards demo syntax (no Steamworks SDK callback mess):
```
try {
    testLeaderboard = Steam.Leaderboard.Create(); //make sure its lifespan is long enough for callback to be fired
                                                  //or the object might get GC-d and all requests might be just cancelled

    testLeaderboard.onFind += () => {
        Debug.Log("Loaded the leaderboard!");
        testLeaderboard.DownloadScores(LeaderboardDataRequest.GLOBAL, 0, 10);
    };

    testLeaderboard.onDownloadScores += (SteamWrapper.LeaderboardEntry[] entries) => {
        Debug.Log("Found the leaderboard!");
        for (int i = 0; i < entries.Length; i++) {
            Debug.Log(entries[i].steamID + " " + entries[i].score);
        }
    };

    testLeaderboard.Find("leaderboard name");
} catch (Exception) {
    Debug.LogError("Uh oh, Steamworks API isn't initialized.");
}
```

So far it only supports leaderboards in a limited way, achievements and some stats. You can also get a list of friends, friend names and your own ID. It's all very limited, but that's what I need for my game. Feel free to expand it.

It's WIP and not all of it has been tested yet, so again, use at your own risk.
