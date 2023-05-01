using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace ORMByExample.Benchmark
{
    [MinColumn, MaxColumn, MeanColumn, MedianColumn,Q1Column, Q3Column,MemoryDiagnoser()]
    public partial class StronglyTypedIdVsGuidPerformance
    {
        public const int countToGenerateGlobal = 100;
        [StronglyTypedId()]
        public partial struct StId { }

        [StronglyTypedId(
            backingType: StronglyTypedIdBackingType.Int, jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]
        public partial struct StIdIntJson { }

        [StronglyTypedId(
            backingType: StronglyTypedIdBackingType.Long, jsonConverter: StronglyTypedIdJsonConverter.NewtonsoftJson)]
        public partial struct StIdLongJson { }

        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public  List<StIdIntJson> StronglyTypedIdIntJson(int countOfGenerated)
        {
            List<StIdIntJson> ids = new List<StIdIntJson>(countOfGenerated);
            for (int i = 0; i < countOfGenerated; i++)
            {
                
                ids.Add(new StIdIntJson(i));
            }

            return ids;
        }
        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public  List<StIdLongJson> StronglyTypedIdLongJson(int countOfGenerated)
        {
            List<StIdLongJson> ids = new List<StIdLongJson>(countOfGenerated);
            for (long i = 0; i < countOfGenerated; i++)
            {
                
                ids.Add(new StIdLongJson(i));
            }

            return ids;
        }

        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public  List<StId> StronglyTypedId(int countOfGenerated)
        {
            List<StId> ids = new List<StId>(countOfGenerated);
            for (int i = 0; i < countOfGenerated; i++)
            {
                
                ids.Add(new StId(Guid.NewGuid()));
            }

            return ids;
        }

        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public  List<string> GuidIdAsString(int countOfGenerated)
        {
            List<string> ids = new List<string>(countOfGenerated);
            for (int i = 0; i < countOfGenerated; i++)
            {
                ids.Add(Guid.NewGuid().ToString());
            }

            return ids;
            
        }

        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public List<Guid> GuidId(int countOfGenerated)
        {
            List<Guid> ids = new List<Guid>(countOfGenerated);
            for (int i = 0; i < countOfGenerated; i++)
            {
                ids.Add(Guid.NewGuid());
            }

            return ids;
        }

        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public List<int> IntId(int countOfGenerated)
        {
            List<int> ids = new List<int>(countOfGenerated);
            for (int i = 0; i < countOfGenerated; i++)
            {
                ids.Add(i);
            }

            return ids;
        }
        [Benchmark]
        [Arguments(countToGenerateGlobal)]
        public List<long> LongId(int countOfGenerated)
        {
            List<long> ids = new List<long>(countOfGenerated);
            for (long i = 0; i < countOfGenerated; i++)
            {
                ids.Add(i);
            }

            return ids;
        }
    }
  
}
