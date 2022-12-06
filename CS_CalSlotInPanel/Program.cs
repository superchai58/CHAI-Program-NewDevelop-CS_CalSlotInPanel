using Connect.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_CalSlotInPanel
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectDBQSMS oCon = new ConnectDBQSMS();
            ConnectDBSMT oConSMT = new ConnectDBSMT();
            //--Loop SN from TempResult--
            DataTable dt2 = new DataTable();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = "Select * From TempSNTest with(nolock)";
            cmd3.CommandTimeout = 1000;
            dt2 = oCon.Query(cmd3);

            DataTable dt3 = new DataTable();
            SqlCommand cmd4 = new SqlCommand();
            SqlCommand cmd5 = new SqlCommand();
            SqlCommand cmd6 = new SqlCommand();
            if (dt2.Rows.Count > 0)
            {
                //--Get AOI from SN_AOI--
                foreach (DataRow row in dt2.Rows)
                {
                    cmd4 = new SqlCommand();
                    oConSMT = new ConnectDBSMT();
                    dt3 = new DataTable();
                    cmd4.CommandText = "Select SN, AOI From SN_AOI with(nolock) Where SN = '" + row["SN"].ToString().Trim() + "' UNION ALL Select SN, AOI From SMTHistory..SN_AOI with(nolock) Where SN = '" + row["SN"].ToString().Trim() + "'";
                    cmd4.CommandTimeout = 180;
                    dt3 = oConSMT.Query(cmd4);

                    if (dt3.Rows.Count > 0)
                    {
                        //--Get AOI--
                        SqlCommand cmd7 = new SqlCommand();
                        DataTable dt4 = new DataTable();
                        cmd7.CommandText = "Select SN, AOI From SN_AOI with(nolock) Where AOI = '" + dt3.Rows[0]["AOI"].ToString().Trim() + "' UNION ALL Select SN, AOI From SMTHistory..SN_AOI with(nolock) Where AOI = '" + dt3.Rows[0]["AOI"].ToString().Trim() + "'";
                        cmd7.CommandTimeout = 180;
                        dt4 = oConSMT.Query(cmd7);

                        if (dt4.Rows.Count > 0)
                        {
                            int i = 1;
                            foreach (DataRow row1 in dt4.Rows)
                            {
                                if (row1["SN"].ToString().Trim() == row["SN"].ToString().Trim())
                                {
                                    if (i == 1 || i == 3 || i == 6 || i == 8 || i == 9 || i == 11)
                                    {
                                        try
                                        {
                                            //--Update slotInPanel = 2--
                                            oCon = new ConnectDBQSMS();
                                            cmd5 = new SqlCommand();
                                            cmd5.CommandText = "Update TempSNTest set Slot = '2' Where SN = '" + row["SN"].ToString().Trim() + "' ";
                                            cmd5.CommandTimeout = 180;
                                            oCon.ExecuteCommand(cmd5);
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            //--Update slotInPanel = 1--
                                            oCon = new ConnectDBQSMS();
                                            cmd6 = new SqlCommand();
                                            cmd6.CommandText = "Update TempSNTest set Slot = '1' Where SN = '" + row["SN"].ToString().Trim() + "' ";
                                            cmd6.CommandTimeout = 180;
                                            oCon.ExecuteCommand(cmd6);
                                        }
                                        catch (Exception ex)
                                        { }
                                    }
                                }
                                i++;
                            }
                        }
                    }
                }
            }
        }
    }
}
