using Xunit;
using System;
using FsCheck.Xunit;

namespace LaYumba.Functional.Tests
{
   using static F;

   public class OptionTest
   {
      [Theory]
      [InlineData("Hello", true)]
      [InlineData(null, false)]
      public void WhenLiftingNonNull_OptionIsSome(string s, bool expected)
         => Assert.Equal(expected, Some(s).IsSome);

      [Theory]
      [InlineData("John", "hello, John")]
      [InlineData(null, "sorry, who?")]
      public void MatchCallsAppropriateFunc(string name, string expected)
         => Assert.Equal(expected
            , Some(name).Match(
               Some: n => $"hello, {n}",
               None: () => "sorry, who?"));

      [Fact]
      public void NoneField_IsNone()
         => Assert.Equal(true
            , Option<string>.None.IsNone);
   }


   public class Option_Map_Test
   {
      class Apple { }
      class ApplePie { public ApplePie(Apple apple) { } }

      Func<Apple, ApplePie> makePie = apple => new ApplePie(apple);

      [Fact]
      public void GivenSomeApple_WhenMakePie_ThenSomePie()
      {
         var appleOpt = Option.Of(new Apple());
         var pieOpt = appleOpt.Map(makePie);
         Assert.True(pieOpt.IsSome);
      }

      [Fact]
      public void GivenNoApple_WhenMakePie_ThenNoPie()
      {
         var appleOpt = LaYumba.Functional.Option.Of((Apple)null);
         var pieOpt = appleOpt.Map(makePie);
         Assert.True(pieOpt.IsNone);
      }
   }

   public class Option_Apply_Test
   {
      Func<int, int, int> add = (a, b) => a + b;
      Func<int, int, int> multiply = (i, j) => i * j;

      [Fact]
      public void MapAndApplySomeArg_ReturnsSome()
      {
         var opt = Some(3)
             .Map(multiply)
             .Apply(Some(4));

         Assert.Equal(Some(12), opt);
      }

      [Property]
      public void MapAndApplyNoneArg_ReturnsNone(int i)
      {
         var opt = Some(i)
             .Map(multiply)
             .Apply(None);

         var opt2 = ((Option<int>)None)
             .Map(multiply)
             .Apply(i);

         Assert.Equal(None, opt);
         Assert.Equal(None, opt2);
      }

      [Fact]
      public void ApplySomeArgs()
      {
         var opt = Some(add)
             .Apply(Some(3))
             .Apply(Some(4));

         Assert.Equal(Some(7), opt);
      }

      [Property]
      public void ApplyNoneArgs(int i)
      {
         var opt = Some(add)
             .Apply(None)
             .Apply(Some(i));

         Assert.Equal(None, opt);
      }
   }
}