using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
	/// <summary>
	/// A class that adds aditinal math functions.
	/// </summary>
    public static class Numbers
    {
		public static float Approach(float value, float target, float amount)
		{
			if (value < target)
			{
				value = Math.Min(value + amount, target);
			}
			else
			{
				value = Math.Max(value - amount, target);
			}
			return value;
		}

		public static int Approach(int value, int target, int amount)
		{
			if (value < target)
			{
				value = Math.Min(value + amount, target);
			}
			else
			{
				value = Math.Max(value - amount, target);
			}
			return value;
		}
	}
}
