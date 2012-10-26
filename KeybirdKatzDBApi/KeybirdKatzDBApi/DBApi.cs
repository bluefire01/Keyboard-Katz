using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace KeybirdKatzDBApi
{
    public class DBApi
    {
        private static readonly DBApi instance = new DBApi();

        static private KeybirdKatzDataSetTableAdapters.PlayerTableAdapter playerTableAdapter;
        static private KeybirdKatzDataSetTableAdapters.PlayerSummaryTableAdapter playerSummaryTableAdapter;
        static private KeybirdKatzDataSet kkDataSet;

        public long addPlayer(String playerName)
        {
            playerTableAdapter.Fill(kkDataSet.Player);
            Random random = new Random();
            long key = random.Next();
            try
            {
                key = random.Next();
                kkDataSet.Player.AddPlayerRow(key, playerName);
                playerTableAdapter.Update(kkDataSet);
                //System.Console.Out.WriteLine("Added Player {0} {1}", key, playerName);
                return key;
            }
            catch (System.Data.ConstraintException c)
            {
                System.Console.Out.WriteLine(c.Message);
                return addPlayer(playerName);
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.Message);
                return -1;
            }
        }

        public bool deletePlayer(long playerID)
        {
            try
            {
                KeybirdKatzDataSet.PlayerRow row = kkDataSet.Player.FindByPlayerID(playerID);
                //mark row for deletion
                row.Delete();
                //commit change
                playerTableAdapter.Update(kkDataSet);
                return true;
            }
            catch (Exception e)
            {
                System.Console.Out.WriteLine(e.Message);
                return false;
            }
        }

        public void deleteAllPlayers()
        {
            int i = 0;
            while (i < kkDataSet.Player.Rows.Count)
            {
                //mark row for deletion
                kkDataSet.Player.Rows[i].Delete();
                i++;
            }
            //commit changes
            playerTableAdapter.Update(kkDataSet);

        }

        public void changePlayerName(long playerID, String newName)
        {
            KeybirdKatzDataSet.PlayerRow row = kkDataSet.Player.FindByPlayerID(playerID);
            //System.Console.Out.WriteLine(row[1]);
            //column 0 is PlayerID, column 1 is PlayerName
            row.PlayerName = newName;
            playerTableAdapter.Update(kkDataSet);
        }

        public String getPlayerName(long playerID)
        {
            KeybirdKatzDataSet.PlayerRow row = kkDataSet.Player.FindByPlayerID(playerID);
            return row.PlayerName;
        }

        private DBApi()
        {
            
            playerTableAdapter = new KeybirdKatzDataSetTableAdapters.PlayerTableAdapter();
            playerSummaryTableAdapter = new KeybirdKatzDataSetTableAdapters.PlayerSummaryTableAdapter();
            kkDataSet = new KeybirdKatzDataSet();
            playerTableAdapter.Fill(kkDataSet.Player);
        }

        public static DBApi Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
