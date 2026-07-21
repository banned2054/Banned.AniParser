using Banned.AniParser;

const string filename = "[ANi]BanG Dream! Ave Mujica - 01 [1080P][Baha][WEB-DL][AAC AVC][CHT].mp4";

var result = new AniParser().Parse(filename);

if (result?.Title != "BanG Dream! Ave Mujica" || result.Episode != 1)
    throw new InvalidOperationException("NativeAOT parsing failed.");

Console.WriteLine("NativeAOT smoke test passed.");
