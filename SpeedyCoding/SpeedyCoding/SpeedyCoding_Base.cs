using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Base
    {
        #region Debug
        public static T Print<T>( this T src )
        {
            Console.WriteLine( src?.ToString() );
            return src;
        }

        public static T Print<T>( this T src , string msg )
        {
            Console.WriteLine( msg + " : " + src?.ToString() );
            return src;
        }

        public static T [ ] Print<T>( this T [ ] src )
        {
            if ( src == null ) return null;
            foreach ( var item in src )
            {
                Console.Write( item.ToString() + " " );
            }
            Console.WriteLine();
            return src;
        }

        public static T [ ] Print<T>( this T [ ] src , string msg )
        {
            if ( src == null ) return null;
            Console.Write( msg + " : " );
            foreach ( var item in src )
            {
                Console.Write( item.ToString() + " " );
            }
            Console.WriteLine();
            return src;
        }
        #endregion


        #region Function
        public static TResult Map<TSource, TResult>(
          this TSource src ,
          Func<TSource , TResult> fn )
          => fn( src );

        public static TResult [ ][ ] MapLoop<TSource, TResult>(
           this TSource [ ][ ] src
           , Func<TSource , TResult> fn )
        {
            return src.Select( row => 
                                row.Select( col => 
                                            fn( col ) )
                                   .ToArray())
                      .ToArray();
        }

        public static TResult [ ] [ ] [ ] MapLoop<TSource, TResult>(
           this TSource [ ] [ ] [ ] src
           , Func<TSource , TResult> fn )
        {
            return src.Select( first =>
                                first.Select( second =>
                                                second.Select( thrid =>
                                                        fn( thrid ) )
                                                      .ToArray() )
                                           
                                   .ToArray() )
                      .ToArray();
        }


        public static TResult [ , ] MapLoop<TSource, TResult>(
           this TSource [ , ] src
           , Func<TSource , TResult> fn )
        {
            int row = src.GetLength(0);
            int col = src.GetLength(1);
            TResult[,] result = new TResult[row,col];

            for ( int j = 0 ; j < row ; j++ )
            {
                for ( int i = 0 ; i < col ; i++ )
                {
                    result[j , i] = fn( src [ j , i ] );
                }
            }
            return result;
        }

        public static TResult [ , , ] MapLoop<TSource, TResult>(
           this TSource [ , , ] src
           , Func<TSource , TResult> fn )
        {
            int first = src.GetLength(0);
            int second = src.GetLength(1);
            int third = src.GetLength(2);
            TResult[,,] result = new TResult[first,second,third];

            for ( int j = 0 ; j < first ; j++ )
            {
                for ( int i = 0 ; i < second ; i++ )
                {
                    for ( int k = 0 ; k < third ; k++ )
                    {
                        result [ j , i ,k ] = fn( src [ j , i , k] );
                    }
                }
            }
            return result;
        }



        public static T Act<T>(
            this T src ,
            Action<T> action )
        {
            action( src );
            return src;
        }

        /// <summary>
        /// Need to select return type like Array or List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ActLoop<T>(
            this IEnumerable<T> src
            , Action<T> action )
        {
            foreach ( var item in src )
            {
                action( item );
            }
            return src;
        }

        public static IEnumerable<T> ActLoop<T>(
            this IEnumerable<T> src
            , Action<T , int> action )
        {
            for ( int i = 0 ; i < src.Count() ; i++ )
            {
                action( src.ElementAt( i ) , i );
            }
            return src;
        }

        public static T [ ][ ] ActLoop<T>(
            this T [ ][ ] src
            , Action<T> action )
            where T : struct
        {
            for ( int j = 0 ; j < src.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < src.GetLength( 1 ) ; i++ )
                {
                    action( src [ j ][ i ] );
                }
            }
            return src;
        }

        public static T [ ][][ ] ActLoop<T>(
            this T [ ][][ ] src
            , Action<T> action )
            where T : struct
        {
            for ( int j = 0 ; j < src.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < src.GetLength( 1 ) ; i++ )
                {
                    for ( int k = 0 ; k < src.GetLength( 0 ) ; k++ )
                    {
                        action( src [ j ][ i ][ k ] );
                    }
                }
            }
            return src;
        }

        public static T [ , ] ActLoop<T>(
            this T [ , ] src
            , Action<T> action )
            where T : struct
        {
            for ( int j = 0 ; j < src.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < src.GetLength( 1 ) ; i++ )
                {
                    action( src [ j , i ] );
                }
            }
            return src;
        }

        public static T [ , , ] ActLoop<T>(
            this T [ , , ] src
            , Action<T> action )
            where T : struct
        {
            for ( int j = 0 ; j < src.GetLength( 0 ) ; j++ )
            {
                for ( int i = 0 ; i < src.GetLength( 1 ) ; i++ )
                {
                    for ( int k = 0 ; k < src.GetLength(0) ; k++ )
                    {
                        action( src [ j , i , k] );
                    }
                } 
            }
            return src;
        }



        public static TSource Measure_Act<TSource>(
                this TSource src ,
                string msg ,
                int iter ,
                Action<TSource> fn )
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for ( int i = 0 ; i < iter ; i++ )
            {
                fn( src );
            }
            stw.Stop();
            Console.WriteLine( msg + $"{stw.ElapsedMilliseconds / 1.0}" );
            return src;
        }

        public static TResult Measure_Map<TSource, TResult>(
                this TSource src ,
                string msg ,
                int iter ,
                Func<TSource , TResult> fn )
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for ( int i = 0 ; i < iter ; i++ )
            {
                fn( src );
            }
            stw.Stop();
            Console.WriteLine( msg + $"{stw.ElapsedMilliseconds / 1.0}" );
            return fn( src );
        }

        public static TResult Measure<TSource, TSource2, TResult>(
            this TSource src ,
            string msg ,
            int iter ,
            TSource2 src2 ,
            Func<TSource , TSource2 , TResult> fn )
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for ( int i = 0 ; i < iter ; i++ )
            {
                fn( src , src2 );
            }
            stw.Stop();
            Console.WriteLine( $"{stw.ElapsedMilliseconds / 1.0}" + msg );
            return fn( src , src2 );
        }
        #endregion


        #region PatternMatch


        #endregion

        #region Length
        public static int Len<TSrc>(
            this IEnumerable<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
            this IList<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
            this ICollection<TSrc> src )
        {
            return src.Count();
        }

        public static int Len<TSrc>(
           this TSrc [ ] src ,
           int order = 0 )
        {
            return src.GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ ] [ ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src [ 0 ].GetLength( 0 );
            else return src [ 0 ].GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ , ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src.GetLength( 1 );
            else return src.GetLength( 0 );
        }


        public static int Len<TSrc>(
          this TSrc [ ] [ ] [ ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src [ 0 ].GetLength( 0 );
            if ( order == 2 ) return src [ 0 ] [ 0 ].GetLength( 0 );
            else return src [ 0 ] [ 0 ].GetLength( 0 );
        }

        public static int Len<TSrc>(
          this TSrc [ , , ] src ,
          int order = 0 )
        {
            if ( order == 0 ) return src.GetLength( 0 );
            if ( order == 1 ) return src.GetLength( 1 );
            if ( order == 2 ) return src.GetLength( 2 );
            else return src.GetLength( 0 );
        }
        #endregion  
    }
}

