#r "nuget:Newtonsoft.Json/12.0.2"

/*
SS1 - 120,785
SS2 - 160,905
SS3 - 200,1025

C1 - 260,1180
C2 - 445,1180
C3 - 630,1180

TOP    - 445,840
CENTER - 445,920
BOTTOM - 445,1000
LEFT   - 365,920
RIGHT  - 525,920
*/

/**
possible commands I found:

KBDPR:158:0 - press Back key
KBDRL:158:0 - release Back
KBDPR:102:0 - same for Home
KBDRL:102:0
KBDPR:221:0 - 'recent apps' button
KBDRL:221:0
MULTI:1:0:xx:yy - touch down at xx,yy coordinates
MULTI:0:6 and MSBRL:1337814:-1072938 - two lines for touch up, never experimented if both are really needed, just copied from macro; numbers in MSBRL are always the same, they are not coordinates
MULTI:1:2:xx:yy - used after touch down, this command swipes to xx,yy. Used repeatedly for short intervals, should be followed with touch up command for 'end of swipe'.

For myself, I made a python script to 'convert' simple macro format (commands like 'touch', 'swipe', 'delay') to Nox's gibberish. It works and is pretty simple to do, I just use an internal 'timer' variable and append it at end of nox strings for correct timings.

NOTE: Lance Attack Cancel works better with 50fps
**/

using Newtonsoft.Json;

public Character UNDEFINED;
public delegate void Character(int delay);
public static StringBuilder Macro = new StringBuilder();
public static int ScriptTime = 0;

public void StartScript() {
    Macro.Clear();
    ScriptTime = 0;    
}

public void StartScript(out Character unit1, out Character unit2, out Character unit3)
{
    StartScript();

    unit1 = Char1;
    unit2 = Char2;
    unit3 = Char3;
}

public void EndScript(string outputPath)
{
    File.WriteAllText(outputPath, Macro.ToString());
    $"ScriptTime: {ScriptTime}, TimeStamp: {DateTime.Now.ToString()}".Dump();
}

public void Tap(int delay, int count = 1, int speed = 20, int interval = 200, int x = 445, int y = 920)
{
    do
    {
        ScriptTime += delay;
        Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:{x}:{y}ScRiPtSePaRaToR{ScriptTime}");

        ScriptTime += speed;
        Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{ScriptTime}
");
        delay = interval;
    } while (--count > 0);

}

public void Swipe(int delay, int xoffset = 0, int yoffset = 0, int speed = 30)
{
    SwipeEx(445, 900, 445 + xoffset, 900 - yoffset, delay, speed);
}

public void SwipeEx(int x1, int y1, int x2, int y2, int delay, int speed = 30)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:{x1}:{y1}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += speed;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{x2}:{y2}ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{ScriptTime}
");
}

public void Jump(int delay, int direction = 0, int speed = 30)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:{445}:1000ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += speed;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{445 + direction}:840ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{ScriptTime}
");
}

public void Move(int delay, int duration, int xoffset = 0, int yoffset = 0)
{
    MoveEx(445, 900, 445 + xoffset, 900 - yoffset, delay, duration);
}

public void MoveEx(int x1, int y1, int x2, int y2, int delay, int duration)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:{x1}:{y1}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += 30;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{x2}:{y2}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += duration;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{ScriptTime}
");
}

public void BackSwitch(int delay, Character unit, int xoffset = 0, int yoffset = 0)
{
    var attr = unit.Method.GetCustomAttribute<PointAttribute>();
    BackSwitchEx(445, 900, 445 + xoffset, 900 - yoffset, attr.X, attr.Y, delay);
}

