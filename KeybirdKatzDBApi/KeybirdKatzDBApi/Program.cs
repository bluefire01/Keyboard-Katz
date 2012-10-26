using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace KeybirdKatzDBApi
{
    class Program
    {
        private static DBApi dbApi;
        private static Random random = new Random(); 
        List<long> userKeys;


        static void Main(string[] args)
        {
            dbApi = DBApi.Instance;
            Program prgm = new Program();
            prgm.userKeys = new List<long>();

            dbApi.deleteAllPlayers();

            for (int i = 0; i < 10; i++)
            {
                long nKey = dbApi.addPlayer(random.Next(10).ToString());
                System.Console.Out.WriteLine(nKey);
                prgm.userKeys.Add(nKey);
            }

            String theName = "Mike";
            System.Console.Out.WriteLine("Change player name {0} with name {1} to {2}", prgm.userKeys[0], dbApi.getPlayerName(prgm.userKeys[0]), theName);
            dbApi.changePlayerName(prgm.userKeys[0], theName);
            
            System.Console.Out.WriteLine("Delete Key {0}", prgm.userKeys[1]);
            dbApi.deletePlayer(prgm.userKeys[1]);
            dbApi.deleteAllPlayers();
        }
    }
}
