using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace _3
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length >= 3 && args.Length % 2 == 1)
            {
                string[] moves = args;

                if (moves.Length != moves.Distinct().Count()) {
                    Console.WriteLine("Moves have duplicates");
                    Environment.Exit(0);
                }

                var cmoveIndex = (new Random()).Next(moves.Length);
                
                string key = secretkey();

                Console.WriteLine("HMAC: {0}", hmac(moves[cmoveIndex], key));

                Console.WriteLine("Available moves:");
                
                for (int i = 0; i < moves.Length; i++)
                {
                    Console.WriteLine("{0} - {1}", i + 1, moves[i]);
                }

                Console.WriteLine("0 - exit");

                Console.WriteLine("Enter your move:");
                var umove = Console.ReadLine();

                if(string.IsNullOrEmpty(umove) || umove == "0") {
                    Environment.Exit(0);
                }

                var umoveIndex = Convert.ToInt32(umove) - 1;

                Console.WriteLine("Your move: {0}", moves[umoveIndex]);
                Console.WriteLine("Computer move: {0}", moves[cmoveIndex]);
                
                winornot(moves.Length, cmoveIndex, umoveIndex);

                Console.WriteLine("HMAC key: {0}", key);
            }
            else
            {
                Console.WriteLine("Please enter more than three, odd number of moves");
            }

            Console.ReadLine();
        }

        static string hmac(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty).ToLower();
            }
        }

        private static string secretkey()
        {
            byte[] byteArray = new Byte[32];
            
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(byteArray);
            }

            return Convert.ToBase64String(byteArray);
        }

        static void winornot(int movesLength, int cmoveIndex, int umoveIndex)
        {
            if(umoveIndex == cmoveIndex)
            {
                Console.WriteLine("Draw.");
            }
            else
            {
                bool win = false;

                if(cmoveIndex + ((movesLength - 1) / 2) + 1 > movesLength)
                {
                    win = (umoveIndex > cmoveIndex || (umoveIndex <= ((cmoveIndex + (movesLength - 1) / 2)) % movesLength));
                }
                else
                {
                    win = (umoveIndex > cmoveIndex && (umoveIndex <= (cmoveIndex + (movesLength - 1) / 2)));
                }

                Console.WriteLine(win ? "You win!" : "You lose!");
            }
        }
    }
}
