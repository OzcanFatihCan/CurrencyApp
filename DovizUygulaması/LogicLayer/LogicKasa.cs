using DataAccessLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class LogicKasa
    {
        public static List<EntityKasa> LLKasaGetir()
        {
            return DALKasa.KasaGetir();
        }

        public static bool LLKasaGuncelle(EntityKasa ent)
        {
            if (!string.IsNullOrEmpty(ent.Dolar.ToString()) &&
                !string.IsNullOrEmpty(ent.Euro.ToString()) &&
                !string.IsNullOrEmpty(ent.Pound.ToString()) &&
                !string.IsNullOrEmpty(ent.Japonyen.ToString()) &&
                !string.IsNullOrEmpty(ent.TurkLıra.ToString()))
            {
                return DALKasa.KasaGuncelle(ent);
            }
            else
            {
                return false;
            }
        }
    }
}
