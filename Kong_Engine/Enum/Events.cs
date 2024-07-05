using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kong_Engine.Enum
{
    public enum Events
    {
        // Game Lifecycle Events
        GAME_START,
        GAME_PAUSE,
        GAME_RESUME,
        GAME_OVER,
        GAME_QUIT,
        GAME_SAVE,
        GAME_LOAD,
        GAME_RESET,

        // Level/Scene Events
        LEVEL_START,
        LEVEL_COMPLETE,
        LEVEL_FAIL,
        LEVEL_RESTART,
        SCENE_LOAD,
        SCENE_UNLOAD,

        // Player Actions
        PLAYER_MOVE,
        PLAYER_JUMP,
        PLAYER_ATTACK,
        PLAYER_DIE,
        PLAYER_RESPAWN,
        PLAYER_DAMAGE,
        PLAYER_HEAL,
        PLAYER_LEVEL_UP,
        PLAYER_INTERACT,

        // Enemy Actions
        ENEMY_SPAWN,
        ENEMY_MOVE,
        ENEMY_ATTACK,
        ENEMY_DIE,
        ENEMY_TAKE_DAMAGE,

        // Item Events
        ITEM_SPAWN,
        ITEM_PICKUP,
        ITEM_USE,
        ITEM_DESTROY,
        ITEM_DROP,
        ITEM_EXPIRE,

        // Environmental Events
        TRIGGER_ENTER,
        TRIGGER_EXIT,
        PLATFORM_MOVE,
        DOOR_OPEN,
        DOOR_CLOSE,
        CHECKPOINT_REACHED,

        // UI Events
        UI_BUTTON_CLICK,
        UI_SHOW,
        UI_HIDE,
        UI_UPDATE,

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
