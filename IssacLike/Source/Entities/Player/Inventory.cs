using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeldaLike.Source.Entities.Items;

namespace ZeldaLike.Source.Entities.Player {
    public class Inventory {

        public Dictionary<string, bool> CollectedItems = new Dictionary<string, bool>();

        public Inventory() { }

        public bool BasicSwordCollected = false;
        public bool SignificantItemCollected = false;

        private int m_Position;
        private int m_Width;
        private int m_Height;
    }
}
