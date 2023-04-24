using System.Linq;
using DecisionSupport.PAM;

namespace DecisionSupportConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var algorithmPAM = new Algorithm();
            var clusters = algorithmPAM.GetClusters().FirstOrDefault();
        }
    }
}