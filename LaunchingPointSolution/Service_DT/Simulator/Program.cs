using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simulator.localhost;
using Simulator.testsp;
using Simulator.ameri007;
using Simulator.AMERI201;
using Simulator.AMGMO001;
using Simulator.AMIST001;
using Simulator.APPRO001;
using Simulator.ARCST001;
using Simulator.ASCEN001;
using Simulator.ASCEN002;
using Simulator.ASSET005;
using Simulator.BANKE010;
using Simulator.BANKE011;
using Simulator.BELEM001;
using Simulator.BRIGH001;
using Simulator.BUCKH001;
using Simulator.COMMU100;
using Simulator.CONTI001;
using Simulator.CORNE001;
using Simulator.CORNE010;
using Simulator.CRLHO001;
using Simulator.CROSS004;
using Simulator.DALLA001;
using Simulator.DVFGM001;
using Simulator.FARME007;
using Simulator.FLAGS002;
using Simulator.HERNA001;
using Simulator.HIGHP001;
using Simulator.HILLH001;
using Simulator.ICGHO001;
using Simulator.INVES003;
using Simulator.JMJFI001;
using Simulator.MERID001;
using Simulator.MONEY010;
using Simulator.MORTG022;
using Simulator.NSHPA001;
using Simulator.OAKTR001;
using Simulator.PARAM004;
using Simulator.PLANR001;
using Simulator.PLATI001;
using Simulator.PREFER001;
using Simulator.PREMI007;
using Simulator.PROFI001;
using Simulator.PURPO001;
using Simulator.REEVE001;
using Simulator.RELIA005;
using Simulator.ROBER002;
using Simulator.RUBIC001;
using Simulator.SCOTT001;
using Simulator.SETTL001;
using Simulator.SOUTH008;
using Simulator.SOUTH009;
using Simulator.SWANF001;
using Simulator.UNITE010;
using Simulator.VANTA003;
using Simulator.VELOC001;
using Simulator.World005;

