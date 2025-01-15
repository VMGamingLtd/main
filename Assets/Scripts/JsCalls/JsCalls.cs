using System;
using System.Runtime.InteropServices;
namespace Js
{

	public class JsCalls
    {
        [DllImport("__Internal")]
        public static extern void Restart();
	}
}