public void BackSwitchEx(int x1, int y1, int x2, int y2, int tapx, int tapy, int delay)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:{x1}:{y1}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += 30;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{x2}:{y2}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += 1000;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:2:5:{tapx}:{tapy}:{x2}:{y2}ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += 300;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:6:{x2}:{y2}ScRiPtSePaRaToR{ScriptTime}");

    var endTime = ScriptTime - 50;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{endTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{endTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{endTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{endTime}
");
}

public void CrossSkill(int delay, Character unit1, Character? unit2 = null, int switchDelay = 1500)
{
    if (unit2 == null)
    {
        SS3(delay);
        unit1(switchDelay);
    }
    else
    {
        unit1(delay);
        unit2(switchDelay);
    }
}

public void Parry(int delay, int duration, int x = 445)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:0:445:820ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += duration;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{x}:760ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime += 24;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:1:2:{x}:590ScRiPtSePaRaToR{ScriptTime}");

    ScriptTime++;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:6ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MULTI:0:1ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|MSBRL:0:0ScRiPtSePaRaToR{ScriptTime}
");
}

public enum Direction
{
    Left,
    Right
}

public void QuickLanceParry(int delay, Direction direction, Character? unit = null, int tCancel = 300)
{
    var xoffset = direction == Direction.Left ? -50 : 50;

    Swipe(delay, xoffset);
    Swipe(tCancel, yoffset: 50);
    Tap(16);
    if (unit == null) SS3(50);
    else unit(50);
}

public void FlickParry(int delay)
{
    Parry(delay, 240);
}

public void LeftParry(int delay, int duration)
{
    Parry(delay, duration, 365);
}

public void RightParry(int delay, int duration)
{
    Parry(delay, duration, 525);
}

public void SS1(int delay)
{
    Tap(delay, x: 120, y: 785);
}

public void SS2(int delay)
{
    Tap(delay, x: 160, y: 905);
}

public void SS3(int delay, int charge = 20)
{
    Tap(delay, speed: charge, x: 200, y: 1025);
}

[Point(260, 1180)]
public void Char1(int delay)
{
    Tap(delay, x: 260, y: 1180);
}

[Point(445, 1180)]
public void Char2(int delay)
{
    Tap(delay, x: 445, y: 1180);
}

[Point(630, 1180)]
public void Char3(int delay)
{
    Tap(delay, x: 630, y: 1180);
}

public void SwordAtkCancel(int delay, int hits, int idle = 56)
{
    //    Tap(delay);
    //    for (var i = 1; i < hits; ++i)
    //    {
    //        Swipe(210, yoffset: 100);
    //        Tap(48);
    //    }

    Tap(delay, speed: 10);
    ScriptTime += 10;
    for (var i = 1; i < hits; ++i)
    {
        Swipe(200, yoffset: 100, speed: 10);
        Tap(30, speed: 10);
        ScriptTime += idle;
    }
}

public void RapierAtkCancel(int delay, int hits, int idle = 5)
{
    Tap(delay);
    for (var i = 1; i < hits; ++i)
    {
        var xoffset = 0;//i % 2 == 0 ? 5 : -5;
        Swipe(172, xoffset, 100, 10);
        Tap(24, speed: 5);
        ScriptTime += idle;
    }
}


public void DaggerAtkCancel(int delay, int hits)
{
    Tap(delay);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(410, yoffset: 100, speed: 20);
        Tap(20, speed: 20);
    }

    //    Tap(delay, speed: 10);
    //    ScriptTime += 10;
    //    for (var i = 1; i < hits; ++i)
    //    {
    //        Swipe(200, yoffset: 100, speed: 10);
    //        Tap(22, speed: 10);
    //        //ScriptTime += 10;
    //    }
}

public void DualBladeAtkCancel(int delay, int hits)
{
    Tap(delay);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(290, yoffset: 100);
        Tap(21, speed: 16);
    }
}
public void LanceAtkCancel(int delay, int hits)
{
    Tap(delay, speed: 10);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(229, yoffset: 50, speed: 20);
        Tap(20, speed: 10);
    }
}

public void RodAtkCancel(int delay, int hits)  // not working
{
    Tap(delay);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(520, yoffset: 100, speed: 14);
        Tap(16, speed: 8);
    }
}

// Normal atk better
public void BowAtkCancel(int delay, int hits)
{
    Tap(delay);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(500, yoffset: 100, speed: 20);
        Tap(16, speed: 4);
    }
}

public void GunAtkCancel(int delay, int hits)
{
    Tap(delay);
    Tap(16);
    for (var i = 1; i < hits; ++i)
    {
        Swipe(600, yoffset: 100, speed: 20);
        Tap(16, speed: 2);
        Tap(20, speed: 2);
    }
}

