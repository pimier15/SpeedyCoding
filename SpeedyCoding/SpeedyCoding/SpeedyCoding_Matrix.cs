using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Matrix
    {
        #region Distance
        public static double L1(
             this Tuple<int , int> @this ,
            Tuple<int , int> target
            )
        {
            return Math.Abs( @this.Item1 - target.Item2 );
        }

        public static double L2(
            this Tuple<int , int> @this ,
            Tuple<int , int> target
            )
        {
            return Math.Sqrt( ( @this.Item1 - target.Item1 ) ^ 2 + ( @this.Item2 - target.Item2 ) ^ 2 );
        }

        public static double L2(
            this Tuple<byte , byte> @this ,
            Tuple<byte , byte> target
            )
        {
            return Math.Sqrt( ( ( int )@this.Item1 - ( int )target.Item1 ) ^ 2 + ( ( int )@this.Item2 - ( int )target.Item2 ) ^ 2 );
        }
        #endregion

        #region Shape Convertion
        // Reshape Vector. Element is filled first by last order size 
        //  [ second ] x first  order 
        public static Tsrc [ , ] Reshape<Tsrc>(
            this Tsrc [ ] src
            , int first
            , int second
            )
        {
            var result = new Tsrc[first,second];
            int idx = 0;
            for ( int f = 0 ; f < first ; f++ )
            {
                for ( int s = 0 ; s < second ; s++ )
                {
                    result [ f , s ] = src [ idx++ ];
                }
            }
            return result;
        }

        // ( [ third ] x second ) x first order 
        public static Tsrc [ , , ] Reshape<Tsrc>(
            this Tsrc [ ] src
            , int first
            , int second
            , int third
            )
        {
            var result = new Tsrc[first,second,third];
            int idx = 0;
            for ( int f = 0 ; f < first ; f++ )
            {
                for ( int s = 0 ; s < second ; s++ )
                {
                    for ( int t = 0 ; t < third ; t++ )
                    {
                        result [ f , s , t ] = src [ idx++ ];
                    }
                }
            }
            return result;
        }

        // ( [ second ] x first )  order 
        public static Tuple<TSrc , int , int> [ ] ZipFlattenReshape<TSrc>(
          this TSrc [ ] @this
          , int row
          , int col )
        {
            if ( @this.GetLength( 0 ) != row * col ) return null;
            return Enumerable.Range( 0 , row )
                                    .SelectMany(
                                        j => Enumerable.Range( 0 , col ) ,
                                        ( j , i ) => Tuple.Create( @this [ j * col + i ] , j , i ) )
                                    .ToArray();
        }

        // ( [ third ] x second ) x first order 
        public static Tuple<TSrc , int , int , int> [ ] ZipFlattenReshape<TSrc>(
          this TSrc [ ] src
          , int first
          , int second
          , int third)
        {
            if ( src.GetLength( 0 ) != first * second * third ) return null;
            return Enumerable.Range( 0 , first )
                    .SelectMany( ( x , f ) => Enumerable.Range( 0 , second )
                                        .SelectMany( s => Enumerable.Range( 0 , third )
                                                     , ( s , t ) => Tuple.Create( src [ f * first + s * second + t ] , f , s , t ) ))
                    .ToArray();
        }

        //( [ col ] x rows )
        public static TSrc [ ] Flatten<TSrc>(
            this TSrc [ ] [ ] src )
        {
            int pagingSize = src[0].GetLength(0), pagingNum = src.GetLength(0);
            return src.SelectMany(
                            rows => rows ,
                            ( rows , col ) => col ).ToArray();
        }

        public static TSrc [ ] Flatten<TSrc>(
           this TSrc [ ] [ ] [ ] src )
        {
            int fsize = src[0].GetLength(0);
            int ssize = src[1].GetLength(0);
            int tsize = src[2].GetLength(0);


            int totalSize =  fsize + ssize + tsize;
            List<TSrc> result = new List<TSrc>();
            for ( int f = 0 ; f < fsize ; f++ )
            {
                for ( int s = 0 ; s < ssize ; s++ )
                {
                    for ( int t = 0 ; t < tsize ; t++ )
                    {
                        result.Add( src [f][s][t] );
                    }
                }
            }
            return result.ToArray();
        }

        public static TSrc [ ] [ ] Padding<TSrc>(
    this TSrc [ ] [ ] @this ,
    int padSize )
        {
            if ( padSize == 0 ) return @this;
            int rownum = @this.GetLength( 0 );
            int colnum = @this[0].GetLength( 0 );


            var basedata
                = new TSrc[rownum + padSize*2]
                     .Select( x
                         => new TSrc[colnum + padSize*2] )
                     .ToArray();
            for ( int j = padSize ; j < basedata.Len() - padSize ; j++ )
            {
                for ( int i = padSize ; i < basedata [ j ].Len() - padSize ; i++ )
                {
                    basedata [ j ] [ i ] = @this [ j - padSize ] [ i - padSize ];
                }
            }
            return basedata;
        }

        public static TSrc [ , ] Padding<TSrc>(
           this TSrc [ , ] @this ,
           int padSize )
        {
            if ( padSize == 0 ) return @this;

            var basedata
                = new TSrc[@this.GetLength( 0 ) + padSize*2,@this.GetLength(1) + padSize*2];

            for ( int j = padSize ; j < basedata.GetLength( 0 ) - padSize ; j++ )
            {
                for ( int i = padSize ; i < basedata.GetLength( 1 ) - padSize ; i++ )
                {
                    basedata [ j , i ] = @this [ j - padSize , i - padSize ];
                }
            }
            return basedata;
        }

        public static Array [ ] DCopy<TSrc>(
          this Array [ ] src )
        {
            Array[] output = new Array[src.GetLength(0)];
            for ( int i = 0 ; i < src.GetLength( 0 ) ; i++ )
            {
                output [ i ] = src [ i ];
            }
            return output;
        }

        public static TSrc [ ] Slice1D<TSrc>(
            this TSrc [ ] @this ,
            int start ,
            int end )
        {
            TSrc[] output = new TSrc[end - start];
            int count = 0;
            for ( int i = start ; i < end ; i++ )
            {
                output [ count ] = @this [ i ];
                count++;
            }
            return output;
        }

        public static TSrc [ ] [ ] Slice2D<TSrc>(
            this TSrc [ ] [ ] @this ,
            int [ ] yxStart ,
            int [ ] yxEnd )
        {
            TSrc[][] output = new TSrc[yxEnd[0] - yxStart[0]][];
            int countY = 0;
            int countX = 0;
            for ( int i = yxStart [ 0 ] ; i < yxEnd [ 0 ] ; i++ )
            {
                countX = 0;
                for ( int j = yxStart [ 1 ] ; j < yxEnd [ 1 ] ; j++ )
                {
                    output [ countY ] [ countX ] = @this [ j ] [ i ];
                    countX++;
                }
                countY++;
            }
            return output;
        }

        #endregion

        #region Concatenate
        public static T [ ] Concate_H<T>(
            this T [ ] src ,
            T [ ] targetRight )
        {
            var output = new T[src.Length + targetRight.Length];
            for ( int i = 0 ; i < src.Length ; i++ )
            {
                output [ i ] = src [ i ];
            }
            for ( int i = 0 ; i < targetRight.Length ; i++ )
            {
                output [ i + src.Length ] = targetRight [ i ];
            }
            return output;
        }

        public static List<T> Concate_H<T>(
            this List<T> src ,
            List<T> targetRight )
        {
            var output = new List<T>(src);
            output.AddRange( targetRight );
            return output;
        }


        public static T [ , , ] Concate_H<T>(
           this T [ , , ] src ,
           T [ , , ] targetRight )
            where T : new()
        {
            if ( src.GetLength( 0 ) == targetRight.GetLength( 0 )
                && src.GetLength( 2 ) == targetRight.GetLength( 2 ) )
            {
                int order0Len = src.GetLength(0);
                int order1Len = src.GetLength(1) + targetRight.GetLength(1);
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>( j =>
                {
                    for (int i = 0; i < src.GetLength(1); i++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = src[j, i, k];
                            }
                            catch (Exception e)
                            {
                                e.ToString().Print();
                            }
                        }
                    }

                    for (int i = src.GetLength(1); i < order1Len; i++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = targetRight[j, i -src.GetLength(1) , k];
                            }
                            catch (Exception e)
                            {
                                e.ToString().Print();
                            }
                        }
                    }
                });

                if ( order0Len * order1Len * order2Len > double.MaxValue )
                {
                    Parallel.For( 0 , order0Len , new Action<int>( j => {
                        act( j );
                    } ) );
                }
                else
                {
                    for ( int j = 0 ; j < order0Len ; j++ )
                    {
                        act( j );
                    }
                }
                return output;
            }
            Console.WriteLine( "Source and Target Length are not same" );
            return null;
        }


        public static T [ , , ] Concate_H<T>(
          this T [ , , ] src
           , T [ , , ] targetRight
           , int clipping )
           where T : new()
        {
            if ( src.GetLength( 0 ) == targetRight.GetLength( 0 )
                && src.GetLength( 2 ) == targetRight.GetLength( 2 ) )
            {
                int order0Len = src.GetLength(0);
                int order1Len = src.GetLength(1) + targetRight.GetLength(1) - 2* clipping;
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>( j =>
                {
                    for (int i = 0; i < src.GetLength(1) - clipping; i++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = src[j, i, k];
                            }
                            catch (Exception e)
                            {
                                e.ToString().Print();
                            }

                        }
                    }

                    for (int i = src.GetLength(1) - clipping ; i < order1Len; i++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = targetRight[j, i - src.GetLength(1) + clipping , k];
                            }
                            catch (Exception e)
                            {
                                e.ToString().Print();
                            }

                        }
                    }
                });

                if ( order0Len * order1Len * order2Len > double.MaxValue )
                {
                    Parallel.For( 0 , order0Len , new Action<int>( j => {
                        act( j );
                    } ) );
                }
                else
                {
                    for ( int j = 0 ; j < order0Len ; j++ )
                    {
                        act( j );
                    }
                }
                return output;
            }
            Console.WriteLine( "Source and Target Length are not same" );
            return null;
        }



        public static T [ , , ] Concate_V<T>(
           this T [ , , ] src ,
           T [ , , ] targetBottom )
            where T : new()
        {
            if ( src.GetLength( 1 ) == targetBottom.GetLength( 1 )
                && src.GetLength( 2 ) == targetBottom.GetLength( 2 ) )
            {
                int order0Len = src.GetLength(0) + targetBottom.GetLength(0);
                int order1Len = src.GetLength(1) ;
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>(i =>
                {
                    for (int j = 0; j < src.GetLength(0); j++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = src[j, i, k];
                            }
                            catch (Exception e)
                            {
                                e.ToString().Print();
                            }

                        }
                    }

                    for (int j = src.GetLength(0); j < order0Len; j++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            try
                            {
                                output[j, i, k] = targetBottom[j - src.GetLength(0) , i  , k];
                            }
                            catch (Exception e)
                            {
                                e.ToString();
                            }
                        }
                    }
                });

                if ( order0Len * order1Len * order2Len > double.MaxValue )
                {
                    Parallel.For( 0 , order1Len , new Action<int>( i => {
                        act( i );
                    } ) );
                }
                else
                {
                    for ( int i = 0 ; i < order1Len ; i++ )
                    {
                        act( i );
                    }
                }
                return output;
            }
            Console.WriteLine( "Source and Target Length are not same" );
            return null;
        }


        #endregion

        #region MinMax
        public static T MaxArray<T>(
            this T [ , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i => src [ j , i ] )
                             .Max() )
                    .Max();
        }

        public static T MaxArray<T>(
            this T [ , , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i =>
                                  Enumerable.Range( 0 , src.GetLength( 2 ) )
                                      .Select( k => src [ j , i , k ] )
                                      .Max() )
                             .Max() )
                    .Max();
        }

        public static T MinArray<T>(
            this T [ , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i => src [ j , i ] )
                             .Min() )
                    .Min();
        }

        public static T MinArray<T>(
          this T [ , , ] src )
          where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i =>
                                  Enumerable.Range( 0 , src.GetLength( 2 ) )
                                      .Select( k => src [ j , i , k ] )
                                      .Min() )
                             .Min() )
                    .Min();
        }
        #endregion

        #region Conversion
        public static TSrc [ , ] ToMat<TSrc>(
            this TSrc [ ] [ ] @this )
        {
            int rowL = @this.Len(0), colL = @this.Len(1);

            TSrc[,] output = new TSrc[rowL, colL];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                for ( int i = 0 ; i < colL ; i++ )
                {
                    output [ j , i ] = @this [ j ] [ i ];
                }
            }
            return output;
        }

        public static TSrc [ , , ] ToMat<TSrc>(
            this TSrc [ ] [ ] [ ] @this )
        {
            int rowL = @this.Len(0), fcolL = @this.Len(1), scolL = @this.Len(2);

            TSrc[,,] output = new TSrc[rowL, fcolL, scolL];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                for ( int i = 0 ; i < fcolL ; i++ )
                {
                    for ( int k = 0 ; k < scolL ; k++ )
                    {
                        output [ j , i , k ] = @this [ j ] [ i ] [ k ];
                    }
                }
            }
            return output;
        }

        public static TSrc [ ] [ ] [ ] ToJagged<TSrc>(
           this TSrc [ , , ] @this )
        {
            int rowL = @this.Len(0), fcolL = @this.Len(1), scolL = @this.Len(2);

            TSrc[][][] output = new TSrc[rowL][][];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                TSrc[][] second = new TSrc[fcolL][];
                for ( int i = 0 ; i < fcolL ; i++ )
                {
                    TSrc[] third = new TSrc[scolL];
                    for ( int k = 0 ; k < scolL ; k++ )
                    {
                        third [ k ] = @this [ j , i , k ];
                    }
                    second [ i ] = third;
                }
                output [ j ] = second;
            }
            return output;
        }


        public static TSrc [ ] [ ] AddVector<TSrc>( this TSrc [ ] [ ] @this , int len )
        {
            return @this.Select( x => new TSrc [ len ] ).ToArray();
        }



        public static T [ ] [ ] JArray<T>( this int @this , int col )
        {
            return Enumerable.Range( 0 , @this ).Select( x => new T [ col ] ).ToArray();
        }

        public static T [ ] [ ] [ ] JArray<T>( this int @this , int fcol , int scol )
        {
            return Enumerable.Range( 0 , @this )
                    .Select( fcols => Enumerable.Range( 0 , fcol )
                                        .Select( scols => new T [ scol ] )
                                        .ToArray() )
                    .ToArray();
        }

        public static double ToDegree( this double radian )
        {
            return radian * 180 / Math.PI;
        }

        #endregion

        #region Operation

        public static int [ ] Dot(
           this IEnumerable<int> @this ,
           IEnumerable<int> target )
        {
            return @this.Count() != target.Count() ? null
                : @this.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        public static double [ ] Dot(
            this IEnumerable<double> @this ,
            IEnumerable<double> target )
        {
            return @this.Count() != target.Count() ? null
                : @this.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        public static float [ ] Dot(
           this IEnumerable<float> @this ,
           IEnumerable<float> target )
        {
            return @this.Count() != target.Count() ? null
                : @this.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        // Dot 2D
        public static int [ ] [ ] Dot(
            this IEnumerable<IEnumerable<int>> @this ,
            IEnumerable<IEnumerable<int>> target )
        {
            return @this.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

        public static double [ ] [ ] Dot(
            this IEnumerable<IEnumerable<double>> @this ,
            IEnumerable<IEnumerable<double>> target )
        {
            return @this.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

        public static float [ ] [ ] Dot(
            this IEnumerable<IEnumerable<float>> @this ,
            IEnumerable<IEnumerable<float>> target )
        {
            return @this.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

        public static Tsrc [ ] [ ] PartialDot<Tsrc>(
   this Tsrc [ ] [ ] @this ,
   dynamic [ ] [ ] target ,
   int [ ] startYX ,
   int [ ] hw )
        {
            Tsrc[][] output = new Tsrc[target.GetLength(0)]
                                .Select(x => new Tsrc[target[0].GetLength(0)])
                                .ToArray();

            int county = 0 , countx = 0;

            for ( int j = startYX [ 0 ] ; j < startYX [ 0 ] + hw [ 0 ] ; j++ )
            {
                countx = 0;
                for ( int i = startYX [ 1 ] ; i < startYX [ 1 ] + hw [ 1 ] ; i++ )
                {
                    output [ county ] [ countx ] = target [ county ] [ countx ] * @this [ j ] [ i ];
                    countx++;
                }
                county++;
            }
            return output;
        }

        public static double [ ] [ ] PartialDot(
           this double [ ] [ ] @this ,
           double [ ] [ ] target ,
           int [ ] startYX ,
           int [ ] hw )
        {
            double[][] output = new double[target.GetLength(0)]
                                .Select(x => new double[target[0].GetLength(0)])
                                .ToArray();

            int county = 0 , countx = 0;

            for ( int j = startYX [ 0 ] ; j < startYX [ 0 ] + hw [ 0 ] ; j++ )
            {
                countx = 0;
                for ( int i = startYX [ 1 ] ; i < startYX [ 1 ] + hw [ 1 ] ; i++ )
                {
                    output [ county ] [ countx ] = target [ county ] [ countx ] * @this [ j ] [ i ];
                    countx++;
                }
                county++;
            }
            return output;
        }
        #endregion  

    }
}
