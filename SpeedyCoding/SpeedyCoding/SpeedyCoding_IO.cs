using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_IO
    {
        public static string TrimBaseName( this string filepath )
        {
            return Path.GetFileName( filepath );
        }

        public static string TrimFileNameOnly( this string filepath )
        {
            return Path.GetFileName( filepath )
                       .Split( '.' )
                       .First();
        }

        public static string TrimDirPath( this string filepath )
        {
            return Path.GetDirectoryName( filepath );
        }
    }
}
