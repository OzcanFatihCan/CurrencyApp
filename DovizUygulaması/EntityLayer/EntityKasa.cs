using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class EntityKasa
    {
        private int dolar;
        private int euro;
        private int pound;
        private int japonyen;
        private int turkLıra;
        public int Dolar { get => dolar; set => dolar = value; }
        public int Euro { get => euro; set => euro = value; }
        public int Pound { get => pound; set => pound = value; }
        public int Japonyen { get => japonyen; set => japonyen = value; }
        public int TurkLıra { get => turkLıra; set => turkLıra = value; }
    }
}
