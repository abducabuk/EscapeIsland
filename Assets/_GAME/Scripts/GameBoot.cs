using CbkSDK.Core.ServiceLocator.Attribute;
using UnityEngine;

namespace _GAME.Scripts
{
    [BootGame]
    public class GameBoot
    {
        public GameBoot()
        {
            Application.targetFrameRate = 60;
            // var commands = new CommandChain();
            //  // var commands = new CommandGroup();
            //  commands.Execute();
        }
    }
}