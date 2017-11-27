using Xunit;
using ThatChat;

namespace ThatChat.Test
{
    public class TestAccount
    {
        [Theory]
        [InlineData("")]
        [InlineData("a")]
        public void InitializeMultipleAccounts_NameRepeated_NamesNotTheSame(string name)
        {
            Account acct0 = new Account(name);
            Account acct1 = new Account(name);

            Assert.NotEqual(acct0.Name, acct1.Name);
        }

        [Theory]
        [InlineData("")]
        public void InitializeAccount_NameInvalid_NameSetDefault(string name)
        {
            Account acct = new Account(name);

            Assert.NotEqual(acct.Name, name);
        }
    }
}
