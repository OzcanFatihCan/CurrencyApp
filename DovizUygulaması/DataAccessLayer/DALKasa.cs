using EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DALKasa
    {
        public static List<EntityKasa> KasaGetir()
        {
            List<EntityKasa> Kasa=new List<EntityKasa>();
            SqlCommand komut1 = new SqlCommand("SELECT * FROM TBLDOVIZKASA",Baglanti.conn);
            if (komut1.Connection.State != ConnectionState.Open)
            {
                komut1.Connection.Open();
            }
            SqlDataReader dr=komut1.ExecuteReader();
            while (dr.Read())
            {
                EntityKasa ent=new EntityKasa();
                ent.Dolar = int.Parse(dr["DOLAR"].ToString());
                ent.Euro = int.Parse(dr["EURO"].ToString());
                ent.Pound = int.Parse(dr["POUND"].ToString());
                ent.Japonyen = int.Parse(dr["JAPONYEN"].ToString());
                ent.TurkLıra= int.Parse(dr["TURKLIRA"].ToString());
                Kasa.Add(ent);
            }
            dr.Close();
            return Kasa;    
        }
        
        public static bool KasaGuncelle(EntityKasa ent)
        {
            try
            {
                SqlCommand komut2 = new SqlCommand("UPDATE TBLKASA SET " +
                    "DOLAR = DOLAR + @Dolar, " +
                    "EURO = EURO + @Euro, " +
                    "POUND = POUND + @Pound, " +
                    "TURKLIRA = TURKLIRA + @TurkLira, " +
                    "JAPONYEN = JAPONYEN + @JaponYen " +
                    "WHERE KASAID = 1", Baglanti.conn);

                komut2.Parameters.AddWithValue("@Dolar", ent.Dolar);
                komut2.Parameters.AddWithValue("@Euro", ent.Euro);
                komut2.Parameters.AddWithValue("@Pound", ent.Pound);
                komut2.Parameters.AddWithValue("@TurkLira", ent.TurkLıra);
                komut2.Parameters.AddWithValue("@JaponYen", ent.Japonyen);

                if (komut2.Connection.State != ConnectionState.Open)
                {
                    komut2.Connection.Open();
                }

                int etkilenenSatir = komut2.ExecuteNonQuery();
                return etkilenenSatir > 0;
            }
            catch (Exception ex)
            {
                // Hata durumunda gerekli işlemleri yapabilirsiniz.
                return false;
            }
        }
    }
}
