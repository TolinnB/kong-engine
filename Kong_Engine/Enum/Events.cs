using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kong_Engine.Enum
{
    /// <summary>
    /// This is just a list of starter events we think the engine will need.
    /// 
    /// TODO - Integrate into each state
    /// </summary>
    public enum Events
    {
        // Game events that start, end or restart a game
        GAME_START,
        GAME_QUIT,
        GAME_PAUSE,
        GAME_RESUME,
        GAME_OVER,
        GAME_SAVE,
        GAME_LOAD,
        GAME_RESET,

        // Level events that start, or end, or restart a level
        LEVEL_START,
        LEVEL_COMPLETE,
        LEVEL_FAIL,
        LEVEL_RESTART,
        SCENE_LOAD,
        SCENE_UNLOAD,

        // Placeholder actions for the player
        PLAYER_MOVE,
        PLAYER_JUMP,
        PLAYER_ATTACK,
        PLAYER_DIE,
        PLAYER_RESPAWN,
        PLAYER_DAMAGE,

        // Placeholder actions for the player
        ENEMY_MOVE,
        ENEMY_ATTACK,
        ENEMY_DIE,
        ENEMY_TAKE_DAMAGE,

        // Placeholder actions for the environment
        TRIGGER_ENTER,
        TRIGGER_EXIT,
        PLATFORM_MOVE,
        CHECKPOINT_REACHED,

        // UI Events
        UI_BUTTON_CLICK,
        UI_SHOW,
        UI_HIDE,
        UI_UPDATE,

        // TODO- Alex do we need more of these?
        // Input Events
        INPUT_KEY_DOWN,
        INPUT_KEY_UP,
        INPUT_MOUSE_CLICK,
        INPUT_MOUSE_MOVE,

        // Audio Events
        AUDIO_PLAY,
        AUDIO_PAUSE,
        AUDIO_STOP,
        AUDIO_VOLUME_CHANGE,
        AUDIO_LOOP,

        // Resource Management Events
        RESOURCE_LOAD,
        RESOURCE_UNLOAD,
        RESOURCE_RELOAD,
    }
}
