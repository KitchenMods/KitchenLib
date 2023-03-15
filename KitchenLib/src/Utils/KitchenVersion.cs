using System.Text.RegularExpressions;

namespace KitchenLib.Utils
{
	public class KitchenVersion
	{
		public int Major { get; private set; }
		public int Minor { get; private set; }
		public int Patch { get; private set; }
		public string Hotfix { get; private set; }
		public string Hash { get; private set; }

		private Regex pattern = new Regex(@"([0-9])\.([0-9])\.([0-9])([a-z]*) \(([^)]*)");
		private Regex pattern2 = new Regex("(\\d+)\\.(\\d+)\\.(\\d+)([a-z]*\\d+)?");

        public KitchenVersion(string versionString, BaseMod mod)
        {
            string VersionString = versionString;

			Match match = Regex.Match(versionString, pattern2.ToString());

            if (match.Success == false)
            {
                Major = 0;
                Minor = 0;
                Patch = 0;
                Hotfix = "";
                Hash = "XXXX";
                mod.Error("ERROR - KitchenVersion: Version string is not valid. Using default version.");
            }
            else
            {
                Major = int.Parse(match.Groups[1].Value);
                Minor = int.Parse(match.Groups[2].Value);
                Patch = int.Parse(match.Groups[3].Value);
                Hotfix = match.Groups[4].Value;
                Hash = match.Groups[5].Value;
            }
        }
    }
}
