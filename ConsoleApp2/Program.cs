// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Test>();

public class Test
{
	public static string 轉全形文字(string input)
	{
		char[] c = input.ToCharArray();
		for (int i = 0; i < c.Length; i++)
		{
			// ASCII Code 32 是「空白字元」
			if (c[i] == 32)
			{
				c[i] = (char)12288; // Unicode 12288 是「全形空格字元」
				continue;
			}
			// ASCII 碼都加上 65248 就可以轉成全形字元
			else if (c[i] > 32 && c[i] < 127)
			{
				c[i] = (char)(c[i] + 65248);
			}
		}
		return new string(c);
	}

	public static string 轉全形文字2(string input)
	{
		unsafe
		{
			var a = new string(input);

			fixed (char* c = a)
			{
				for (int i = 0; i < input.Length; i++)
				{
					if (c[i] == 32)
					{
						c[i] = (char)12288; // Unicode 12288 是「全形空格字元」
						continue;
					}
					else if (c[i] > 32 && c[i] < 127)
					{
						c[i] += (char)65248;
					}
				}

				return a;
			}
		}
	}

	public static string 轉全形文字3(string input)
	{
		Span<char> c = stackalloc char[input.Length];

		for (int i = 0; i < input.Length; i++)
		{
			if (c[i] == 32)
			{
				c[i] = (char)12288; // Unicode 12288 是「全形空格字元」
				continue;
			}
			else if (c[i] > 32 && c[i] < 127)
			{
				c[i] += (char)65248;
			}
			else
			{
				c[i] = input[i];
			}
		}

		return c.ToString();
	}

	[Benchmark]
	public void Test1()
	{
		轉全形文字("1234567890");
	}

	[Benchmark]
	public void Test2()
	{
		轉全形文字2("1234567890");
	}

	[Benchmark]
	public void Test3()
	{
		轉全形文字3("1234567890");
	}
}
