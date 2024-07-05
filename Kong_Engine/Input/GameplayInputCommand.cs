using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kong_Engine.Input.Base;

namespace Kong_Engine.Input
{
    public static class GameplayInputCommand
    {
        public class GameExit : BaseInputCommand { }
        public class PlayerMoveLeft : BaseInputCommand { }
        public class PlayerMoveRight : BaseInputCommand { }
        public class PlayerMoveDown : BaseInputCommand { }
        public class PlayerMoveUp : BaseInputCommand { }
    }
}
