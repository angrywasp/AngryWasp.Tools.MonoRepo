using AngryWasp.Helpers;
using NBitcoin;
using Nethereum.Signer;

namespace AngryWasp.Cryptography
{
    public class Mnemonic
    {
        private Wordlist wordList;
        public Mnemonic(Wordlist wordList = null)
        {
            if (wordList == null)
                this.wordList = Wordlist.English;
            else
                this.wordList = wordList;
        }

        public EthECKey CreateWalletFromSeed(string mnemonicPhrase, int keyIndex, string keyPath = "m/44'/60'/0'/0")
        {
            try
            {
                var mnemonic = new NBitcoin.Mnemonic(mnemonicPhrase, wordList);
                var keyPathToDerive = KeyPath.Parse(keyPath);
                var pk = new NBitcoin.ExtKey(mnemonic.DeriveSeed().ToHex()).Derive(keyPathToDerive);
                NBitcoin.ExtKey keyNew = pk.Derive((uint)keyIndex);
                return new EthECKey($"0x{keyNew.PrivateKey.ToBytes().ToHex()}");
            }
            catch
            {
                return null;
            }
        }

        public string CreateNewSeed()
        {
            var seed = new NBitcoin.Mnemonic(wordList,
                AngryWasp.Cryptography.Helper.GenerateSecureBytes(16));

            var words = seed.Words;

            var returnValue = string.Empty;

            for (int i = 0; i < words.Length; i++)
                returnValue += $"{words[i]} ";

            return returnValue.TrimEnd(' ');
        }
    }
}