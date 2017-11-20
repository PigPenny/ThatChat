using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ThatChat;

namespace ThatChat.test
{
    public class TestAppVar
    {
        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void InitializeMultipleAppVars_KeyRepeated_ArgumentException(string key)
        {
            new AppVar<string>(key);
            Action init = () => initial(key);

            Assert.Throws<ArgumentException>(init);
        }

        private void initial(string key)
        {
            new AppVar<string>(key);
        }
    }
}
