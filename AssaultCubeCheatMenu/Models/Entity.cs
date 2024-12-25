using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AssaultCubeCheatMenu.Models
{
    public class Entity
    {
        public IntPtr Address { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public float Health { get; set; }
        public AmmoEntity Ammos { get; set; }
        public int Team { get; set; }
        public float Distance { get; set; }
    }
}
