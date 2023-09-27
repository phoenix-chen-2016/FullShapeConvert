// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<Test>();
//Console.WriteLine(Test.轉全形文字4("1234567890"));

public class Test
{
	private const string InputText = "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

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

	public static unsafe string 轉全形文字2(string input)
	{
		var a = new string(input);

		fixed (char* c = a)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (c[i] == 32)
				{
					c[i] = (char)12288; // Unicode 12288 是「全形空格字元」
				}
				else if (c[i] > 32 && c[i] < 127)
				{
					c[i] += (char)65248;
				}
			}

			return a;
		}
	}

	public static string 轉全形文字3(string input)
	{
		Span<char> c = stackalloc char[input.Length];

		input.AsSpan().CopyTo(c);

		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] == 32)
			{
				c[i] = (char)12288; // Unicode 12288 是「全形空格字元」
			}
			else if (input[i] > 32 && input[i] < 127)
			{
				c[i] = (char)(input[i] + 65248);
			}
		}

		return new string(c);
	}

	public static string 轉全形文字4(string input)
		=> string.Create(
			input.Length,
			input,
			(dst, src) =>
			{
				src.AsSpan().CopyTo(dst);

				for (int i = 0; i < dst.Length; i++)
				{
					if (dst[i] == (char)32)
					{
						dst[i] = (char)12288; // Unicode 12288 是「全形空格字元」
					}
					else if (dst[i] > (char)32 && dst[i] < (char)127)
					{
						dst[i] += (char)65248;
					}
				}
			});

	[Benchmark]
	public void Test1()
	{
		轉全形文字(InputText);
	}

	[Benchmark]
	public void Test2()
	{
		轉全形文字2(InputText);
	}

	[Benchmark]
	public void Test3()
	{
		轉全形文字3(InputText);
	}

	[Benchmark]
	public void Test4()
	{
		轉全形文字4(InputText);
	}
}
