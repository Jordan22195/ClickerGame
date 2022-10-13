using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scritps
{
    static class Globals
    {

        public static int[] baseDamageTable;

        public static int getBaseDamage()
        {
            return baseDamageTable[(int)ascensionLevel];
        }

        static Globals()
        {   
            ascensionLevel = ASCENSION_LEVEL.F;
            baseDamageTable = new int[(int)ASCENSION_LEVEL.COUNT]
                {
                    1,      //F
                    10,     //E
                    50,     //D
                    200,    //C
                    1000,   //B
                    8000,   //A
                    50000,  //S
                    300000, //SS
                    1500000 //SSS
                };
        }
        public enum ASCENSION_LEVEL
        {
            F = 0,
            E,
            D,
            C,
            B,
            A,
            S,
            SS,
            SSS,
            COUNT
        }

        public static string AscensionLevelToString(ASCENSION_LEVEL lvl)
        {
            switch (lvl)
            {
                case ASCENSION_LEVEL.A:
                    return "A";
                case ASCENSION_LEVEL.B:
                    return "B";
                case ASCENSION_LEVEL.C:
                    return "C";
                case ASCENSION_LEVEL.D:
                    return "D";
                case ASCENSION_LEVEL.E:
                    return "E";
                case ASCENSION_LEVEL.F:
                    return "F";
                case ASCENSION_LEVEL.S:
                    return "S";
                case ASCENSION_LEVEL.SS:
                    return "SS";
                case ASCENSION_LEVEL.SSS:
                    return "SSS";
            }
            return "";
        }
        public static ASCENSION_LEVEL ascensionLevel;
    }
}
