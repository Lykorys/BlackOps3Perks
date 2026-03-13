
using System.Collections.Generic;
using Terraria.ModLoader;
using BoneTest.Content.Items.Weapons;

namespace BoneTest.Content.Utils.Functions
{
    public class PackAPunchProcesses : GlobalItem
    {
        public static readonly int Duration = 240;
        public static readonly int Timeout = 480;
        public class PunchProcess {
            public int Input;
            public int Output;
            
        }

        // 1. Declare the list as static, but don't initialize it yet
        public static List<PunchProcess> PunchUpgrades;

        // 2. Use a static constructor to initialize it
        static PackAPunchProcesses() 
        {
            PunchUpgrades = new List<PunchProcess>()
            {
                new PunchProcess { 
                    Input = ModContent.ItemType<MR6>(), 
                    Output = ModContent.ItemType<RK5>() 
                },
                new PunchProcess { 
                    Input = ModContent.ItemType<RK5>(), 
                    Output = ModContent.ItemType<MR6>() 
                }
            };
        }
        

    };
}