namespace Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = @"C:\Z.TXT";
            System.IO.StreamWriter afile = new System.IO.StreamWriter(filepath);

            string str = "";

            //localhost.LP2ServiceClient localhostc = new localhost.LP2ServiceClient();
            //try
            //{
            //    str = "localhost   " + localhostc.GetVersion();

            //    Console.WriteLine(str);
            //    afile.WriteLine(str);
            //}
            //catch( Exception ex )
            //{
            //    str = str = "localhost   " + ex.Message;
            //    Console.WriteLine(str);
            //    afile.WriteLine(str);
            //}

            testsp.LP2ServiceClient testspc = new testsp.LP2ServiceClient();
            try
            {
            str = "testsp   " + testspc.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "testsp   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ameri007.LP2ServiceClient ameri007c = new ameri007.LP2ServiceClient();
            try
            {
            str = "ameri007   01   " + ameri007c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ameri007   01   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            AMERI201.LP2ServiceClient AMERI201c = new AMERI201.LP2ServiceClient();
            try
            {
            str = "AMERI201   02   " + AMERI201c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "AMERI201   02   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            AMGMO001.LP2ServiceClient AMGMO001c = new AMGMO001.LP2ServiceClient();
            try
            {
            str = "AMGMO001   03   " + AMGMO001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "AMGMO001   03   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            AMIST001.LP2ServiceClient AMIST001c = new AMIST001.LP2ServiceClient();
            try
            {
            str = "AMIST001   04   " + AMIST001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "AMIST001   04   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            APPRO001.LP2ServiceClient APPRO001c = new APPRO001.LP2ServiceClient();
            try
            {
            str = "APPRO001   05   " + APPRO001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "APPRO001   05   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ARCST001.LP2ServiceClient ARCST001c = new ARCST001.LP2ServiceClient();
            try
            {
            str = "ARCST001   06   " + ARCST001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ARCST001   06   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ASCEN001.LP2ServiceClient ASCEN001c = new ASCEN001.LP2ServiceClient();
            try
            {
            str = "ASCEN001   07   " + ASCEN001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ASCEN001   07   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ASCEN002.LP2ServiceClient ASCEN002c = new ASCEN002.LP2ServiceClient();
            try
            {
            str = "ASCEN002   08   " + ASCEN002c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ASCEN002   08   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ASSET005.LP2ServiceClient ASSET005c = new ASSET005.LP2ServiceClient();
            try
            {
            str = "ASSET005   09   " + ASSET005c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ASSET005   09   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            BANKE010.LP2ServiceClient BANKE010c = new BANKE010.LP2ServiceClient();
            try
            {
            str = "BANKE010   10   " + BANKE010c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "BANKE010   10   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            BANKE011.LP2ServiceClient BANKE011c = new BANKE011.LP2ServiceClient();
            try
            {
            str = "BANKE011   11   " + BANKE011c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "BANKE011   11   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            BELEM001.LP2ServiceClient BELEM001c = new BELEM001.LP2ServiceClient();
            try
            {
            str = "BELEM001   12   " + BELEM001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "BELEM001   12   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            BRIGH001.LP2ServiceClient BRIGH001c = new BRIGH001.LP2ServiceClient();
            try
            {
            str = "BRIGH001   13   " + BRIGH001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "BRIGH001   13   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            BUCKH001.LP2ServiceClient BUCKH001c = new BUCKH001.LP2ServiceClient();
            try
            {
            str = "BUCKH001   14   " + BUCKH001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "BUCKH001   14   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            COMMU100.LP2ServiceClient COMMU100c = new COMMU100.LP2ServiceClient();
            try
            {
            str = "COMMU100   15   " + COMMU100c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "COMMU100   15   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            CONTI001.LP2ServiceClient CONTI001c = new CONTI001.LP2ServiceClient();
            try
            {
            str = "CONTI001   16   " + CONTI001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "CONTI001   16   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            CORNE001.LP2ServiceClient CORNE001c = new CORNE001.LP2ServiceClient();
            try
            {
            str = "CORNE001   17   " + CORNE001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "CORNE001   17   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            CORNE010.LP2ServiceClient CORNE010c = new CORNE010.LP2ServiceClient();
            try
            {
            str = "CORNE010   18   " + CORNE010c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "CORNE010   18   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            CRLHO001.LP2ServiceClient CRLHO001c = new CRLHO001.LP2ServiceClient();
            try
            {
            str = "CRLHO001   19   " + CRLHO001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "CRLHO001   19   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            CROSS004.LP2ServiceClient CROSS004c = new CROSS004.LP2ServiceClient();
            try
            {
            str = "CROSS004   20   " + CROSS004c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "CROSS004   20   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            DALLA001.LP2ServiceClient DALLA001c = new DALLA001.LP2ServiceClient();
            try
            {
            str = "DALLA001   21   " + DALLA001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "DALLA001   21   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            DVFGM001.LP2ServiceClient DVFGM001c = new DVFGM001.LP2ServiceClient();
            try
            {
            str = "DVFGM001   22   " + DVFGM001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "DVFGM001   22   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            FARME007.LP2ServiceClient FARME007c = new FARME007.LP2ServiceClient();
            try
            {
            str = "FARME007   23   " + FARME007c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "FARME007   23   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            FLAGS002.LP2ServiceClient FLAGS002c = new FLAGS002.LP2ServiceClient();
            try
            {
            str = "FLAGS002   24   " + FLAGS002c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "FLAGS002   24   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            HERNA001.LP2ServiceClient HERNA001c = new HERNA001.LP2ServiceClient();
            try
            {
                str = "HERNA001   25   " + HERNA001c.GetVersion();

                Console.WriteLine(str);
                afile.WriteLine(str);
            }
            catch (Exception ex)
            {
                str = str = "HERNA001   25   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            HIGHP001.LP2ServiceClient HIGHP001c = new HIGHP001.LP2ServiceClient();
            try
            {
            str = "HIGHP001   26   " + HIGHP001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "HIGHP001   26   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            HILLH001.LP2ServiceClient HILLH001c = new HILLH001.LP2ServiceClient();
            try
            {
            str = "HILLH001   27   " + HILLH001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "HILLH001   27   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ICGHO001.LP2ServiceClient ICGHO001c = new ICGHO001.LP2ServiceClient();
            try
            {
            str = "ICGHO001   28   " + ICGHO001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ICGHO001   28   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            INVES003.LP2ServiceClient INVES003c = new INVES003.LP2ServiceClient();
            try
            {
            str = "INVES003   29   " + INVES003c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "INVES003   29   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            JMJFI001.LP2ServiceClient JMJFI001c = new JMJFI001.LP2ServiceClient();
            try
            {
            str = "JMJFI001   30   " + JMJFI001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "JMJFI001   30   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            MERID001.LP2ServiceClient MERID001c = new MERID001.LP2ServiceClient();
            try
            {
            str = "MERID001   31   " + MERID001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "MERID001   31   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            MONEY010.LP2ServiceClient MONEY010c = new MONEY010.LP2ServiceClient();
            try
            {
            str = "MONEY010   32   " + MONEY010c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "MONEY010   32   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            MORTG022.LP2ServiceClient MORTG022c = new MORTG022.LP2ServiceClient();
            try
            {
            str = "MORTG022   33   " + MORTG022c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "MORTG022   33   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            NSHPA001.LP2ServiceClient NSHPA001c = new NSHPA001.LP2ServiceClient();
            try
            {
            str = "NSHPA001   34   " + NSHPA001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "NSHPA001   34   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            OAKTR001.LP2ServiceClient OAKTR001c = new OAKTR001.LP2ServiceClient();
            try
            {
            str = "OAKTR001   35   " + OAKTR001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "OAKTR001   35   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PARAM004.LP2ServiceClient PARAM004c = new PARAM004.LP2ServiceClient();
            try
            {
            str = "PARAM004   36   " + PARAM004c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PARAM004   36   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PLANR001.LP2ServiceClient PLANR001c = new PLANR001.LP2ServiceClient();
            try
            {
            str = "PLANR001   37   " + PLANR001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PLANR001   37   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PLATI001.LP2ServiceClient PLATI001c = new PLATI001.LP2ServiceClient();
            try
            {
            str = "PLATI001   38   " + PLATI001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PLATI001   38   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PREFER001.LP2ServiceClient PREFER001c = new PREFER001.LP2ServiceClient();
            try
            {
            str = "PREFER001  39   " + PREFER001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PREFER001  39   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PREMI007.LP2ServiceClient PREMI007c = new PREMI007.LP2ServiceClient();
            try
            {
            str = "PREMI007   40   " + PREMI007c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PREMI007   40   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PROFI001.LP2ServiceClient PROFI001c = new PROFI001.LP2ServiceClient();
            try
            {
            str = "PROFI001   41   " + PROFI001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PROFI001   41   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            PURPO001.LP2ServiceClient PURPO001c = new PURPO001.LP2ServiceClient();
            try
            {
            str = "PURPO001   42   " + PURPO001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "PURPO001   42   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            REEVE001.LP2ServiceClient REEVE001c = new REEVE001.LP2ServiceClient();
            try
            {
            str = "REEVE001   43   " + REEVE001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "REEVE001   43   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            RELIA005.LP2ServiceClient RELIA005c = new RELIA005.LP2ServiceClient();
            try
            {
            str = "RELIA005   44   " + RELIA005c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "RELIA005   44   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            ROBER002.LP2ServiceClient ROBER002c = new ROBER002.LP2ServiceClient();
            try
            {
            str = "ROBER002   45   " + ROBER002c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "ROBER002   45   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            RUBIC001.LP2ServiceClient RUBIC001c = new RUBIC001.LP2ServiceClient();
            try
            {
            str = "RUBIC001   46   " + RUBIC001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "RUBIC001   46   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            SCOTT001.LP2ServiceClient SCOTT001c = new SCOTT001.LP2ServiceClient();
            try
            {
            str = "SCOTT001   47   " + SCOTT001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "SCOTT001   47   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            SETTL001.LP2ServiceClient SETTL001c = new SETTL001.LP2ServiceClient();
            try
            {
            str = "SETTL001   48   " + SETTL001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "SETTL001   48   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            SOUTH008.LP2ServiceClient SOUTH008c = new SOUTH008.LP2ServiceClient();
            try
            {
            str = "SOUTH008   49   " + SOUTH008c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "SOUTH008   49   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            SOUTH009.LP2ServiceClient SOUTH009c = new SOUTH009.LP2ServiceClient();
            try
            {
            str = "SOUTH009   50   " + SOUTH009c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "SOUTH009   50   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            SWANF001.LP2ServiceClient SWANF001c = new SWANF001.LP2ServiceClient();
            try
            {
            str = "SWANF001   51   " + SWANF001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "SWANF001   51   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            UNITE010.LP2ServiceClient UNITE010c = new UNITE010.LP2ServiceClient();
            try
            {
            str = "UNITE010   52   " + UNITE010c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "UNITE010   52   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            VANTA003.LP2ServiceClient VANTA003c = new VANTA003.LP2ServiceClient();
            try
            {
            str = "VANTA003   53   " + VANTA003c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "VANTA003   53   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            VELOC001.LP2ServiceClient VELOC001c = new VELOC001.LP2ServiceClient();
            try
            {
            str = "VELOC001   54   " + VELOC001c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "VELOC001   54   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            World005.LP2ServiceClient World005c = new World005.LP2ServiceClient();
            try
            {
            str = "World005   55   " + World005c.GetVersion();

            Console.WriteLine(str);
            afile.WriteLine(str);
            }
            catch( Exception ex )
            {
                str = str = "World005   55   " + ex.Message;
                Console.WriteLine(str);
                afile.WriteLine(str);
            }

            afile.Close();
            Console.ReadLine();
            
        }
    }
}
