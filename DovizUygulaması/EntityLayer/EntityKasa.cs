using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class EntityKasa
    {
        private double dolar;
        private double euro;
        private double pound;
        private double japonyen;
        private double turkLıra;

        public double Dolar { get => dolar; set => dolar = value; }
        public double Euro { get => euro; set => euro = value; }
        public double Pound { get => pound; set => pound = value; }
        public double Japonyen { get => japonyen; set => japonyen = value; }
        public double TurkLıra { get => turkLıra; set => turkLıra = value; }
    }
}
