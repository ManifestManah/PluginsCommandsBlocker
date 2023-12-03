/////////////////
// - IMPORTS - //
/////////////////

// C# Specific imports
using System;

// CSSharp specific imports
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

// Required for being able to use attributes for commands and defining minimum api versions
using CounterStrikeSharp.API.Core.Attributes;

// Required for being able to check a users administrator permissions
using CounterStrikeSharp.API.Modules.Admin;

// Required for being able to use attribute based command setup
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Attributes.Registration;

// Required for using chat colors in messages amongst many other things
using CounterStrikeSharp.API.Modules.Utils;


// Specifies the namespace of the plugin, this should match the name of the plugin file
namespace PluginCommandsBlocker;

// Defines the minimum version of CounterStrikeSharp required in order to run this plugin on the server 
[MinimumApiVersion(96)]

// Specifies our main class, this should match the name of the namespace
public class PluginCommandsBlocker : BasePlugin
{
    // The retrievable information about the plugin itself
    public override string ModuleName => "[Custom] Plugin Commands Blocker";
    public override string ModuleAuthor => "Manifest @Road To Glory";
    public override string ModuleDescription => "Prevents players from accessing your server's plugin information through the player console.";
    public override string ModuleVersion => "V. 1.0.1 [Beta]";


    // Multi-colored clan tag and predefined chat colors
    private string ChatPrefix = $"[{ChatColors.Lime}Server{ChatColors.White}]";
    private string ChatColorHighlight = $"{ChatColors.Lime}";
    private string ChatColorNormal = $"{ChatColors.White}";


    // This happens when the plugin is loaded
    public override void Load(bool hotReload)
    {
        // Adds a command listener to track the usage of the specified command
        AddCommandListener("sm", CommandListener_BlockOutput);
        AddCommandListener("meta", CommandListener_BlockOutput);
        AddCommandListener("css_plugins", CommandListener_BlockOutput);
    }


    // This happens when the meta command is being used 
    private HookResult CommandListener_BlockOutput(CCSPlayerController? player, CommandInfo info)
    {
        // If the CCSPlayerController is null then execute this section
        if (player == null)
        {
            return HookResult.Continue;
        }

        // If the the CCSPlayerController entity is invalid by pointing to a null pointer then execute this section
        if (!player.IsValid)
        {
            return HookResult.Continue;
        }

        // If the pawn associated with the CCSPlayerController is invalid then execute this section
        if (!player.PlayerPawn.IsValid)
        {
            return HookResult.Continue;
        }

        // If the player has root administrator permissions then execute this section
        if (AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            return HookResult.Continue;
        }

        // Sends a message in the player's chat area which can only be seen by that specific player
        player.PrintToChat($"{ChatPrefix} The {ChatColorHighlight}plugin commands{ChatColorNormal} are not accessible to the public.");
        player.PrintToChat($"- Contact the {ChatColorHighlight}server's staff team{ChatColorNormal} if you have any questions.");

        return HookResult.Stop;
    }
}
