﻿namespace AutoMapper.UnitTests
{
    namespace ContextItems
    {
        using Should;
        using Xunit;

        public class When_mapping_with_contextual_values : AutoMapperSpecBase
        {
            private Dest _dest;

            public class Source
            {
                public int Value { get; set; }
            }

            public class Dest
            {
                public int Value { get; set; }
            }

            public class ContextResolver : IValueResolver
            {
                public ResolutionResult Resolve(ResolutionResult source)
                {
                    return source.New((int) source.Value + (int)source.Context.Options.Items["Item"]);
                }
            }

            [Fact]
            public void Should_use_value_passed_in()
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Source, Dest>()
                        .ForMember(d => d.Value, opt => opt.ResolveUsing<ContextResolver>().FromMember(src => src.Value));
                });

                var dest = Mapper.Map<Source, Dest>(new Source { Value = 5 }, opt => { opt.Items["Item"] = 10; });

                dest.Value.ShouldEqual(15);
            }
        }
    }
}