
using System.Collections.Generic;
using Terraria.ModLoader;
using BlackOps3.Content.Items.Weapons;
using BlackOps3.Content.Items.Weapons.BO3.Pistols;

namespace BlackOps3.Content.Systems
{
    public class PackAPunchProcesses : GlobalItem
    {
        public static readonly int Duration = 240;
        public static readonly int Timeout = 480;
        public class PunchProcess {
            public int Input;
            public int Output;
            
        }
        public static List<PunchProcess> PunchUpgrades;
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