private void KeyPress(int keyCode, int delay)
{
    ScriptTime += delay;
    Macro.AppendLine($@"0ScRiPtSePaRaToR720|1280|KBDPR:{keyCode}:0ScRiPtSePaRaToR{ScriptTime}
0ScRiPtSePaRaToR720|1280|KBDRL:{keyCode}:0ScRiPtSePaRaToR{ScriptTime}
");
}

public void BackButton(int delay)
{
    KeyPress(158, delay);
}

public void HomeButton(int delay)
{
    KeyPress(102, delay);
}

public void OverviewButton(int delay)
{
    KeyPress(221, delay);
}

public void Type(string text, int delay)
{
    ScriptTime += delay;
    foreach (var c in text)
    {
        Macro.AppendLine($"1ScRiPtSePaRaToR{c}|0ScRiPtSePaRaToR{ScriptTime}");
        ScriptTime += 200;
    }
}

public void SwitchAccount(string transferCode, string password)
{
    OverviewButton(500);
    SwipeEx(600, 1170, 200, 1170, 1000);
    SwipeEx(600, 1170, 200, 1170, 1000);

    Tap(2000, x: 470, y: 200);      // File Manager
    Tap(2000, x: 1240, y: 500);     // Bookmarks
    Tap(2000, x: 440, y: 190);      // Select 'com.bandainamcoent.saomdna'
    Tap(2000, x: 805, y: 30);       // Select 'shared_prefs'
    Tap(2000, x: 580, y: 200);      // Open 'files'
    Tap(2000, x: 370, y: 200);      // Open 'memories'
    Tap(2000, x: 220, y: 170);      // Open 'json'
    Tap(2000, x: 215, y: 30);       // Select 'app_data'
    Tap(2000, x: 1245, y: 40);      // Open File Manager Menu
    Tap(2000, x: 770, y: 170);      // Select 'Delete selection'
    Tap(2000, x: 740, y: 560);      // Select 'Yes' to confirm deletion

    HomeButton(1000);
    Tap(1500, x: 710, y: 635);      // Launch SAOMD
    Tap(10000, x: 860, y: 360);     // Confirm language
    Tap(13000, x: 850, y: 520);     // Accept policy
    Tap(1500, x: 910, y: 360);      // Confirm age warning
    Tap(4000, x: 1150, y: 660);     // Select 'Support'
    Tap(1000, x: 410, y: 410);      // Select 'Transfer Data'
    Tap(1000, x: 750, y: 380);      // Select 'Transfer ID'
    Tap(1000, x: 530, y: 360);      // Select 'Transfer code' field
    Tap(500, x: 750, y: 360);       // Select 'Password' field
    Tap(500, x: 530, y: 360);       // Select 'Transfer code' field
    Type(transferCode, 500);        // Input transfer code
    Tap(1000, x: 750, y: 360);      // Select 'Password' field
    Type(password, 500);            // Input password
    Tap(500, x: 850, y: 360);       // Confirm transfer code input
    Tap(2000, x: 750, y: 360);      // Confirm transfer
    Tap(1000, x: 820, y: 360);      // Confirm transfer complete
    Tap(11000, x: 850, y: 520);     // Accept policy
    Tap(1500, x: 910, y: 360);      // Confirm age warning
    Tap(360, 640, 3000, 10);        // Link start!
}

private static readonly string NoxPath = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "Nox");
private static readonly string MacroFolderName = "record";
private static readonly string MacroDefFile = "records";

public string CreateNoxMacro(string name, int interval = 0, int mode = 0, int repeatCount = 1)
{
    var macros = GetNoxMacros();
    var key = macros.FirstOrDefault(kv => kv.Value.Name == name).Key;

    if (string.IsNullOrWhiteSpace(key))
    {
        key = Guid.NewGuid().ToString().Replace("-", string.Empty);
        var macro = new NoxMacro
        {
            Name = name,
            New = "false",
            PlaySet = new MacroConfig
            {
                Accelerator = "1",
                Interval = $"{interval}",
                Mode = $"{mode}",
                PlayOnStart = "false",
                PlaySeconds = "0#0#0",
                RepeatTimes = $"{repeatCount}",
                RestartPlayer = "false",
                RestartTime = "60"
            },
            Priority = "0",
            Time = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}"
        };

        foreach (var v in macros.Values)
        {
            var priority = int.Parse(v.Priority) + 1;
            v.Priority = $"{priority}";
        }

        macros.Add(key, macro);
        File.WriteAllText(Path.Combine(NoxPath, MacroFolderName, MacroDefFile), JsonConvert.SerializeObject(macros, Formatting.Indented));
    }

    var macroFilePath = Path.Combine(NoxPath, MacroFolderName, key);
    File.WriteAllText(macroFilePath, "");

    return macroFilePath;
}

public Dictionary<string, NoxMacro> GetNoxMacros()
{
    var json = File.ReadAllText(Path.Combine(NoxPath, MacroFolderName, MacroDefFile));
    return JsonConvert.DeserializeObject<Dictionary<string, NoxMacro>>(json);
}

public class PointAttribute : Attribute
{
    public PointAttribute(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}

public class NoxMacro
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("new")]
    public string New { get; set; }

    [JsonProperty("playSet")]
    public MacroConfig PlaySet { get; set; }

    [JsonProperty("priority")]
    public string Priority { get; set; }  // Order of macro in macro list;

    [JsonProperty("time")]
    public string Time { get; set; }       // Time since unix epoch in seconds
}

public class MacroConfig
{
    [JsonProperty("accelerator")]
    public string Accelerator { get; set; }    // Acceleration modifier

    [JsonProperty("interval")]
    public string Interval { get; set; }       // Interval between macro repeats

    [JsonProperty("mode")]
    public string Mode { get; set; }           // Loop mode - 0 (loop count), 1 (loop until stop), 2 (loop time)

    [JsonProperty("playOnStart")]
    public string PlayOnStart { get; set; }    // Start macro on emulator start

    [JsonProperty("playSeconds")]
    public string PlaySeconds { get; set; }    // Time config for 'loop time' in h#m#s format

    [JsonProperty("repeatTimes")]
    public string RepeatTimes { get; set; }    // The number of repeats for 'loop count'

    [JsonProperty("restartPlayer")]
    public string RestartPlayer { get; set; }  // Flag indicating the emulator will restart

    [JsonProperty("restartTime")]
    public string RestartTime { get; set; }    // The time in minutes when the emulator will restart if RestartPlayer is enabled 
}
