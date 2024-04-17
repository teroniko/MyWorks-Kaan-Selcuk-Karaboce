// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("tMmwiFSHKytBl6qVogDxXhRWZYk5TAAv47DdXNddFoEXlX8JFD7UqEGgy2UEjecrrmQ3nwSL0hSAvrMrhyXhOf3r5/Ub3/qontTYkEXDaOT+fXN8TP59dn7+fX18+2vwuxKL21lYU9bDe9WEw76zQU6iOBIEDWlSUjvDI4v6trbnM7JgKW7g71hKuu28rBdoh82Bg5TZXpX+FEBqu4u26vIfu1Wz2rHW59Xm4rLoOFfYvQPJxcIw+XDIINFuzxXcFllFQB8QoCv/H9VLUBH52bAxOzfD4AUU5oQu93wtyE4NI2PGhiP0G90hWi1MmoknTP59XkxxenVW+jT6i3F9fX15fH8E+Lylk7/+kXoRptn9meyBpRvXfwkt0DPxx5LvPX5/fXx9");
        private static int[] order = new int[] { 9,6,3,10,6,11,13,10,12,13,12,11,13,13,14 };
        private static int key = 124;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
